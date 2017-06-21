(function ($) {

    var img = new Image();
    var originalWidth = 0;
    var originalHeight = 0;
    var $image = $('#js-avatar-image');

    function showModal($btn_file) {
        var file = $btn_file.prop('files')[0];
        var imageType = /image.*/;
        // if file is an image file
        if (file.type.match(imageType)) {
            var reader = new FileReader()
            reader.onload = function (event) {

                img.onload = function () {
                    // set image size so it will fit in canvas
                    originalWidth = img.width;
                    originalHeight = img.height;
                }

                img.src = event.target.result;

                $image.attr("src", event.target.result);

                $("#modal-image-cropper").modal({ backdrop: 'static', keyboard: false });

            };
            reader.readAsDataURL(file);

        }

        $btn_file.val('');
    }

    function showResponse(data) {
        if (data) {
            if (data.ErrNo == 0) {
                toastr.success(data.ErrMsg);
                setInterval(function () {
                    window.location.reload();
                }, 1234);
            } else {
                toastr.error(data.ErrMsg);
            }
            return;
        }
        toastr.error("出现错误,请重试~");
    }

    function showRequest(formData, jqForm, options) {
        var queryString = $.param(formData);
        return true;
    }


    $(document).ready(function () {

        var options = {
            beforeSubmit: showRequest,
            success: showResponse
        };
        $('.js-account-form').submit(function () {
            $(this).ajaxSubmit(options);
            return false;
        });

        $('.js-password-form').submit(function () {
            $(this).ajaxSubmit(options);
            return false;
        });


        $("#btnPostFile").click(function () {
            var l = Ladda.create(this);
            l.start();

            $image.cropper('getCroppedCanvas');
            $image.cropper('getCroppedCanvas', {
                width: 160,
                height: 90
            });


            $image.cropper('getCroppedCanvas').toBlob(function (blob) {
                var formData = new FormData();
                formData.append('avatar', blob);

                $.ajax('/account/upload', {
                    method: "POST",
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        l.stop();
                        l.remove();
                        $("#modal-image-cropper").modal("hide");
                        if (!data) {
                            return;
                        }
                        var mobile_avatar = $("#mobile-avatar");
                        var pc_avatar = $("#pc-avatar");
                        mobile_avatar.attr("src", data);
                        pc_avatar.attr("src", data);
                        toastr.success("头像更新成功~");
                    },
                    error: function () {
                        l.stop();
                        l.remove();
                        toastr.error("头像上传失败,请重试~");
                    }
                });
            });
        });


        $('#modal-image-cropper').on('shown.bs.modal', function () {
            $image.cropper({
                autoCropArea: 0.5,
                ready: function () {
                    $image.cropper('setCanvasData', canvasData);
                    $image.cropper('setCropBoxData', cropBoxData);
                }
            });

            $image.cropper({
                strict: false,
                background: false,
                zoomable: false,
                autoCropArea: 0.8,
                aspectRatio: 16 / 9,
                crop: function (e) {
                }
            });
        }).on('hidden.bs.modal', function () {
            cropBoxData = $image.cropper('getCropBoxData');
            canvasData = $image.cropper('getCanvasData');
            $image.cropper('destroy');
        });


        $('#avatar-fileupload').on('change', function (e) {
            e.preventDefault();
            var $btn_file = $('#avatar-fileupload');
            showModal($btn_file);
        });

        $('#avatar-mobile-fileupload').on('change', function (e) {
            e.preventDefault();
            var $btn_file = $('#avatar-mobile-fileupload');
            showModal($btn_file);
        });

    });
})(jQuery);