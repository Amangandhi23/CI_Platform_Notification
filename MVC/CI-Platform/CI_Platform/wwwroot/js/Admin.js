
/************************************************************************* DATATABLES JS *************************************************************************/

$(document).ready(function () {
    $('.DataTable').DataTable({
        info: false,
        lengthChange: false,
        dom: '<"float-start"f><"#DataTablesId"t>i<"#paginatorId"lp>',
        pageLength: 5,
        responsive: true,
        deferLoading: 57,
        language: {
            searchPlaceholder: "Search records"
        },
    });
});


/************************************************************************* User PAGE JS *************************************************************************/
$('.CountrySelect').on('change', function () {
    var countryId = $('.CountrySelect').find(":selected").val();
    $.ajax({
        url: '/Mission/GetCityByCountry',
        type: 'GET',
        data: { CountryId: countryId },
        dataType: 'json',
        success: function (data) {
            var cities = $('.CityList');
            cities.empty();
            var items = "";
            $(data).each(function (i, item) {
                items += `<option value=` + item.cityId + `>` + item.name + `</option>`
            });
            cities.html(items);
        }
    });
});


function FillDeletedId1() {

    $.ajax({
        url: '/Admin/PartialViewForAddeditUser',
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('.UserAddData').empty();
            $('.UserAddData').html(data);
            $('.fname').val("");
            $('.lname').val("");
            $('.mail').val("");
            $('.phonenumber').val("");
            $('.emid').val("");
            $('.dname').val("");
            $('.userstatus').val("");
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function FillUserData(userid) {
    console.log(userid);


    
        $.ajax({
            url: '/Admin/FillUserData',
            type: 'POST',
            data: { userid: userid },
            success: function (data) {
                $('.UserAddData').empty();
                $('.UserAddData').html(data);
                console.log(data);
            },
            error: function (data) {
                debugger
                console.log(data);
            }

        });
    
    

    
}


function ReturnUserPage() {
    $.ajax({
        url: '/Admin/PartialViewForAdminUser',
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('.UserAddData').empty();
            $('.UserAddData').html(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
}


function AddUserData() {


    var FirstName = $('.fname').val();
    var LastName = $('.lname').val();
    var Email = $('.mail').val();
    var PhoneNumber = $('.phonenumber').val();
    var EmployeeId = $('.emid').val();
    var DepartmentName = $('.dname').val();
    var Status = $('.userstatus').val();
    var CityId = $('#CityList').val();
    var CountryId = $('#CountrySelect').val();
    var UserId = $('.UserId').val();

    var flag = true;

    if (FirstName == "") {
        document.getElementById("fnamespan").style.display = "block";
        flag = false;
    }
    if (LastName == "") {
        document.getElementById("lnamespan").style.display = "block";
        flag = false;
    }
    if (Email == "") {
        document.getElementById("emailspan").style.display = "block";
        flag = false;
    }
    if (PhoneNumber == "") {
        document.getElementById("phonenumberspan").style.display = "block";
        flag = false;
    }
    else if (PhoneNumber.length != 10) {
        $('#phonenumberspan').text('Phone Number Length Must Be 10!');
        document.getElementById("phonenumberspan").style.display = "block";
        flag = false;
    }
    if (EmployeeId == "") {
        document.getElementById("employeeidspan").style.display = "block";
        flag = false;
    }
    if (DepartmentName == "") {
        document.getElementById("dnamespan").style.display = "block";
        flag = false;
    }
    if (Status == "") {
        document.getElementById("statusspan").style.display = "block";
        flag = false;
    }
    if (Status != "0") {
        if (Status != 1) {
            $('#statusspan').text('Status Value 0 or 1 Only!');
            document.getElementById("statusspan").style.display = "block";
            flag = false;
        }
    }
    

    if (flag) {
        var formData = new FormData();
        formData.append('FirstName', FirstName);
        formData.append('LastName', LastName);
        formData.append('Email', Email);
        formData.append('PhoneNumber', PhoneNumber);
        formData.append('EmployeeId', EmployeeId);
        formData.append('DepartmentName', DepartmentName);
        formData.append('Status', Status);
        formData.append('CityID', CityId);
        formData.append('CountryId', CountryId);
        formData.append('UserId', UserId);
        $.ajax({
            url: '/Admin/AddUserData',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                $('.UserAddData').html(data);
                toastr.success("User Data Add Successfully!");
                console.log(data);
            },
            error: function (error) {
                toastr.success("Email or employeeid Already Register!");
            }

        });
    }



    
}



/************************************************************************* Mission PAGE JS *************************************************************************/
let files;
let documentfiles = [];
function AddEditMissionRedirect() {
    $.ajax({
        url: '/Admin/PartialViewForMission',
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('.MissionData').empty();
            $('.MissionData').html(data);
            CKEDITOR.replace('Mission_desc');
            CKEDITOR.replace('content');
            $('.Mission_startdate').val("");
            $('.Mission_enddate').val("");
            $('.Mission_deadline').val("");
            $('.Mission_sheets').val("");
            $('.Mission_action').val("");
        },
        error: function (error) {
            console.log(error);
        }
    });
}


function CountrySelected(countryid) {
    $.ajax({
        url: '/Mission/GetCityByCountry',
        type: 'GET',
        data: { CountryId: countryid },
        dataType: 'json',
        success: function (data) {
            console.log(data);
            var cities = $('.Mission_city');
            cities.empty();
            var items = "";
            $(data).each(function (i, item) {
                items += `<option value=` + item.cityId + `>` + item.name + `</option>`
            });
            cities.html(items);
        }
    });
}

function Divhide(str) {
    if (str == "GO") {
        $('.goalbaseddiv').css('display', 'inline');
        $('.timebaseddiv').css('display', 'none');
    } else {
        $('.goalbaseddiv').css('display', 'none');
        $('.timebaseddiv').css('display', 'inline');
    }
}

let Skillarr = [];
function Demofunction() {
    $('.Mission_skill').on('click', function () {
        console.log("hii");
        let sa = [];
        $('.Mission_skill').find('.classofskill:checked').each(function () {
            if (sa.indexOf(this.value) == -1) {
                sa.push(this.value);
            }
        });
        Skillarr.length = 0;
        Skillarr.push.apply(Skillarr, sa);
    });

}


function AddMissionData() {
    var missionId = $('.MissionId').val();
    var title = $('.Mission_title').val();
    var ShortDesc = $('.Mission_shortdesc').val();
    var Description = CKEDITOR.instances['Mission_desc'].getData();
    var OName = $('.Mission_orgname').val();
    var Odetail = CKEDITOR.instances['content'].getData();
    var startdate = $('.Mission_startdate').val();
    var enddate = $('.Mission_enddate').val();
    var missiontype = $('.Mission_type').val();
    var totalsheet = $('.Mission_sheets').val();
    var deadline = $('.Mission_deadline').val();
    var goalaction = $('.Mission_action').val();
    var goaltext = $('.Mission_goaltext').val();
    var availability = $('.Mission_availability').val();
    var status = $('.Mission_status').val();
    var themeid = $('.Mission_theme').val();
    var countryid = $('.Mission_country').val();
    var cityid = $('.Mission_city').val();
    var url = $('#utubeurl').val();
    var VideoUrl = url.split("\n");


    let flag = true;
    
    if (title.trim() == "") {
        document.getElementById("tspan").style.display = "block";
        flag = false;
    }
    if (ShortDesc.trim() == "") {
        document.getElementById("sdspan").style.display = "block";
        flag = false;
    }
    if (OName.trim() == "") {
        document.getElementById("onspan").style.display = "block";
        flag = false; 
    }
    if (startdate == "") {
        document.getElementById("startdspan").style.display = "block";
        flag = false; 
    }
    if (enddate == "") {
        document.getElementById("enddspan").style.display = "block";
        flag = false; 
    }
    if (startdate > enddate) {
        $('#enddspan').text('End date must be grater then startdate');
        document.getElementById("enddspan").style.display = "block";
        flag = false;
    }
    if (missiontype == "TIME") {
        if (totalsheet.trim() == "") {
            document.getElementById("sheetspan").style.display = "block";
            flag = false;
        }
        if (deadline.trim() == "") {
            document.getElementById("deadspan").style.display = "block";
            flag = false;
        }
    }
    else {
        if (goalaction.trim() == "") {
            document.getElementById("actionspan").style.display = "block";
            flag = false;
        }
        if (goaltext.trim() == "") {
            document.getElementById("goaltextspan").style.display = "block";
            flag = false;
        }
    }
    
    if (availability == "-1") {
        document.getElementById("avspan").style.display = "block";
        flag = false; 
    }
    if (themeid == "-1") {
        document.getElementById("themespan").style.display = "block";
        flag = false; 
    }
    if (countryid == "-1") {
        document.getElementById("countryspan").style.display = "block";
        flag = false; 
    }

    if (flag) {
        var Img = "";
        for (let i = 0; i < Imagesave.length; i++) {
            if (i == Imagesave.length - 1) {
                Img += Imagesave[i];
            }
            else {
                Img += Imagesave[i] + ",";
            }
        }


        var urlstr = "";
        for (let i = 0; i < VideoUrl.length; i++) {
            if (i == VideoUrl.length - 1) {
                urlstr += VideoUrl[i].trim();
            }
            else {
                urlstr += VideoUrl[i].trim() + ",";
            }
        }

        var str = "";


        for (let i = 0; i < Skillarr.length; i++) {
            if (i == Skillarr.length - 1) {
                str += Skillarr[i];
            }
            else {
                str += Skillarr[i] + ",";
            }
        }



        var doc = "";
        for (let i = 0; i < documentfiles.length; i++) {
            if (i == documentfiles.length - 1) {
                doc += documentfiles[i];
            }
            else {
                doc += documentfiles[i] + ",";
            }
        }

        var formData = new FormData();
        formData.append('Title', title);
        formData.append('ShortDescription', ShortDesc);
        formData.append('Description', Description);
        formData.append('OrganizationName', OName);
        formData.append('OrganizationDetail', Odetail);
        formData.append('StartDate', startdate);
        formData.append('EndDate', enddate);
        formData.append('MissionType', missiontype);
        formData.append('TotalSheet', totalsheet);
        formData.append('GoalValue', goalaction);
        formData.append('GoalObjectiveText', goaltext);
        formData.append('Availability', availability);
        formData.append('Status', status);
        formData.append('CityID', cityid);
        formData.append('CountryId', countryid);
        formData.append('ThemeId', themeid);
        formData.append('SkillIdArr', str);
        formData.append('VideoUrlString', urlstr);
        formData.append('Imagedraft', Img);
        formData.append('MissionId', missionId);
        formData.append('deadLine', deadline);
        formData.append('Documentstr', doc);


        var imgfiles = $('#file')[0].files;
        var file;
        for (let i = 0; i < imgfiles.length; i++) {
            file = imgfiles[i];
            formData.append('Images', file);
        }

        var docfiels = $('#document')[0].files;
        var dfile;
        for (let i = 0; i < docfiels.length; i++) {
            dfile = docfiels[i];
            formData.append('Document', dfile);
        }

        $.ajax({
            url: '/Admin/AddEditMissionData',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                console.log(data);
                $('.MissionData').empty();
                $('.MissionData').html(data);
                toastr.success("Mission Data Add Successfully!");
                documentfiles.length = 0;
            }
        });
    }
    else {
        CKEDITOR.replace('Mission_desc');
        CKEDITOR.replace('content');
    }







    
}


function GetMissionData(MissionId) {
    $.ajax({
        url: '/Admin/GetMissionDataForEdit',
        type: 'POST',
        data: {MissionId:MissionId},
        success: function (data) {
            console.log(data);
            $('.MissionData').empty();
            $('.MissionData').html(data);
            CKEDITOR.replace('Mission_desc');
            CKEDITOR.replace('content');
            var str = $('.Mission_type').val();
            Divhide(str);
        },
        error: function (error) {
            console.log(error);
        }
    });
}


function ReturnMissionPage() {
    $.ajax({
        url: '/Admin/PartialViewForAdminMission',
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('.MissionData').empty();
            $('.MissionData').html(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
}





















/************************************************************************* Theme PAGE JS *************************************************************************/

function AddthemeData() {
    var themename = $('#themename').val();
    let flag = true;
    if (themename == "") {
        alert("Theme Name Is Required !");
        flag = false
    }

    if (flag) {
        $.ajax({
            url: '/Admin/ThemeDataAdd',
            type: 'POST',
            data: { ThemeName: themename },
            success: function (data) {
                $('.ThemeAddData').html(data);
                toastr.success("Theme Add Successfully!");
                console.log(data);
            }

        });
    }

    
}

function themestatus(statusval, Id) {
    var status = statusval;
    var ThemeId = Id;

    console.log(status);
    $.ajax({
        url: '/Admin/ThemeStatusCheck',
        type: 'POST',
        data: { status: status, id: ThemeId },
        success: function (data) {
            $('.ThemeAddData').html(data);

            if (status == "0") {
                toastr.info("Theme deactivated!");
            }
            else {
                toastr.success("Theme activated!");
            }
            console.log(data);
        }

    });
}



/************************************************************************* Skill PAGE JS *************************************************************************/


function AddSkillData() {
    var skillname = $('#skillname').val();

    let flag = true;
    if (skillname == "") {
        alert("Skill Name Is Required !");
        flag = false
    }


    $.ajax({
        url: '/Admin/SkillDataAdd',
        type: 'POST',
        data: { SkillName: skillname },
        success: function (data) {
            $('.SkillAddData').html(data);
            toastr.success("Skill Add Successfully!");
            console.log(data);
        }

    });
}

function skillstatus(statusval, Id) {
    var status = statusval;
    var Skillid = Id;

    console.log(status);
    $.ajax({
        url: '/Admin/SkillStatusCheck',
        type: 'POST',
        data: { status: status, id: Skillid },
        success: function (data) {
            $('.SkillAddData').html(data);
            if (status == "0") {
                toastr.info("Skill deactivated!");
            }
            else {
                toastr.success("Skill activated!");
            }
            console.log(data);
        }

    });
}




/************************************************************************* STORY PAGE JS *************************************************************************/
function StstusCheckForStory(storystatus, id) {
    $.ajax({
        url: '/Admin/StoryStatusCheck',
        type: 'POST',
        data: { status: storystatus, Storyid: id },
        success: function (data) {
            $('.Storyforajax').empty();
            $('.Storyforajax').html(data);
            console.log(data);
        }
    });
}

function Viewdetail(id) {
    $.ajax({
        url: '/Admin/StoryDetailPage',
        type: 'POST',
        data: {Storyid: id },
        success: function (data) {
            $('.Storyforajax').empty();
            $('.Storyforajax').html(data);
            console.log(data);
        }

    });
}








/************************************************************************* MissionApplication PAGE JS *************************************************************************/
function statusbtn(str, id) {
    var status = str;
    var MissionApplicationid = id;

    console.log(status);
    $.ajax({
        url: '/Admin/MissionApplicationStatusCheck',
        type: 'POST',
        data: { status: status, id: MissionApplicationid },
        success: function (data) {
            $('.Missionapplicationforajax').empty();
            $('.Missionapplicationforajax').html(data);
            console.log(data);
        }
    });

}



/************************************************************************* Baneer JS *************************************************************************/

function chooseImageBanner() {
    $('#bannerimg1').click();
}
function ReturnBannerPage() {
    $.ajax({
        url: '/Admin/ReturnBanner',
        type: 'GET',
        success: function (data) {
            $('.Bnaaerpartialview').empty();
            $('.Bnaaerpartialview').html(data);
            console.log(data);
        }
    });
}


function GetDataBanner(id) {
    $.ajax({
        url: '/Admin/GetBannerData',
        type: 'GET',
        data: { BannerId: id },
        success: function (data) {
            $('.Bnaaerpartialview').empty();
            $('.Bnaaerpartialview').html(data);
            console.log(data);
        }
    });
}


function AddEditBannnerOpen() {
    $.ajax({
        url: '/Admin/AddEditViewOpen',
        type: 'GET',
        success: function (data) {
            $('.Bnaaerpartialview').empty();
            $('.Bnaaerpartialview').html(data);
            console.log(data);
        }
    });
}

let uploadBtn;
let chosenImage;
let ImageArrayForBanner = [];
var ImgPathForBanner = [];
function AddBannerData() {
    var text = $('.btext').val();
    var sortorder = $('.bannersortorder').val();


    let flag = true;
    if (text == "") {
        // Display error messages
        document.getElementById("bannertextspan").style.display = "block";
        flag = false; // Prevent form submission
    }
    if (sortorder == "") {
        // Display error messages
        document.getElementById("bannersortspan").style.display = "block";
        flag = false; // Prevent form submission
    }


    if (flag) {
        var formData = new FormData();
        formData.append('Text', $('.btext').val());
        formData.append('SortOrder', $('.bannersortorder').val());
        formData.append('BannerId', $('.bennerid').val());


        console.log($('.bannersortorder').val());
        console.log($('.btext').val());

        var bannerimgfile = ImageArrayForBanner[0];
        if (ImageArrayForBanner.length == 0) {
            formData.append('BannerImg', ImgPathForBanner[0]);
        }
        else {
            formData.append('BannerImg1', bannerimgfile);
        }


        $.ajax({
            url: '/Admin/AddEditBannerData',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                $('.Bnaaerpartialview').empty();
                $('.Bnaaerpartialview').html(data);
                toastr.success("Banner Data Add Successfully!");
                console.log(data);
            }
        });
    }
}








/************************************************************************* Table Items Deleted JS *************************************************************************/

function FillDeletedId(id) {
    $('.AddDeleteId').val(id);
}


function ItemDeleted(str) {
    var id = $('.AddDeleteId').val();
    console.log(id);
    $.ajax({
        url: '/Admin/DeletedItems',
        type: 'POST',
        data: { Itemid: id, Table: str },
        success: function (data) {
            console.log(data);
            if (str == "Story") {
                $('.Storyforajax').empty();
                $('.Storyforajax').html(data);
                toastr.success("User Story Deleted Successfully!");
            }
            else if (str == "User") {
                $('.UserAddData').empty();
                $('.UserAddData').html(data);
                toastr.success("User data Deleted Successfully!");
            }
            else if (str == "CMS") {
                $('.CMSData').empty();
                $('.CMSData').html(data);
                toastr.success("CMS data Deleted Successfully!");
            }
            else if (str == "Mission") {
                $('.MissionData').empty();
                $('.MissionData').html(data);
                toastr.success("Mission Deleted Successfully!");
            }
            else if (str == "Banner") {
                $('.Bnaaerpartialview').empty();
                $('.Bnaaerpartialview').html(data);
                toastr.success("Banner data Deleted Successfully!");
            }

            console.log(data);
        }
    });
}




/************************************************************************* CMS Page JS *************************************************************************/

function AddDataCMS() {
    $.ajax({
        url: '/Admin/PartialViewForCms',
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('.CMSData').empty();
            $('.CMSData').html(data);
            CKEDITOR.replace('content');
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function AddEditCMSData() {

    var title = document.getElementById("cmstitle").value;
    var slug = document.getElementById("cmsslug").value;
    var flag = true;
    // Check if any fields are empty
    if (title == "") {
        // Display error messages
        document.getElementById("cmstitlespan").style.display = "block";
        flag = false; // Prevent form submission
    }
    if (slug == "") {
        // Display error messages
        document.getElementById("cmsslugspan").style.display = "block";
        flag = false; // Prevent form submission
    }
    if (flag) {
        /*var title = $('#cmstitle').val();*/
        var discription = CKEDITOR.instances['content'].getData();
        /*var slug = $('#cmsslug').val();*/
        var status = $('#cmsstatus').val();
        var cmsId = $('#cmsId').val();
        console.log(cmsId);
        $.ajax({
            url: '/Admin/AddEditCmsData',
            type: 'POST',
            data: { title: title, discription: discription, slug: slug, status: status, cmsId: cmsId },
            success: function (data) {
                console.log(data);
                $('.CMSData').empty();
                $('.CMSData').html(data);
                if (cmsId == "0") {
                    toastr.success("Cms Page data Add Successfully!");
                }
                else {
                    toastr.success("Cms Page data Edit Successfully!");
                }
                
            },
            error: function (error) {
                toastr.success("Slug already exsist please use another slug");
            }
        });
        console.log(status);
    }

}

function HideErrorMessage(elementID) {
    document.getElementById(elementID).style.display = "none";
}

function ReturnCmsPage() {
    $.ajax({
        url: '/Admin/PartialViewForAdminCms',
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('.CMSData').empty();
            $('.CMSData').html(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function EditCms(id) {
    console.log(id);
    console.log(id);
    $.ajax({
        url: '/Admin/PartialViewForCms',
        type: 'POST',
        data: { id: id },
        success: function (data) {
            console.log(data);
            $('.CMSData').empty();
            $('.CMSData').html(data);
            CKEDITOR.replace('content');
            
        },
        error: function (error) {
            console.log(error);
        }
    });
}