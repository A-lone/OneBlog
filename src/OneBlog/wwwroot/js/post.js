OneBlog = {
    comments: {
        postId: null,
        contentBox: $("#Content"),
        captcha: $('#Captcha'),
        replyToId: $("#hiddenReplyTo")
    },
    cancelReply: function () {
        this.replyToComment('');
    },
    addComment: function (e) {
        var content = OneBlog.comments.contentBox.val();
        var captcha = OneBlog.comments.captcha.val();
        if (!captcha) {
            toastr.error("请填写验证码~");
            return false;
        }
        if (!content) {
            toastr.error("请填写内容~");
            return false;
        }
        var l = Ladda.create(document.getElementById('btnSaveAjax'));
        l.start();
        var formData = $("#commentForm input,textarea").map(function () {
            return ($(this).attr("name") + '=' + $(this).val());
        }).get().join("&");
        $.ajax('/comment', {
            method: "POST",
            data: formData,
            success: function (data) {
                l.stop();
                l.remove();
                var result = data.Result;
                var error = data.Error;
                if (error) {
                    toastr.error(error);
                    return;
                }
                console.log('e');
                var commentId = data.CommentId;
                var commentCount = data.CommentCount;
                var commentList = $("#commentlist");
                // add comment html to the right place
                var id = OneBlog.comments.replyToId ? OneBlog.comments.replyToId.val() : '';
                if (id) {
                    var replies = $('#replies_' + id);
                    replies.html(replies.html() + result);
                } else {
                    commentList.html(result + commentList.html());
                    commentList.css('display', 'block');
                }

                var commentForm = $('#comment-form');
                var base = $("#comment");
                var commentlist = $("#commentlist");
                commentForm.insertBefore(commentlist);

                $("body,html").animate({ scrollTop: $('#id_' + commentId).offset().top }, 1000);
                shake($('#id_' + commentId + " .comment-item  .comment-content "), "notice-red", 3);

                if ($('#cancelReply')) {
                    $('#cancelReply').css('display', 'none');
                }

                $("#captchaImg").click();

                // reset form values
                OneBlog.comments.contentBox.val("");
                OneBlog.comments.replyToId.val("")
                OneBlog.comments.captcha.val("");
            },
            error: function (data) {
                l.stop();
                l.remove();
                if (!data) {
                    toastr.error("评论失败,请重试~");
                } else {
                    var error = data.Error;
                    toastr.error(error);
                }
                $("#captchaImg").click();
            }
        });
        return false;
    },
    replyToComment: function (id) {

        init();

        // set hidden value
        OneBlog.comments.replyToId.val(id);

        // move comment form into position
        var commentForm = $('#comment-form');
        if (!id || id == '' || id == null || id == '00000000-0000-0000-0000-000000000000') {
            // move to after comment list
            var base = $("#comment");
            var commentlist = $("#commentlist");
            commentForm.insertBefore(commentlist);

            $('#cancelReply').css("display", "none");
        } else {
            // show cancel
            $('#cancelReply').css("display", "block");

            // move to nested position
            var parentComment = $('#id_' + id);
            var replies = $('#replies_' + id);

            // add if necessary
            if (replies == null) {
                replies = document.createElement('div');
                replies.className = 'comment-replies';
                replies.id = 'replies_' + id;
                parentComment.append(replies);
            }
            replies.css("display", "block");
            replies.append(commentForm);
        }


    }
};

$(document).ready(function () {

    setTimeout(function () {
        $.ajax('/postcount', {
            method: "POST",
            data: "id=" + OneBlog.comments.postId,
            success: function (data) {
            }
        });
    }, 1000);

});


function init() {
    OneBlog.comments.contentBox = $("#Content");
    OneBlog.comments.captcha = $('#Captcha');
    OneBlog.comments.replyToId = $("#hiddenReplyTo");
    OneBlog.comments.contentBox.val("");
    OneBlog.comments.replyToId.val("")
    OneBlog.comments.captcha.val("");
}


function shake(ele, cls, times) {
    var eleclass = ele.attr("class");
    if (eleclass == 'undefined') {
        eleclass = '';
    }

    var i = 0, t = false, o = eleclass + " ", c = "", times = times || 2;
    if (t) return;
    t = setInterval(function () {
        i++;
        c = i % 2 ? o + cls : o;
        ele.attr("class", c);
        if (i == 2 * times) {
            clearInterval(t);
            ele.removeClass(cls);
        }
    }, 200);
};
