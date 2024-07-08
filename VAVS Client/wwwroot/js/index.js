$(document).ready(function () {
    $('.selectpicker').selectpicker();
    $('.bs-searchbox input').attr('placeholder', 'ယာဥ်အမျိုးအစား ထည့်ပါ');
    $('.dropdown').addClass("col-12");
    $('.bs-searchbox input').on('input', function () {
        var searchValue = $(this).val();
        var result = null;
        if (searchValue != null && searchValue != '' && searchValue.length >= 3) {
            $.getJSON("/IRD_VAVS_Client/VehicleStandardValue/GetMadeModel", { searchString: searchValue }, function (models) {
                if (models != null && !jQuery.isEmptyObject(models)) {
                    $('.selectpicker').empty(); 
                    $.each(models, function (index, model) {
                        //console.log("model ......" + model);
                        $('.selectpicker').append('<option>' + model + '</option>');
                    });
                    $('.selectpicker').selectpicker('refresh'); 
                }
            });
        }
        result = $('.dropdown-menu .active .text').text();
        console.log("Selected Option Text1: " + result);
        result = $('.filter-option-inner-inner').text();
        console.log("Selected Option Text3: " + result);

        result = $('.selectpicker').find("option:selected").text();
        console.log("Selected Option Text4: " + result);
        console.log("result null..................................." + (result !== null) + " / " + (result !== 'undefined') + "/" + (result !== ""))
        if (result !== null && result !== "" && result !== "undefined") {
            $.getJSON("/IRD_VAVS_Client/VehicleStandardValue/GetBrandNames", { madeModel: result }, function (brands) {
                console.log("here1 .............................................");
                if (brands != null && !jQuery.isEmptyObject(brands)) {
                    $('#carBrand').empty();
                    $('#carBrand').append('<option value="">Select Brand</option>');
                    $.each(brands, function (index, brand) {
                        $('#carBrand').append('<option value="' + brand + '">' + brand + '</option>');
                    });
                }
            });
        }
        
        function getSelectedText() {
            var selectedText = $('.selectpicker').find("option:selected").text();
            return selectedText;
        }

        getSelectedText();
        $('.selectpicker').on('changed.bs.select', function (e, clickedIndex, isSelected, previousValue) {
            var result1 = getSelectedText();
            console.log("result1 null..................................." + (result1 !== null) + " / " + (result1 !== 'undefined') + "/" + (result1 !== ""))

            if (result1 !== null && result1 !== "" && result1 !== "undefined") {
                $.getJSON("/IRD_VAVS_Client/VehicleStandardValue/GetBrandNames", { madeModel: result1 }, function (brands) {
                    console.log("here2 .............................................");

                    if (brands != null && !jQuery.isEmptyObject(brands)) {
                        $('#carBrand').empty();
                        $('#carBrand').append('<option value="">Select Brand</option>');
                        $.each(brands, function (index, brand) {
                            $('#carBrand').append('<option value="' + brand + '">' + brand + '</option>');
                        });
                    }
                });
            }
            console.log("select change............." + result1);
        }); 
    });

    
   
});
function getModelYearByManufacturerAndBrands() {
    console.log("here oncha.ge...........................")
    var manufacturer = document.getElementById("madeModel").value;
    var brand = document.getElementById("carBrand").value;
    console.log("manufacturer.................." + manufacturer);
    console.log("brand.................." + brand);

    if (manufacturer !== null && manufacturer !== "" && manufacturer !== "undefined") {
        $.getJSON("/IRD_VAVS_Client/VehicleStandardValue/GetMadeModelYearByManufacturerAndBrand", { madeModel: manufacturer, brandName: brand }, function (years) {
            console.log("here2 .............................................");

            if (years != null && !jQuery.isEmptyObject(years)) {
                $('#modelYear').empty();
                $('#modelYear').append('<option value="">Select Year</option>');
                $.each(years, function (index, year) {
                    $('#modelYear').append('<option value="' + year + '">' + year + '</option>');
                });
            }
        });
    }
}