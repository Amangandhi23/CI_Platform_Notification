$(".goalbtn").on('click', function () {
    var id = $(this).val();
    $.ajax({
        url: '/Profile/GetTimeSheetData',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            console.log(data);
            console.log(data);
            $('.Goaltitle').val(data.title);
            $('.GoalTimesheetId').val(data.timeSheetId);
            if (data.type == "TIME") {
                $('.TimeHours').val(data.hours);
                $('.TimeMinutes').val(data.minutes);
            }
            else {
                $('.GoalAction').val(data.action);
            }
            var date = new Date(data.date);
            var date1 = date.getDate();
            let month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (date1 < 10) {
                date1 = "0" + date1;
            }
            if (month < 10) {
                month = "0" + month;
            }
            console.log(month);
            var actualDate = year + "-" + month + "-" + date1;
            $('.GoalDate').val(actualDate);
            $('.GoalMessage').val(data.message);

        }
    });
});

$('.deletebtn').on('click', function () {
    var timessheetId = $(this).val();
    $('.AddDeleteId').val(timessheetId);
});


function cleanTimedata() {
    $('.Adddate').val("");
    $('.AddHours').val("");
    $('.AddMin').val("");
    $('.AddAction').val("");
}