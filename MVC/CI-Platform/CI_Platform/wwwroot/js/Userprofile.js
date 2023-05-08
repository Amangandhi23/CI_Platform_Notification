$('#CountrySelect').on('change', function () {
    var countryId = $('#CountrySelect').find(":selected").val();
    $.ajax({
        url: '/Mission/GetCityByCountry',
        type: 'GET',
        data: { CountryId: countryId },
        dataType: 'json',
        success: function (data) {
            var cities = $('#CityList');
            cities.empty();
            var items = "";
            $(data).each(function (i, item) {
                items += `<option value=` + item.cityId + `>` + item.name + `</option>`
            });
            cities.html(items);
        }
    });
});


let uploadBtn = document.getElementById("file");
let chosenImage = document.getElementById("ImageChooseFile");
uploadBtn.onchange = () => {
    let reader = new FileReader();
    reader.readAsDataURL(uploadBtn.files[0]);
    console.log(uploadBtn.files[0]);
    console.log(chosenImage);
    chosenImage.setAttribute("src", URL.createObjectURL(uploadBtn.files[0]));
    
}

$('#DeleteImg').on('click', function () {
    chosenImage.setAttribute("src", "/Assest/user1.png");
});

let SkillArr = [];
$(function () {

    $('body').on('click', '.list-group .list-group-item', function () {
        $(this).toggleClass('active');
    });
    $('.list-arrows button').click(function () {
        var $button = $(this), actives = '';
        if ($button.hasClass('move-left')) {
            actives = $('.list-right ul li.active');
            actives.clone().appendTo('.list-left ul');
            for (let i = 0; i < actives.length; i++) {
                let index = SkillArr.indexOf(actives[i].innerText);
                if (index !== -1) {
                    SkillArr.splice(index, 1);
                }
            }

            actives.remove();
        } else if ($button.hasClass('move-right')) {
            actives = $('.list-left ul li.active');
            actives.clone().appendTo('.list-right ul');
            for (let i = 0; i < actives.length; i++) {
                let index = SkillArr.indexOf(actives[i].innerText);
                if (index === -1) {
                    SkillArr.push(actives[i].innerText);
                }
            }
            actives.remove();
        }
    });
    $('.dual-list .selector').click(function () {
        var $checkBox = $(this);
        if (!$checkBox.hasClass('selected')) {
            $checkBox.addClass('selected').closest('.well').find('ul li:not(.active)').addClass('active');
            $checkBox.children('i').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
        } else {
            $checkBox.removeClass('selected').closest('.well').find('ul li.active').removeClass('active');
            $checkBox.children('i').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
        }
    });
    $('[name="SearchDualList"]').keyup(function (e) {
        var code = e.keyCode || e.which;
        if (code == '9') return;
        if (code == '27') $(this).val(null);
        var $rows = $(this).closest('.dual-list').find('.list-group li');
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
        $rows.show().filter(function () {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
    });

});


$(document).ready(function () {
    $('.SkillAdd .btn-primary').click(function () {
        $('.skillAddInTextarea').empty();
        var skills = "";
        $('.list-right li').each(function () {
            skills += $(this).text().trim() + '\n';
        });
        $('.skillAddInTextarea').val(skills.trim());

    });
});


function Validateform() {
    var oldpwd = $('#oldpwd').val();
    var newpwd = $('#newpwd').val();
    var cnewpwd = $('#cnewpwd').val();

    if (oldpwd === "" || oldpwd === undefined) {
        $('#ValidationForOldPassword').css('disply', 'block');
    }
    if (newpwd === "" || newpwd === undefined) {
        $('#ValidationForNewPassword').css('disply', 'block');
    }
    if (cnewpwd === "" || cnewpwd == undefined) {
        $('#ValidationForConfirmNewPassword').css('disply', 'block');
    }
}


