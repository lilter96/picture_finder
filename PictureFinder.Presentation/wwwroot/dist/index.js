class DeleteClass {
    init() {
        var deleteInput = document.querySelector("#delete-input");
        var tagNameInput = document.querySelector("#tagName-input");

        if (deleteInput) {
            deleteInput.addEventListener("click",
                () => {
                    var deleteRequest = new XMLHttpRequest();
                    deleteRequest.onload = () => {
                        if (deleteRequest.status == 200) {
                            document.location.href = "/";
                        }
                    };
                    deleteRequest.open("PUT", "/Photo/Delete", true);
                    deleteRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
                    console.log(tagNameInput.value);
                    deleteRequest.send(JSON.stringify({
                        Tag: tagNameInput.value
                    }));
                });
        }
    }
}