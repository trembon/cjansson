(function () {
    if (typeof self === 'undefined' || !self.Prism || !self.document) {
        return;
    }

    Prism.hooks.add('before-highlight', function (env) {
        var pre = env.element.parentNode;
        if (!pre || !/pre/i.test(pre.nodeName)) {
            return;
        }

        /* check if the divs already exist */
        var sib = pre.nextSibling;
        var div, div2;
        if (sib && /\s*\bprism-show-filename\b\s*/.test(sib.className) &&
            sib.firstChild &&
            /\s*\bprism-show-filename-label\b\s*/.test(sib.firstChild.className)) {
            div2 = sib.firstChild;
        } else {
            div = document.createElement('div');
            div2 = document.createElement('a');

            div2.className = 'prism-show-filename-label';

            div.className = 'prism-show-filename';
            div.appendChild(div2);

            pre.parentNode.insertBefore(div, pre.nextSibling);
        }

        div2.innerHTML = pre.getAttribute('data-filename');
        div2.href = pre.getAttribute('data-url');
        div2.target = "_blank";
    });
})();