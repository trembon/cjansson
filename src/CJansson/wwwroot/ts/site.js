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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2l0ZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uL1NjcmlwdHMvc2l0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxJQUFJLEdBQUcsR0FBRyxNQUFNLENBQUMscUJBQXFCLElBQVUsTUFBTyxDQUFDLHdCQUF3QixJQUFVLE1BQU8sQ0FBQywyQkFBMkIsSUFBVSxNQUFPLENBQUMsdUJBQXVCLENBQUM7QUFDdkssSUFBSSxHQUFHLEVBQUU7SUFDTCxHQUFHLENBQUMsY0FBYyxNQUFNLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO0NBQ3hEO0tBQUs7SUFDRixNQUFNLENBQUMsZ0JBQWdCLENBQUMsTUFBTSxFQUFFLFFBQVEsQ0FBQyxDQUFDO0NBQzdDO0FBRUQsb0JBQW9CO0FBQ3BCLElBQUksU0FBUyxHQUFHO0lBQ1osVUFBVSxFQUFFLDRDQUE0QztJQUN4RCxTQUFTLEVBQUUseURBQXlEO0lBQ3BFLE9BQU8sRUFBRSx5Q0FBeUM7SUFDbEQsVUFBVSxFQUFFLHlFQUF5RTtDQUN4RixDQUFBO0FBRUQsU0FBUyxRQUFRO0lBQ2IseUNBQXlDO0lBQ3pDLElBQUksUUFBUSxHQUFHLE1BQU0sQ0FBQyxRQUFRLENBQUMsUUFBUSxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztJQUNuRCxJQUFJLFFBQVEsQ0FBQyxNQUFNLElBQUksQ0FBQyxFQUFFO1FBQ3RCLElBQUksT0FBTyxHQUFHLEdBQUcsR0FBRyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7UUFDaEMsSUFBSSxTQUFTLEdBQUcsUUFBUSxDQUFDLGdCQUFnQixDQUFDLFNBQVMsQ0FBQyxDQUFDO1FBQ3JELEtBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxTQUFTLENBQUMsTUFBTSxFQUFFLENBQUMsRUFBRSxFQUFFO1lBQ3ZDLElBQUksY0FBYyxHQUFzQixTQUFTLENBQUMsSUFBSSxD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