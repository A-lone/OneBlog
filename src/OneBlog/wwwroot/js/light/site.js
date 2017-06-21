$(document).ready(function () {
    $(".off-canvas-toggle").click(function (e) {
        e.preventDefault(), $(".off-canvas-container").toggleClass("is-active")
    });
    var e = $(".search-form__field"), t = $(".search-results"), n = $(".toggle-search-button"), i = "        <div class='search-results__item'>          <a class='search-results__item__title' href='{{link}}'>{{title}}</a>          <span class='post__date'>{{pubDate}}</span>        </div>";
    n.click(function (t) {
        t.preventDefault(), $(".search-form-container").addClass("is-active"),
            $(".off-canvas-container").removeClass("is-active"),
            setTimeout(function () { e.focus() }, 500)
    }),
        $(".search-form-container, .close-search-button").on("click keyup", function (e) {
            if (e.target != this && "fa fa-close" == e.target.className && 27 != e.keyCode) {
                $(".search-form-container").removeClass("is-active");
            }
        }),
        e.ghostHunter({
            results: t, onKeyUp: !0, info_template: "<h4 class='heading'>Number of posts found: {{amount}}</h4>",
            result_template: i, before: function () {
                t.fadeIn()
            }
        })
}
);