// Add to fav Mission
let addToFavourite = document.getElementsByClassName("AddFavButton");
for (var i = 0; i < addToFavourite.length; i++) {
    addToFavourite[i].addEventListener("click", addToFavouritefun);
}
function addToFavouritefun() {
   
    var text = event.target.getAttribute("value");
    var favarray = text.split(" ");
    let obj = {
        missionId: favarray[0],
        userId: favarray[1]
    };
    var url = "/Mission/AddToFavourite?favObj=" + JSON.stringify(obj);

    $.ajax({
        url: url,
        success: function (data) {
            window.location.reload();
        },
        error: function (error) {
            console.log(error);
        }
    });
}


// Recomended To Co-Workers
$('.recom').on('click', function () {
    var invite = $(this).val();
    var arr = invite.split(" ");

    var recommend = {
        MissionId: arr[0],
        UserId: arr[1],
        FromUserId: arr[2],
        UserEmail: arr[3]
    }
    url1 = "/Mission/Recommandation?recommend=" + JSON.stringify(recommend);
    $.ajax({
        url: url1,
        success: function (data) {
            /*window.location.reload();*/
            toastr.success("Email has been sent to your Co-Worker");
            $("#recommended").modal("hide");
        }
    });
    console.log(recommend);
});



//Rating Function
function ratingfunction() {
    let text = event.target.getAttribute("value");
    var arr = text.split(" ");

    var rating_detail = {
        Rating: arr[0],
        MissionId: arr[1],
        UserId: arr[2],
    }
    console.log(rating_detail);
    url = "/Mission/GetRating?rating_detail=" + JSON.stringify(rating_detail);
    $.ajax({
        url: url,
        success: function (data) {
            var rating = $('#For_rating');
            rating.empty();
            rating.html(data);
            console.log(data);
        }
    });
};



//  Comment Code
$('#Commentpost').on('click', function () {
    let text = event.target.getAttribute("value");
    var CommentText = document.getElementById("textcomment").value;
    var arr = text.split(" ");

    var Comment_details = {
        MissionId: arr[0],
        UserId: arr[1],
        commenttext: CommentText
    }
    url = "/Mission/GetComment?Comment_details=" + JSON.stringify(Comment_details);
    $.ajax({

        url: url,
        success: function (data) {
            var cmnt = $('#For_comment');
            cmnt.html(data);
            $('#textcomment').val("");

        }

    });
    console.log(CommentText);
});


// Apply Mission Button
$('#ApplyMission').on('click', function () {
    var apply = $(this).val();
    $.ajax({
        url: '/Mission/GetApaply',
        type: 'GET',
        data: { Apply: apply },
        success: function (data) {
            var temp = "";
            var applyToMissionText = $('#Apply_btn');
            temp += ` <a type="button" class="btn btn-outline-info  btn_sm w-25 h-auto" style="border-radius:24px">Panding</a>`;
            applyToMissionText.html(temp);
        }
    });
});

/*Recent Volunteeres Image And Name*/
let recent_vol = document.getElementsByClassName("recent-vol");
let prev_vol = document.getElementById("prev-vol");
let next_vol = document.getElementById("next-vol");
let page = 1;
let pageSize = 6;
let maxpages = Math.ceil(recent_vol.length / 6);
let recentvolpagenumber = document.getElementById("recentvolpagenumber");



recentpagination();

prev_vol.addEventListener("click", () => {
    if (page > 1) {
        page = page - 1;
    }
    recentpagination();
});

next_vol.addEventListener("click", () => {
    if (page != maxpages) {
        page = page + 1;
    }
    recentpagination();
});

function recentpagination() {
    for (i = 0; i < recent_vol.length; i++) {
        if (i < (page*pageSize) && i > (((page - 1)* pageSize) - 1)) {
            recent_vol[i].classList.remove("d-none");
        }
else {
            recent_vol[i].classList.add("d-none");
        }
    }
    recentvolpagenumber.innerHTML = `<span class="page-link" style="color:black">${((page - 1) * 6) + 1
        } - ${(page) * 6 < recent_vol.length ? (page) * 6 : recent_vol.length} of ${ recent_vol.length } Recent Volunteers</span > `
}