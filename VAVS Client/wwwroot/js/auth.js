function hi(event, imagePreviewId) {
    console.log("hello");
    var e = event.target;
    var file = e.files[0];
    var reader = new FileReader();

    reader.onload = function (event) {
        var image = document.createElement('img');
        image.src = event.target.result;
        image.classList.add("object-fit-fill");
        image.style.height = "130px";
        var preview = document.getElementById(imagePreviewId);
        preview.innerHTML = '';
        preview.appendChild(image);
    };
    reader.readAsDataURL(file);
}
function CheckRealUser() {
    console.log("here checkreal user");
    let vehicleNumber = $("#vehicleNumber").val();
    let chassisNumber = $("#chassisNumber").val();
    console.log("v no / c no: " + vehicleNumber + " / " + chassisNumber);
    let IsRealUser = true;
        if (IsRealUser) {
            console.log("is real user is true");
            $("#search").modal('hide');
            $(".modal-backdrop").hide();
            $("#register").modal('show');
        }
}
