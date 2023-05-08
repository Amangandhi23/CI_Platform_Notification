
let ImgSelection = [];

let files = [],
    dragArea = document.querySelector('.drag-area'),
    input = document.querySelector('.drag-area input'),
    button = document.querySelector('.drag-card button'),
    select = document.querySelector('.drag-area .select'),
    container = document.querySelector('.container-img');

/ CLICK LISTENER /
select.addEventListener('click', () => input.click());

/ INPUT CHANGE EVENT /
input.addEventListener('change', () => {
    let file = input.files;

    // if user select no image
    if (file.length == 0) return;

    for (let i = 0; i < file.length; i++) {
        if (file[i].type.split("/")[0] != 'image') continue;
        if (!files.some(e => e.name == file[i].name)) files.push(file[i])
    }

    showImages();
    textareaval();
});

/ SHOW IMAGES /
function showImages() {
    container.innerHTML = files.reduce((prev, curr, index) => {
        return `${prev}
<div class="image">
<span onclick="delImage(${index})">&times;</span>
<img src="${URL.createObjectURL(curr)}" />
</div>`
    }, '');
    for (let i = 0; i < files.length; i++) {
        console.log(files[i].name)
    }
}

/ DELETE IMAGE /
function delImage(index) {
    files.splice(index, 1);
    showImages();
    textareaval();
}

/ DRAG & DROP /
dragArea.addEventListener('dragover', e => {
    e.preventDefault()
    dragArea.classList.add('dragover')
})

    /* DRAG LEAVE */
dragArea.addEventListener('dragleave', e => {
    e.preventDefault()
    dragArea.classList.remove('dragover')
});

/ DROP EVENT /
dragArea.addEventListener('drop', e => {
    e.preventDefault()
    dragArea.classList.remove('dragover');

    let file = e.dataTransfer.files;
    input.files = e.dataTransfer.files;
    for (let i = 0; i < file.length; i++) {
        / Check selected file is image /
        if (file[i].type.split("/")[0] != 'image') continue;

        if (!files.some(e => e.name == file[i].name)) files.push(file[i])
    }
    showImages();
    textareaval();
});

function textareaval() {
 
    const dt = new DataTransfer();
    for (let i = 0; i < files.length; i++) {
        dt.items.add(files[i]);
    }

    input.files = dt.files;


}




function Savestory() {

    var Missionid = $('#missionid').find(':selected').data('missionid');
    var title = $('#storytitle').val();
    var links = $('#utubeurl').val().trim();
    var Url = links.split("\n");
    let flag = 1;
    var storydesc = CKEDITOR.instances['content'].getData();
    if (true) {
        if (storydesc === undefined || storydesc === "") {
            $('#storyValidation').css('display', 'block');
            flag = 0;
        }
        if (title === undefined || title === "") {
            $('#TitleValidation').css('display', 'block');
            flag = 0;
        }
        else if (title.length > 255 || title.length < 4) {
            $('#TitleValidationlength').css('display', 'block');
            flag = 0;
        }
        if (Url.length > 20) {
            $('#utubelinkvalidation').css('display', 'block');
            flag = 0;
        }
        if (Missionid === undefined) {
            $('#MissionIdValidation').css('display', 'block');
            flag = 0;
        }
    }
    if(flag == 1) {
        var Missionid = $('#missionid').find(':selected').data('missionid');
        var title = $('#storytitle').val();
        var date = $('#storydate').val();
        var storydesc = CKEDITOR.instances['content'].getData();
        var links = $('#utubeurl').val().trim();
        var Url = links.split("\n");
        var Image = $('.ImgDraft').val();
        console.log(Image);


        var date = $('#storydate').val();
        var parts = date.split("-");
        var formattedDate = parts[1] + "/" + parts[2] + "/" + parts[0];
        console.log(formattedDate);
        console.log(date);
        

        var story_details = {
            MissionId: Missionid,
            Title: title,
            Date: date,
            Storydesc: storydesc,
            Videourl: Url,
            Image: ImgSelection,
            Date: formattedDate
        }
        var formData = new FormData();
        formData.append('story_details', JSON.stringify(story_details));

        var fileInput = $('#file')[0].files;
        var file;
        for (let i = 0; i < fileInput.length && i < 20; i++) {
            file = fileInput[i];
            formData.append('files', file);
        }

        $.ajax({
            url: '/Story/SaveStory',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                
                var link = '/Story/Storydetail?storyid=' + data;

                document.getElementById("submit_btn").disabled = false;
                $("#submit_btn").css("opacity", "1");
                document.getElementById("Preview").disabled = false;
                $("#Preview").css("display", "inline");
                console.log(data);
                $("#Storyidget").val(data);
                $('#Preview').attr('href', link);
            }

        });
    }
    console.log(Missionid);
    console.log(title);
    console.log(date);
}

function delImageSelect(id) {
    var valueToDelete = id;
    var index = ImgSelection.indexOf(valueToDelete);
    if (index > -1) {
        console.log("before" + ImgSelection);
        ImgSelection.splice(index, 1);
        console.log("After" + ImgSelection);
    }

    console.log("Deleting image with id: ", id);
    var element = $('.imageDraft[data-id="' + id + '"]');
    console.log("Element found: ", element.length > 0);
    element.css('display', 'none');
}

$('#missionid').on('change', function () {
    $('#TitleValidation').css('display', 'none');
    $('#MissionIdValidation').css('display', 'none');
    $('#utubelinkvalidation').css('display', 'none');
    if ($(this).val() === "-1") {
        $('#MissionIdValidation').css('display', 'block');
    }
    var MissionId = $(this).val();
    console.log(MissionId);
    $.ajax({
        url: '/Story/GetStory',
        data: { MissionId: MissionId },
        dataType: 'json',
        success: function (data) {
            console.log(data);

            if (data.length != 0 && data.length > 0) {
                document.getElementById("submit_btn").disabled = false;
                $("#submit_btn").css("display", "inline");
                console.log(data);
                console.log(data[0].date);
                $('#storytitle').val(data[0].title);
                if (data[0].date !== null) {
                    var date = new Date(data[0].date);
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
                    console.log(actualDate);
                    $('#storydate').val(actualDate);
                }
                CKEDITOR.instances['content'].setData(data[0].description);
                var mediapath = '';
                for (var i = 0; i < data[0].mediaPaths.length; i++) {
                    mediapath += data[0].mediaPaths[i] + `\n`;
                }
                $('#utubeurl').val(mediapath);


                ImgSelection.length = 0; // Clear contents
                var items = "";
                for (var i = 0; i < data[0].imagePath.length; i++) {
                    var url = "https://localhost:7023" + data[0].imagePath[i];
                    var newUrl = new URL(url);

                    // Extract the pathname
                    var pathname = newUrl.pathname;
                    items += `<div class="imageDraft" data-id="${pathname}">
                                <span class="CloseSpan" onclick="delImageSelect('${pathname}')">&times;</span>
                                <img class="ImgDraft" src="${pathname}" />
                              </div>`
                    ImgSelection.push(data[0].imagePath[i]);
                }
                $('#PreviewImages').html(items);
            }
            else {
                $('#storytitle').val('');
                CKEDITOR.instances['content'].setData();
                $('#utubeurl').val('');
                $('#storydate').val('');
                $('#PreviewImages').empty();
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
});


// Image Validation
var totalFiles = 0;

function validateFiles(input) {
    var numFiles = input.files.length;
    if (totalFiles + numFiles > 20) {
        alert("You can only upload a maximum of 20 files.");
        input.value = "";
    } else {
        totalFiles += numFiles;
    }
}

