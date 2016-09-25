if (document.documentElement) {
    var cn = document.documentElement.className;
    document.documentElement.className = cn.replace(/no-js/, '');
}
var ob_niceSCrollPostList, ob_animateHeader, ob_animateMainPostList, ob_animateMainPost;
jQuery(document).ready(function ($) {

    //Get post lists via AJAX
    if ($(".ob_post_list article").length == 0) {
        siteUrl = self.location.protocol.toString() + "//" + self.location.host.toString();
        $.get(siteUrl, function (data) {
            $(".ob_post_list").replaceWith($(data).find(".ob_post_list"));
            highlight_post();
        });//get
    };

    //Remove HTML from input search
    function strip(html) {
        var StrippedString = html.replace(/(<([^>]+)>)/ig, "");
        return StrippedString;
    }
    $('#search-field').keydown(function (e) {
        var input_val = $(this).val();
        $(this).val(strip(input_val));

    });

    //Search Plugin
    $("#search-field").ghostHunter({
        results: "#results",
        before: function () {
        },
        onComplete: function (results) {
            $(".ob_post_list").remove();
            $("body").addClass('tag-template').removeClass('home-template');
            var search_value = $("#search-field").val();

            var search_result = '';

            switch (results.length) {
                case 0:
                    search_result = 'No posts found';
                case 1:
                    search_result = results.length + ' post';
                default:
                    search_result = results.length + ' posts';
            };
            var search_header = '<article class="post animated fadeInUp" id="post-" style="animation-duration: 0.6s; animation-timing-function: ease-in-out;">    	<div class="tag-cover no-cover"></div><div class="tag-content"><h1 class="tag-title"><span>Search:</span> ' + search_value + '</h1><ul class="tag-meta"><li class="tag-stats"><i class="fa fa-pencil"></i>' + search_result + '</li></ul></div><!-- /tag-content -->';
            $(".main_col").html(search_header).removeClass().addClass('col-md-8 col-lg-10 col-md-offset-2 col-sm-12 main_col');

            var search_posts = '';
            $(results).each(function (index, el) {
                var search_post = '<li><h5><a href="' + el.link + '">' + el.title + '</a></h5><time>' + el.pubDate.substring(0, el.pubDate.length - 12) + '</time><div class="clearfix"></div></li>';
                search_posts = search_posts + search_post;
            });


            var search_list = '<ol class="archives_post_list">' + search_posts + '<ol>';
            $(search_list).appendTo('.main_col');
            //console.log( results );
            //alert("results have been rendered");
        }
    });

    if ('ontouchstart' in document) {
        $('body').removeClass('no-touch');
    }

    //Higlight current post
    function highlight_post() {
        var current_id = $(".main_col .post").attr('id');
        $(".ob_post_list .post").removeClass('current_post_list');
        $("#list_" + current_id).addClass('current_post_list');
    }
    highlight_post();


    //Make Columns equal height
    function equal_height() {
        var row_height = $(".main_col").parent(".row").height();
        var window_width = $(window).width();
        var window_height = $(window).height();

        if (window_width > 992 && row_height < window_height) {
            $(".main_col, #sidebar").css('height', window_height);
        } else {
            $(".main_col, #sidebar").css('height', 'auto');
        };

    }
    $(window).on("debouncedresize", function (event) {
        equal_height();
    });
    equal_height();




    //Scroll for Blog items
    ob_niceSCrollPostList = function () {
        jQuery(function ($) {
            $(".ob_post_list").niceScroll({
                touchbehavior: true,
                //cursorwidth: "3",
                cursoropacitymax: 0,
                bouncescroll: true,
                cursorcolor: "#000",
                railpadding: { top: 0, right: 2, left: 0, bottom: 0 },
                grabcursorenabled: false,
                autohidemode: true
            });
        });//jQuery(function($)
    }//ob_niceSCrollPostList



    //Scroll for Blog items
    $("#header_sidebar").niceScroll({
        touchbehavior: true,
        //cursorwidth: "3",
        cursoropacitymax: 0.2,
        bouncescroll: true,
        cursorcolor: "#000",
        railpadding: { top: 0, right: 2, left: 0, bottom: 0 },
        bouncescroll: true,
        grabcursorenabled: false
    });

    /*Dropdown menu on hover */

    $("#jqueryslidemenu").on({
        mouseenter: function () {
            $(this).find('.dropdown-menu').first().stop(true, true).delay(100).slideDown(400, function () {
                $(this).addClass('open');
            });
        },
        mouseleave: function () {
            $(this).find('.dropdown-menu').first().stop(true, true).delay(100).slideUp(400, function () {
                $(this).removeClass('open');
            });
        }
    }, ".dropdown");  // descendant selector

    //Disqus

    //$(document).on('click', '.main_col .meta_comments a', function (event) {
    //    event.preventDefault();
    //    /* Act on the event */
    //    var post_id = $(".main_col > .post").attr('id');

    //    disqus_identifier = post_id.replace('post-', '');
    //    $.ajax({
    //        type: "GET",
    //        url: "http://" + disqus_shortname + ".disqus.com/embed.js",
    //        dataType: "script",
    //        cache: true
    //    });
    //});

    $(".collapse").collapse();
    $('.dropdown-toggle').dropdown();
    $('*[data-toggle="tooltip"]').tooltip();

});





jQuery(window).load(function ($) {
    (function ($) {
        $(function () {

            ob_animateHeader = function () {
                jQuery(function ($) {
                    $("#header_sidebar").animo({ animation: 'fadeIn', duration: 0.7, keep: true, timing: 'ease-in-out' }, function (e) {
                        ob_animateMainPostList();

                        var mainpost_delay = 700;
                        if ($("body").hasClass('author-template') || $("body").hasClass('page-template')) { mainpost_delay = 0 };
                        setTimeout(function () {
                            ob_animateMainPost();
                        }, mainpost_delay);
                    });
                });//jQuery(function($)
            }//ob_animateHeader
            ob_animateHeader();





            //Main Post
            ob_animateMainPost = function () {
                jQuery(function ($) {
                    $("#container_post .post, #container_post .page, #container_post article , #container_post #comment-box").animo({ animation: 'fadeInUp', duration: 0.6, keep: true, timing: 'ease-in-out' }, function (e) {
                        //$("<style type='text/css'> .main_col{ border-right-color:#eeeeee!important;} </style>").appendTo("head");
                    });
                });//jQuery(function($)
            }//ob_animateMainPost

            //Posts list
            ob_animateMainPostList = function () {
                jQuery(function ($) {
                    $(".ob_post_list .post").each(function (index, el) {

                        $(".ob_post_list").css("border-right-color", "#eeeeee");

                        setTimeout(function () {
                            $(el).animo({ animation: 'fadeInLeft', duration: 0.4, keep: true, timing: 'ease-in-out' });
                            if ($(".ob_post_list .post").length == index + 1) {
                                ob_niceSCrollPostList();
                            };
                        }, index * 200);

                    });
                });//jQuery(function($)
            }//ob_animateMainPostList




        });// jQuery NoConflict
    })(jQuery);
});// window load
function scrollToElement(selector, time, verticalOffset) {
    time = typeof (time) != 'undefined' ? time : 1000;
    verticalOffset = typeof (verticalOffset) != 'undefined' ? verticalOffset : 0;
    element = jQuery(selector);
    offset = element.offset();
    offsetTop = offset.top + verticalOffset;
    jQuery('html, body').animate({
        scrollTop: offsetTop
    }, time);
}