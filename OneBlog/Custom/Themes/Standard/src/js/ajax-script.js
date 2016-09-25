jQuery(document).ready(function ($) {

    // Establish Variables
    var
		History = window.History, // Note: Using a capital H instead of a lower h
		State = History.getState(),
		$log = $('#log');

    var main_id = ".main_col"; //This is where to get the content.
    var comment_id = "#comment-box";
    var $container = $("#container_post > .row"), //Where to load the content
    siteUrl = self.location.protocol.toString() + "//" + self.location.host.toString(),
    url = '';

    //$("#main").load(State.url+'/?page_id=267 #main');


    //Bind all internal links   
    $("body").on("click", ".ob_post_list .post_title a, .ob_post_list article > a", function (e) {
        e.preventDefault();
        var path = $(this).attr('href');
        var title = $(this).text();

        History.pushState({ state: 'ajax' }, title, path);

    });//on click


    //Bind Pagination links 
    $(document).on("click", ".ob_post_list .pagination a", function (e) {
        e.preventDefault();
        var path = $(this).attr('href');
        var title = document.title;

        History.pushState({ state: 'pagination' }, title, path);
    });//on click


    // Bind to state change
    // When the statechange happens, load the appropriate url via ajax
    History.Adapter.bind(window, 'statechange', function () { // Note: Using statechange instead of popstate
        State = History.getState();

        if (State.data.state == "pagination") {
            loadPagination();
        } else {
            beforeLoad(mainLoad);
        }

        //mainLoad();
    });

    // Load Ajax
    function mainLoad() {

        State = History.getState(); // Note: Using History.getState() instead of event.state
        // History.log('statechange:', State.data, State.title, State.url);
        //console.log(event);
        $(main_id).prepend('<div class="preloader fadeIn main_preloader" style=""><i class="fa fa-cog fa-spin hero_color" style=""></i></div>');

        $container.load(State.url + " " + main_id, function (data) {
            /* After the content loads you can make additional callbacks*/
            //$container.prepend("<div class='main_line'></div>");

            $(".main_preloader").css('opacity', 0).delay(900).fadeOut();

            var request = $(data);

            //Get <script> (if there are) from the #main and execute them
            var $new_main = $(main_id, request);
            var scripts = $new_main.find('script');
            if (scripts != 0) {
                for (var ix = 0; ix < scripts.length; ix++) {
                    eval(scripts[ix].text);
                }
            }

            if ($("body").scrollTop() > 200) {
                scrollToElement(main_id);
            };

            //Show content with animation
            afterLoad(request);


        });
    }

    function beforeLoad(paramFunc) {
        $("#container_post article,#container_post #comment-box").animo({ animation: 'fadeOutDown', duration: 0.3, keep: true, timing: 'ease-in-out' }, function (e) {
            paramFunc();
        });
    }

    function afterLoad(request) {

        ob_animateMainPost();

        //Highlight current post
        var $article = $(".main_col article", request);
        var artcile_id = $($article[0]).attr('id');
        $(".ob_post_list article").removeClass('current_post_list');
        $(".ob_post_list").find("#list_" + artcile_id).addClass('current_post_list');

        //Remove Posts list if it's a page.

    }



    //Load content in pagination
    function loadPagination() {
        State = History.getState();

        $.get(State.url, function (data) {

            $(".ob_post_list").replaceWith($(data).find(".ob_post_list"));
            ob_niceSCrollPostList();
            ob_animateMainPostList();
        });//get

    }

});//Dom ready