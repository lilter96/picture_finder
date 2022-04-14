using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PictureFinder.Integration.Telegram;

namespace PictureFinder.Presentation.HostedService
{
    public class TunnelService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<TunnelService> _logger;
        private readonly IServer _server;

        public TunnelService(
            IServer server,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<TunnelService> logger,
            IConfiguration configuration)
        {
            _server = server;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await WaitForApplicationStarted();

            var urls = _server.Features.Get<IServerAddressesFeature>()!.Addresses;

            var localUrl = urls.Single(u => u.StartsWith("https://"));

            _logger.LogInformation("Starting ngrok tunnel for {LocalUrl}", localUrl);
            var ngrokTask = StartNgrokTunnel(localUrl);

            var publicUrl = await GetNgrokPublicUrl();
            _logger.LogInformation("Public ngrok URL: {NgrokPublicUrl}", publicUrl);

            await ConfigureTelegramBotWebhook(publicUrl);

            await ngrokTask;

            _logger.LogInformation("Ngrok tunnel stopped");
        }

        private Task WaitForApplicationStarted()
        {
            var completionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _hostApplicationLifetime.ApplicationStarted.Register(() => completionSource.TrySetResult(true));
            return completionSource.Task;
        }

        private Task<int> StartNgrokTunnel(string localUrl)
        {
            var p = Process.GetProcessesByName("ngrok").FirstOrDefault();
            if (p != null)
            {
                p.Kill();
                _logger.LogInformation("ngrok.exe killed!");
            }

            var taskCompletionSource = new TaskCompletionSource<int>();
            var psi = new ProcessStartInfo("Ngrok/ngrok",
                $"http {localUrl} -host-header=\"{localUrl.Replace("https://", string.Empty)}\"")
            {
                CreateNoWindow = true
            };

            var proc = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            proc.Exited += (sender, args) =>
            {
                taskCompletionSource.SetResult(proc.ExitCode);
                proc.Dispose();
            };

            proc.Start();

            return taskCompletionSource.Task;
        }

        private async Task<string> GetNgrokPublicUrl()
        {
            using var httpClient = new HttpClient();
            for (var ngrokRetryCount = 0; ngrokRetryCount < 10; ngrokRetryCount++)
            {
                _logger.LogDebug("Get ngrok tunnels attempt: {RetryCount}", ngrokRetryCount + 1);

                try
                {
                    var response =
                        await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                            "http://127.0.0.1:4040/api/tunnels"));

                    var jsonResponseString = await response.Content.ReadAsStringAsync();

                    var jObject = JObject.Parse(jsonResponseString);

                    var publicUrlToken = jObject.First.First.First.ToList()[2];
                    var publicUrl = publicUrlToken.ToObject<string>();

                    if (!string.IsNullOrEmpty(publicUrl)) return publicUrl;
                }
                catch
                {
                    // ignored
                }

                await Task.Delay(200);
            }

            throw new Exception("Ngrok dashboard did not start in 10 tries");
        }

        public async Task ConfigureTelegramBotWebhook(string publicUrl)
        {
            var telegramWebhookUrl = publicUrl + "/telegram";

            _logger.LogDebug("Telegram webhook url {webhook}", telegramWebhookUrl);


            using var httpClient = new HttpClient();

            var setWebhookUrl =
                _configuration["TelegramBot:BaseBotApiUrl"] +
                _configuration["TelegramBot:ApiKey"]
                + "/"
                + string.Format(TelegramBotApiUrls.SetWebhook, telegramWebhookUrl);

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, setWebhookUrl));
            var jsonResponseString = await response.Content.ReadAsStringAsync();

            var responseObject = JObject.Parse(jsonResponseString);

            var status = responseObject.First.ToObject<bool>();

            if (status)
            {
                _logger.LogInformation("Webhook was set on the url via {setUrl}", setWebhookUrl);
            }
            else
            {
                _logger.LogCritical("Webhook was not ser");
                throw new Exception("The webhook was not set.");
            }
        }
    }
}