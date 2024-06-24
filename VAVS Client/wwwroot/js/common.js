$(document).ready(function () {

    var alertBox = $(".alert-msg");
    if (alertBox.length > 0) {
        setTimeout(function () {
            alertBox.fadeOut(500);
        }, 3000);
    }
});

document.addEventListener("DOMContentLoaded", function () {
    var inputs = document.querySelectorAll(".phone");
    console.log(inputs);
    inputs.forEach(function (input) {
        if (input != null) {
            var iti = window.intlTelInput(input, {
                initialCountry: "mm",
                separateDialCode: true,
                utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.15/js/utils.js",
            });
        }
    });
});


var xmlHttp;
function srvTime() {
    try {
        //FF, Opera, Safari, Chrome
        xmlHttp = new XMLHttpRequest();
    }
    catch (err1) {
        //IE
        try {
            xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
        }
        catch (err2) {
            try {
                xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
            }
            catch (eerr3) {
                console.log("AJAX not supported");
            }
        }
    }
    xmlHttp.open('HEAD', window.location.href.toString(), false);
    xmlHttp.setRequestHeader("Content-Type", "text/html");
    xmlHttp.send('');
    return xmlHttp.getResponseHeader("Date");
}

/*
*  Convert Currency to word 
*/
(function (global) {
    'use strict';

    function myanmarNumToWord() {
        var _myanmarNumToWord = {};

        var words = ['', 'တစ်', 'နှစ်', 'သုံး', 'လေး', 'ငါး', 'ခြောက်', 'ခုနှစ်', 'ရှစ်', 'ကိုး', 'တစ်ဆယ်'];
        var wordsConcat = words.slice(1).join('|');

        var numbers = {
            "၀": 0,
            "၁": 1,
            "၂": 2,
            "၃": 3,
            "၄": 4,
            "၅": 5,
            "၆": 6,
            "၇": 7,
            "၈": 8,
            "၉": 9
        };

        _myanmarNumToWord.convertToEnglishNumber = function (numberInput) {
            numberInput = numberInput.toString();

            numberInput = numberInput.replace(/([၁၂၃၄၅၆၇၈၉၀])ဝ/g, '$10');
            numberInput = numberInput.replace(/ဝ([၁၂၃၄၅၆၇၈၉၀])/g, '0$1');

            Object.keys(numbers).forEach(function (item) {
                const re = new RegExp(item, "g");
                numberInput = numberInput.replace(re, numbers[item]);
            });

            return numberInput;
        };

        _myanmarNumToWord.convertToBurmeseNumber = function (numberInput) {
            // convert to num to string
            numberInput = numberInput.toString();

            Object.keys(numbers).forEach(function (item) {
                const re = new RegExp(numbers[item], "g");
                numberInput = numberInput.replace(re, item);
            });

            return numberInput;
        };


        _myanmarNumToWord.convertToBurmeseWords = function (digitCurrency, wordType = 'written') {
            console.log("here convertToBurmeseWords")
            let num = digitCurrency.value.trim();
            let burmeseWordDisplayArea = document.getElementById("currencyToBurmeseWord");
            if (num == '') {
                digitCurrency.value = '';
                burmeseWordDisplayArea.style.display = "none";
            }
            if (!num) {
                digitCurrency.value = '';
                burmeseWordDisplayArea.style.display = "none";
                return num;
            }

            num = _myanmarNumToWord.convertToEnglishNumber(num);

            if (isNaN(num)) {
                digitCurrency.value = '';
                burmeseWordDisplayArea.style.display = "none";
                throw new Error('Invalid number to convert.');
            }

            if ((num = num.toString()).length > 10) {
                digitCurrency.value = '';
                burmeseWordDisplayArea.style.display = "none";
                return 'overflow';
            }
            var n = ('000000000' + num).substr(-10).match(/^(\d{1})(\d{1})(\d{1})(\d{2})(\d{1})(\d{1})(\d{1})(\d{2})$/);

            if (!n) {
                digitCurrency.value = '';
                burmeseWordDisplayArea.style.display = "none";
                return;
            }

            var upperLakh = '';
            var lowerLakh = '';
            upperLakh += (n[1] != 0) ? 'သိန်း' + words[n[1][0]] + 'သောင်း' : '';
            upperLakh += (n[2] != 0) ? ((upperLakh != '') ? '' : 'သိန်း') + words[n[2][0]] + 'ထောင်' : '';
            upperLakh += (n[3] != 0) ? ((upperLakh != '') ? '' : 'သိန်း') + words[n[3][0]] + 'ရာ' : '';

            if ((n[4] != 0)) {
                if (words[n[4][0]] && !words[n[4][1]]) {
                    upperLakh += ((upperLakh != '') ? '' : 'သိန်း') + words[n[4][0]] + 'ဆယ်';
                } else if (words[n[4][0]] || words[n[4][1]]) {
                    upperLakh += (words[Number(n[4])] || words[n[4][0]] + 'ဆယ်' + words[n[4][1]]) + 'သိန်း';
                }
            }

            lowerLakh += (n[5] != 0) ? (words[Number(n[5])]) + 'သောင်း' : '';
            lowerLakh += (n[6] != 0) ? (words[Number(n[6])]) + 'ထောင်' : '';
            lowerLakh += (n[7] != 0) ? (words[Number(n[7])]) + 'ရာ' : '';
            lowerLakh += (n[8] != 0) ? (words[Number(n[8])] || words[n[8][0]] + 'ဆယ်' + words[n[8][1]]) : '';

            var final = (upperLakh !== '' && lowerLakh !== '') ? upperLakh + ' နှင့် ' + lowerLakh : upperLakh + lowerLakh;

            const re = new RegExp("(ဆယ်(?=" + wordsConcat + "))|(ရာ(?=" + wordsConcat + "))|(ထောင်(?=" + wordsConcat + "))|(သောင်း)", 'gi');
            final = final.replace(re, function ($0) {
                if ($0 == "ရာ")
                    return (wordType == 'speech') ? "ရာ့ " : "ရာ ";
                else if ($0 == "ထောင်")
                    return (wordType == 'speech') ? "ထောင့် " : "ထောင် ";
                else if ($0 == "ဆယ်")
                    return "ဆယ့်";
                else
                    return $0 + ' ';
            });
            burmeseWordDisplayArea.style.display = "block";
            document.getElementById("currencyToBurmeseWord").innerHTML = final.trim()
            console.log("myanmar word: " + final);

            return final.trim();
        }
        return _myanmarNumToWord;
    }

    if (typeof define === 'function' && define.amd) {
        define(function () {
            return myanmarNumToWord();
        });

    } else if (typeof exports !== 'undefined') {
        if (typeof module !== 'undefined' && module.exports) {
            exports = module.exports = myanmarNumToWord();
        }
        exports.myanmarNumToWord = myanmarNumToWord();

    } else if (typeof (global.myanmarNumToWord) === 'undefined') {
        global.myanmarNumToWord = myanmarNumToWord();

    }

})(this); 

//console.log(myanmarNumToWord.convertToBurmeseWords(625000000));
function updateModalClass() {
    var modal = document.getElementById('registerModel');
    if (modal) { 
        if (window.innerWidth < 413) {
            modal.classList.add('modal-fullscreen-xxl-down');
            modal.classList.add('mt-3');
            modal.classList.remove('modal-xl');
        } else {
            modal.classList.add('modal-xl');
            modal.classList.remove('modal-fullscreen-xxl-down');
            modal.classList.remove('mt-3');
        }
    } else {
        console.error('Element with id "registerModel" not found.');
    }
}

updateModalClass();

window.addEventListener('resize', updateModalClass);
