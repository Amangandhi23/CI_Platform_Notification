let recent_vol = document.getElementsByClassName("story_card");
let prev_vol = document.getElementById("prev-vol");
let next_vol = document.getElementById("next-vol");
let page = 1;
let pageSize = 6;
let maxpages = Math.ceil(recent_vol.length / 6);



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
        if (i < (page * pageSize) && i > (((page - 1) * pageSize) - 1)) {
            recent_vol[i].classList.remove("d-none");
        }
        else {
            recent_vol[i].classList.add("d-none");
        }
    }
}