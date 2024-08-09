var raf = window.requestAnimationFrame || (<any>window).mozRequestAnimationFrame || (<any>window).webkitRequestAnimationFrame || (<any>window).msRequestAnimationFrame;
if (raf) {
    raf(function () { window.setTimeout(siteLoad, 0); });
}else {
    window.addEventListener('load', siteLoad);
}

// set up share data
let shareData = {
    "facebook": "http://www.facebook.com/sharer.php?u={url}",
    "twitter": "https://twitter.com/intent/tweet?url={url}&text={title}",
    "gplus": "https://plus.google.com/share?url={url}",
    "linkedin": "https://www.linkedin.com/shareArticle?mini=true&url={url}&title={title}"
}

function siteLoad() : void {
    // select the current section in the menu
    let segments = window.location.pathname.split('/');
    if (segments.length >= 2) {
        let section = '/' + segments[1];
        let menuItems = document.querySelectorAll(".menu a");
        for (let i = 0; i < menuItems.length; i++) {
            let currentElement = <HTMLAnchorElement>menuItems.item(i);
            if (currentElement.getAttribute('href').substring(0, section.length) === section) {
                currentElement.classList.add('selected');
            }
        }
    }

    let shareIcons = document.querySelectorAll(".share-btns a");
    for (let i = 0; i < shareIcons.length; i++) {
        shareIcons.item(i).addEventListener("click", shareButtonClicked, false);
    }

    // handle value in contact form
    let form = document.getElementById("contactForm");
    if (form) {
        form.addEventListener("submit", contactFormSubmit, false);
    }
}

function shareButtonClicked(ev: MouseEvent) {
    ev.preventDefault();

    let windowHeight = 300, windowWidth = 500;

    let current = <HTMLAnchorElement>ev.currentTarget;
    let shareSite = current.getAttribute("data-site");
    let shareUrl: string = shareData[shareSite];

    shareUrl = shareUrl.replace(/\{url\}/, () => {
        let parentDataUrl = current.parentElement.getAttribute("data-url");
        if (parentDataUrl !== undefined && parentDataUrl !== null && parentDataUrl !== "") {
            return encodeURIComponent(parentDataUrl);
        } else {
            return encodeURIComponent(window.location.toString());
        }
    });
    shareUrl = shareUrl.replace(/\{title\}/, () => {
        let parentDataTitle = current.parentElement.getAttribute("data-title");
        if (parentDataTitle !== undefined && parentDataTitle !== null && parentDataTitle !== "") {
            return encodeURIComponent(parentDataTitle);
        } else {
            return encodeURIComponent(document.title);
        }
    });

    let windowPosY = (window.screen.height / 2) - (windowHeight / 2);
    let windowPosX = (window.screen.width / 2) - (windowWidth / 2);

    window.open(shareUrl, "", "width=" + windowWidth + ",height=" + windowHeight + ",location=0,menubar=0,left=" + windowPosX + ",top=" + windowPosY);

    return false;
}

function contactFormSubmit() {
    (<HTMLInputElement>document.getElementById("Source")).value = "FormSubmit";
}