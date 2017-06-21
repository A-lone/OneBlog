$(document).ready(function () {
    $('.js--toggle-search-mode').on('click', function (ev) {
        ev.preventDefault();
        $('body').toggleClass('search-mode'); if ($('body').hasClass('search-mode')) {
            setTimeout(function () { $('.js--search-panel-text').focus(); }, 50);
            $(document).on('keyup.searchMode',
                function (ev) {
                    ev.preventDefault();
                    if (ev.keyCode === 27) { $('body').toggleClass('search-mode'); $(document).off('keyup.searchMode'); }
                });
        } else { $(document).off('keyup.searchMode'); }
    });
});