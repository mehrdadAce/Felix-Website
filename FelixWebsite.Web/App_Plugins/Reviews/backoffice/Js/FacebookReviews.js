function checkReviews(length) {
    for (var i = 0; i < length; i++) {
        const element = "#pTag" + i;
        const parentElement = $(element).parent();
        if ($(element).height() > 100) {
            $(element).addClass("setHeight");
            var tag = createTag("more");
            parentElement.on("click", i, extendDiv);
            $(parentElement).append(tag);
        } else {
            $(parentElement).children("a").remove();
        }
    }
}

function changeColor(id) {
    const element = "#" + id;
    if ($(element).hasClass('selected')) {
        $(element).addClass("unselected");
        $(element).removeClass("selected");
    } else {
        $(element).addClass("selected");
        $(element).removeClass("unselected");
    }
}

function extendDiv(index) {
    const actualIndex = index.data;
    const element = "#pTag" + actualIndex;
    $(element).removeClass("setHeight");
    const parentElement = $(element).parent();
    const lessTag = createTag("less");
    parentElement.on("click", actualIndex, cropDiv);
    $(parentElement).children("a").remove();
    $(parentElement).append(lessTag);
}
function cropDiv(index) {
    const actualIndex = index.data;
    const element = "#pTag" + actualIndex;
    $(element).addClass("setHeight");
    const parentElement = $(element).parent();
    const moreTag = createTag("more");
    parentElement.on("click", actualIndex, extendDiv);
    $(parentElement).children("a").remove();
    $(parentElement).append(moreTag);
}

function createTag(identifier) {
    var tag = document.createElement("a");
    if (identifier === "more") {
        tag.innerHTML = "Meer weergeven";
    } else if (identifier === "less") {
        tag.innerHTML = "Minder weergeven";
    } else {
        tag.innerHTML = "Meer weergeven";
    }
    tag.className = "aTag";
    return tag;
}
