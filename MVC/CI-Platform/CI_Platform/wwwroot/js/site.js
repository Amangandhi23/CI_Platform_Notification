

/*This Arrays Created For Filtering(Store Value In this Arrays)*/

let Countryarr = [];
let Cityarr = [];
let Themearr = [];
let Skillarr = [];
let Sortarr = [];
let pagenumber;
let valueofview =""; // this is for grid =view and list view...while applying filter we want if list view active then list view not change

/*MissionCount Print */
$(document).ready(function () {
    changecount();
});

function changecount() {
    setTimeout(function () {
        document.getElementById("countm").innerHTML = $('#PartialMissionCount').text() + " " + "Missions";
    }, 100);
}



/*Country By City And City Filter functionality*/
$('.country_filter li').on('click', function () {

    let ca = [];
    var CountryId = $(this).attr('value');

    if (CountryId == "0") {
        Countryarr = [];
        $.ajax({
            url: '/Mission/GetCityByCountryAll',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var cities = $('.cities');
                cities.empty();
                var items = "";
                $.each(data, function (i, item) {
                    items += `<li class="ms-2"><input type="checkbox" class="form-check-input me-3 clscity"  id="exampleCheck1" value=` + item.name + `><label class="form-check-label" for="exampleCheck1" >` + item.name + `</label></li>`

                });
                cities.html(items);
            }
        });
    }
    else {
        var lenght = CountryId.lenght;
        var countryName = CountryId.substring(2, lenght);
        var index = ca.indexOf(countryName);
        console.log(CountryId.substring(2, lenght));
        if (index == -1) {
            ca.push(CountryId.substring(2, lenght));
        }
        $.ajax({
            url: '/Mission/GetCityByCountry',
            type: 'GET',
            data: { CountryId: CountryId.substring(0, 1) },
            dataType: 'json',
            success: function (data) {
                var cities = $('.cities');
                cities.empty();
                var items = "";
                $.each(data, function (i, item) {
                    items += `<li class="ms-2"><input type="checkbox" class="form-check-input me-3 clscity"  id="exampleCheck1" value=` + item.name + `><label class="form-check-label" for="exampleCheck1" >` + item.name + `</label></li>`

                });
                cities.html(items);
            }
        });

        Countryarr.length = 0;                  // Clear contents
        Countryarr.push.apply(Countryarr, ca);  // Append new contents
    }
    $('#filter_btn').click();
});



/*City Filter*/

$('.cities').on('change', function () {
    let ca = [];

    console.log(Cityarr.length);
    $('.cities').find('.clscity:checked').each(function () {
        console.log(ca.indexOf(this.value) + "value of index");
        if (ca.indexOf(this.value) == -1) {
            ca.push(this.value);
        }
    });
    Cityarr.length = 0;
    Cityarr.push.apply(Cityarr, ca);
    console.log(Cityarr);
    $('#filter_btn').click();
});


/*Theme Filter*/

$('.theme').on('change', function () {
    let ta = [];
    $('.theme').find('.clstheme:checked').each(function () {
        if (ta.indexOf(this.value) == -1) {
            ta.push(this.value);
        }
    });
    Themearr.length = 0;
    Themearr.push.apply(Themearr, ta);
    console.log(Themearr.length + " this is lenght of arr");
    $('#filter_btn').click();
});



/*Skill Filter*/

$('.skill').on('change', function () {
    let sa = [];
    $('.skill').find('.clsskill:checked').each(function () {
        if (sa.indexOf(this.value) == -1) {
            sa.push(this.value);
        }
    });
    Skillarr.length = 0;
    Skillarr.push.apply(Skillarr, sa);
    $('#filter_btn').click();
});



/*Filter Fnctionality With Sorting And Searching */
$('#filter_btn').on('click', function () {
    console.log("hii");
    console.log(Countryarr);
    console.log(Cityarr);
    console.log(Themearr);
    console.log(Skillarr);
    $.ajax({
        url: '/Mission/GetAllFilterData',
        type: 'GET',
        traditional: true,
        data: {
            country: Countryarr,
            city: Cityarr,
            skill: Skillarr,
            theme: Themearr,
            Sort: Sortarr,
            search: input,
            page: pagenumber
        },
        dataType: 'html',

        success: function (data) {
            changecount();
            if (data.length == 0) {
                $('#MissionNotFound').show();
            } else {
                $('#MissionNotFound').hide();
                $('#pid').html(data);
            }

            if (valueofview == "list") {
                $('#list_btn_click').click();
            }

        }

    });
});



/*If MissionNot Available Then use this btn to Reload Page Reload*/
$("#Reload_page").click(function () {
    location.reload(true);
});

function PageChangejs(pageNumber) {
    pagenumber = pageNumber;
    $('#filter_btn').click();
}



/*Code For Pills*/

let searchedFilters = $("#clear-item");
let allDropdowns = $('.dropdown ul');
allDropdowns.each(function () {
    let dropdown = $(this);
    $(this).on('change', 'input[type="checkbox"]', function () {
        if ($(this).is(':checked')) {
            let selectedOptionText = $(this).next('label').text();
            let selectedOptionValue = $(this).val();
            let index;
            const closeAllButton = searchedFilters.children('#remove');
            let pill = $('<div></div>').addClass('pill me-1 d-inline');
            let pillText = $('<span></span>').text(selectedOptionText);
            pill.append(pillText);
            let closeIcon = $('<span></span>').addClass('close').html('x');
            pill.append(closeIcon);
            console.log(pillText);
            closeIcon.click(function () {
                const pillToRemove = $(this).closest('.pill');
                console.log(selectedOptionValue);
                pillToRemove.remove();
                for (var i = 0; i < Themearr.length; i++) {
                    if (Themearr[i] === selectedOptionValue) {
                        index = Themearr.indexOf(selectedOptionValue);
                        Themearr.splice(index, index + 1);
                    }
                }
                for (var i = 0; i < Cityarr.length; i++) {
                    if (Cityarr[i] === selectedOptionValue) {
                        index = Cityarr.indexOf(selectedOptionValue);
                        Cityarr.splice(index, index + 1);
                    }
                }
                for (var i = 0; i < Skillarr.length; i++) {
                    if (Skillarr[i] === selectedOptionValue) {
                        index = Skillarr.indexOf(selectedOptionValue);
                        Skillarr.splice(index, index + 1);
                    }
                }
                console.log(pillText);
                console.log("this is value of pill" + pillText.value);
                console.log("this is value of pill" + Themearr);
                $('#filter_btn').click();
                const checkboxElement = dropdown.find(`input[type="checkbox"][value="${selectedOptionValue}"]`);
                checkboxElement.prop('checked', false);
                if (searchedFilters.children('.pill').length === 1) {
                    searchedFilters.children('#remove').remove();
                }
            });
            if (closeAllButton.length === 0) {
                searchedFilters.append(`<div class="col-auto d-inline border p-1" style="border-radius: 20px;" id="remove">
                                        <a class="btn btn-close-search clr-btn p-1" >
                                        Clear all
                                        </a>
                                        <span aria-hidden="true">×</span>
                                        </div>`);
                $('#remove').click(function () {
                    allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                    searchedFilters.empty();
                    Cityarr = [];
                    Themearr = [];
                    Skillarr = [];
                    Countryarr = [];
                    $('#filter_btn').click();
                });
                searchedFilters.prepend(pill);

            }
            else {
                searchedFilters.children('#remove').before(pill);
            }
        }
        else {
            let selectedOptionText = $(this).next('label').text() + 'x';
            let selectedOptionValue = $(this).val();
            $('.pill').each(function () {
                const pillText = $(this).text();
                if (pillText === selectedOptionText) {
                    $(this).remove();
                }
            });
            if ($('.pill').length == 0) {
                $('#remove').remove();
            }
        }
    });
});


/*Sorting Functionality*/

$('#SortBy button').on('click', function () {
    var SortValue = $(this).attr('value');
    let sortarr = [];
    sortarr.push(SortValue);
    Sortarr.length = 0;
    Sortarr.push.apply(Sortarr, sortarr);
    $('#filter_btn').click();
});


/*Code For Searching */


let searchbar = document.getElementById("Srch_bar_js");
let searchsmall = document.getElementById("search_div_small");

var input;
searchbar.addEventListener("input", search_mission);
searchsmall.addEventListener("input", search_mission);

function search_mission() {
    input = searchbar.value || searchsmall.value;
    input = input.toLowerCase();
    $('#filter_btn').click();
}

