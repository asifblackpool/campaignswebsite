const formservices = (function () {
    // Private function to wrap left-side content and then wrap both in outer container
    function wrapFormContainer() {
        const container = document.querySelector('.content-container');
        if (!container) return; // safety check

        // Create wrapper div for left-side text
        const leftWrapper = document.createElement('div');
        leftWrapper.classList.add('left-col-text');

        // Select all h2, p, a elements before the form
        const elementsToWrap = [];
        container.childNodes.forEach(node => {
            if (node.tagName === 'H2' || node.tagName === 'P' || node.tagName === 'A') {
                elementsToWrap.push(node);
            }
        });

        // Move selected elements into leftWrapper
        elementsToWrap.forEach(el => leftWrapper.appendChild(el));

        // Insert leftWrapper before the form
        const formDiv = container.querySelector('[data-contensis-form-id="weddingEnquiry"]');
        if (!formDiv) return;

        container.insertBefore(leftWrapper, formDiv);

        // Now create the outer container div
        const outerWrapper = document.createElement('div');
        outerWrapper.classList.add('form-outer-container');

        // Move leftWrapper and formDiv into outerWrapper
        outerWrapper.appendChild(leftWrapper);
        outerWrapper.appendChild(formDiv);

        // Insert outerWrapper into the main container
        container.appendChild(outerWrapper);
    }

    // Public API
    return {
        init: wrapFormContainer
    };
})();

