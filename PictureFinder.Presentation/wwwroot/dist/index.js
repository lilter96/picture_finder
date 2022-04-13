class DeleteClass{
 init() {
     var deleteInput = document.querySelector("#delete-input");

     if (deleteInput) {
         deleteInput.addEventListener("click",
             () => {
                 var deleteRequest = new XMLHttpRequest();
                 deleteRequest.onload = () => {
                     if (deleteRequest.status == 200) {
                         document.location.href = "/";
                     }
                 }
                 deleteRequest.open("PUT", "/Photo/Delete", true);
                 deleteRequest.send();
             });
     }
 }
}

