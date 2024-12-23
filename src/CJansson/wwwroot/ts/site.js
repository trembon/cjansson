var raf = window.requestAnimationFrame || window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;
if (raf) {
    raf(function () { window.setTimeout(siteLoad, 0); });
}
else {
    window.addEventListener('load', siteLoad);
}
// set up share data
var shareData = {
    "facebook": "http://www.facebook.com/sharer.php?u={url}",
    "twitter": "https://twitter.com/intent/tweet?url={url}&text={title}",
    "gplus": "https://plus.google.com/share?url={url}",
    "linkedin": "https://www.linkedin.com/shareArticle?mini=true&url={url}&title={title}"
};
function siteLoad() {
    // select the current section in the menu
    var segments = window.location.pathname.split('/');
    if (segments.length >= 2) {
        var section = '/' + segments[1];
        var menuItems = document.querySelectorAll(".menu a");
        for (var i = 0; i < menuItems.length; i++) {
            var currentElement = menuItems.item(i);
            if (currentElement.getAttribute('href').substring(0, section.length) === section) {
                currentElement.classList.add('selected');
            }
        }
    }
    var shareIcons = document.querySelectorAll(".share-btns a");
    for (var i = 0; i < shareIcons.length; i++) {
        shareIcons.item(i).addEventListener("click", shareButtonClicked, false);
    }
    // handle value in contact form
    var form = document.getElementById("contactForm");
    if (form) {
        form.addEventListener("submit", contactFormSubmit, false);
    }
}
function shareButtonClicked(ev) {
    ev.preventDefault();
    var windowHeight = 300, windowWidth = 500;
    var current = ev.currentTarget;
    var shareSite = current.getAttribute("data-site");
    var shareUrl = shareData[shareSite];
    shareUrl = shareUrl.replace(/\{url\}/, function () {
        var parentDataUrl = current.parentElement.getAttribute("data-url");
        if (parentDataUrl !== undefined && parentDataUrl !== null && parentDataUrl !== "") {
            return encodeURIComponent(parentDataUrl);
        }
        else {
            return encodeURIComponent(window.location.toString());
        }
    });
    shareUrl = shareUrl.replace(/\{title\}/, function () {
        var parentDataTitle = current.parentElement.getAttribute("data-title");
        if (parentDataTitle !== undefined && parentDataTitle !== null && parentDataTitle !== "") {
            return encodeURIComponent(parentDataTitle);
        }
        else {
            return encodeURIComponent(document.title);
        }
    });
    var windowPosY = (window.screen.height / 2) - (windowHeight / 2);
    var windowPosX = (window.screen.width / 2) - (windowWidth / 2);
    window.open(shareUrl, "", "width=" + windowWidth + ",height=" + windowHeight + ",location=0,menubar=0,left=" + windowPosX + ",top=" + windowPosY);
    return false;
}
function contactFormSubmit() {
    document.getElementById("Source").value = "FormSubmit";
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2l0ZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uL1NjcmlwdHMvc2l0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxJQUFJLEdBQUcsR0FBRyxNQUFNLENBQUMscUJBQXFCLElBQVUsTUFBTyxDQUFDLHdCQUF3QixJQUFVLE1BQU8sQ0FBQywyQkFBMkIsSUFBVSxNQUFPLENBQUMsdUJBQXVCLENBQUM7QUFDdkssSUFBSSxHQUFHLEVBQUUsQ0FBQztJQUNOLEdBQUcsQ0FBQyxjQUFjLE1BQU0sQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDekQsQ0FBQztLQUFLLENBQUM7SUFDSCxNQUFNLENBQUMsZ0JBQWdCLENBQUMsTUFBTSxFQUFFLFFBQVEsQ0FBQyxDQUFDO0FBQzlDLENBQUM7QUFFRCxvQkFBb0I7QUFDcEIsSUFBSSxTQUFTLEdBQUc7SUFDWixVQUFVLEVBQUUsNENBQTRDO0lBQ3hELFNBQVMsRUFBRSx5REFBeUQ7SUFDcEUsT0FBTyxFQUFFLHlDQUF5QztJQUNsRCxVQUFVLEVBQUUseUVBQXlFO0NBQ3hGLENBQUE7QUFFRCxTQUFTLFFBQVE7SUFDYix5Q0FBeUM7SUFDekMsSUFBSSxRQUFRLEdBQUcsTUFBTSxDQUFDLFFBQVEsQ0FBQyxRQUFRLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDO0lBQ25ELElBQUksUUFBUSxDQUFDLE1BQU0sSUFBSSxDQUFDLEVBQUUsQ0FBQztRQUN2QixJQUFJLE9BQU8sR0FBRyxHQUFHLEdBQUcsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDO1FBQ2hDLElBQUksU0FBUyxHQUFHLFFBQVEsQ0FBQyxnQkFBZ0IsQ0FBQyxTQUFTLENBQUMsQ0FBQztRQUNyRCxLQUFLLElBQUksQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsU0FBUyxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDO1lBQ3hDLElB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