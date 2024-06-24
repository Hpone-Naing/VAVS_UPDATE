const inputs = document.getElementById("inputs");
if (inputs != null) {
    inputs.addEventListener("input", function (e) {
        const target = e.target;
        const val = target.value;

        if (isNaN(val)) {
            target.value = "";
            return;
        }

        if (val != "") {
            const next = target.nextElementSibling;
            if (next) {
                next.focus();
            }
        }
    });

    inputs.addEventListener("keyup", function (e) {
        const target = e.target;
        const key = e.key.toLowerCase();

        if (key == "backspace" || key == "delete") {
            target.value = "";
            const prev = target.previousElementSibling;
            if (prev) {
                prev.focus();
            }
            return;
        }
    });

}


function initializeRemainingTime() {
    var remainingTime = document.getElementById('remainingTime');
    if (remainingTime != null) {
        var expireTimeString = remainingTime.getAttribute('data-expire-time');
        console.log("expire time str:............... " + expireTimeString)
        var remainingTimeElement = document.getElementById('remainingTime');

        function updateCountdown() {
            console.log("innerHtml" + remainingTimeElement.innerHTML);
            var remainingTimeInSeconds;
            console.log("true or false html" + (remainingTimeElement.innerHTML === null) + "/" + (remainingTimeElement.innerHTML === ""));
            if (remainingTimeElement.innerHTML !== "") {
                remainingTimeInSeconds = parseInt(remainingTimeElement.innerHTML);
                remainingTimeInSeconds--;
            } else {
                var expireTime = new Date(expireTimeString);
                //var currentTime = new Date();
                var currentTime = new Date(srvTime());
                console.log("current time: " + currentTime);
                console.log("expire time: " + expireTime);
                remainingTimeInSeconds = Math.floor((expireTime - currentTime) / 1000);
            }
            console.log("remainingTimeInSeconds" + remainingTimeInSeconds);
            remainingTimeElement.innerHTML = remainingTimeInSeconds;


            if (remainingTimeInSeconds <= 10) {
                remainingTimeElement.style.color = 'red';
            } else {
                remainingTimeElement.style.color = 'green';
            }
            if (remainingTimeInSeconds >= 0) {
                setTimeout(updateCountdown, 1000);
            } else {
                remainingTimeElement.innerHTML = "Time out!";

                document.getElementById('resendCode').style.display = "block";
                document.getElementById('signinBtnDiv').style.display = "none";
                document.getElementById('otpContainer').style.display = "none";
            }
        }
        updateCountdown();
    }
    
}
initializeRemainingTime();
