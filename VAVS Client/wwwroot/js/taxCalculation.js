function changeUnit(element) {
    var inputValue = $(element).val().trim();
    var selectUnit = $('#select-unit');

    if (inputValue.length === 1) {
        selectUnit.val('1').trigger('change');
    } else if (inputValue.length === 2) {
        selectUnit.val('10').trigger('change');
    } else if (inputValue.length === 3) {
        selectUnit.val('100').trigger('change');
    } else if (inputValue.length === 4) {
        selectUnit.val('1000').trigger('change');
    } else if (inputValue.length === 5) {
        selectUnit.val('10000').trigger('change');
    } else {
        selectUnit.val('').trigger('change');
    }
}

function calculateFormula(value) {
    if (value < 1) return value;
    if (value <= 300000000) return value * 0.03;
    if (value <= 600000000) return (300000000 * 0.03) + ((value - 300000000) * 0.05);
    if (value <= 1000000000) return (300000000 * 0.03) + (300000000 * 0.05) + ((value - 600000000) * 0.1);
    if (value <= 3000000000) return (300000000 * 0.03) + (300000000 * 0.05) + (400000000 * 0.1) + ((value - 1000000000) * 0.15);
    return (300000000 * 0.03) + (300000000 * 0.05) + (400000000 * 0.1) + (2000000000 * 0.15) + ((value - 3000000000) * 0.3);
}

function calcTax(event) {
    event.preventDefault();
    const standardValue = parseFloat(document.getElementById("standardValue").value.replace(/,/g, '')) || 0;
    const price = parseFloat(document.getElementById("price").value.replace(/,/g, '')) || 0;
    const valueToCalculate = price > standardValue ? price : standardValue;
    const tax = calculateFormula(valueToCalculate);
    document.getElementById("totalTax").value = Math.round(tax).toString();
};

document.addEventListener("DOMContentLoaded", function () {
    console.log("here address class..................")
    var vehicleNumberCells = document.querySelectorAll(".address");
    var shortCells = document.querySelectorAll(".shortCell");
    vehicleNumberCells.forEach(function (cell) {
        var vehicleNumber = cell.textContent.trim();
        if (vehicleNumber.length > 25) {
            var splitLines = [];
            while (vehicleNumber.length > 25) {
                splitLines.push(vehicleNumber.substring(0, 25));
                vehicleNumber = vehicleNumber.substring(25);
            }
            splitLines.push(vehicleNumber);
            cell.innerHTML = splitLines.join("<br>");
        }
    });

    shortCells.forEach(function (cell) {
        var shortCell = cell.textContent.trim();
        if (shortCell.length > 9) {
            var splitLines = [];
            while (shortCell.length > 9) {
                splitLines.push(shortCell.substring(0, 9));
                shortCell = shortCell.substring(9);
            }
            splitLines.push(shortCell);
            cell.innerHTML = splitLines.join("<br>");
        }
    });
});