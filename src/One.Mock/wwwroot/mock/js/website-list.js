// 弹窗触发绑定
$('.modal-trigger').leanModal({ top: 100, overlay: 0.4, closeButton: '.modal_close, .ensure-btn, .cancel-btn' });

// 点击创建站点按钮, 弹窗post表单数据到后台, 重新渲染列表
// 总体,增加卡片按钮
$('.add-website-btn').on('click', function () {
    var $cardItem = $(this).parents('.card-item');
    var Id = $cardItem.data('id');
    addCardFunc(Id);
});

// 取出顶层容器元素
$cardsGroup = $('.cards-group');

// 每个卡片,删除卡片按钮,事件代理
$cardsGroup.on('click', '.delete-card-btn', function () {
    var $cardItem = $(this).parents('.card-item');
    var Id = $cardItem.data('id');
    $.ajax({
        url: '/mock/admin/site/delete',
        type: 'post',
        data: "id=" + Id,
        success: function (res) {
            $cardItem.remove();
        }
    });
});

// 每个卡片,修改卡片按钮,事件代理
$cardsGroup.on('click', '.modify-card-btn', function () {
    var $cardItem = $(this).parents('.card-item');
    var Id = $cardItem.data('id');

    var Name = $cardItem.find('.card-name-bar').html().trim();
    var Url = $cardItem.find('.card-url-bar').html().trim();
    var Path = $cardItem.find('.card-path-bar').html().trim();
    var Cookie = $cardItem.find('.cookie-content').val();
    var Default = $cardItem.data('default');

    modifyCardFunc(Id, Name, Url, Path, Cookie, Default);
});

//// 默认进入页面,发出get请求渲染列表
//$.ajax({
//    url: '/mock/admin/site',
//    type: 'get',
//    dataType: 'json',
//    contentType: "application/json",
//    success: function (res) {
//        // 使用juicer填充列表
//        var data = res.Data;
//        if (!this.addTpl) {
//            $('.col-container .cards-group').children().remove();
//            this.addTpl = juicer($('#insert-card-tpl').html());
//            var html = this.addTpl.render({ data: data });
//        }
//        $(".col-container .cards-group").append(html);
//    }
//});

// 点击创建站点按钮,发出post请求,以返回的response json渲染列表
// 各个type的弹窗UI和表单提交
/*
 * @desc 增加卡片
 * @params Url, 必填
 * @params Cookie, 必填
 * @params Name, 必填
 * @params IsDefault, 必填
 * @params SitePaths, 必填
 */

var addCardFunc = function (Id) {
    var dialogContent = [
        '<div class="modal-form add-card-container">',
        '<div class="form-row-container name-container">',
        '<div class="form-row-left">',
        'Name:',
        '</div>',

        '<div class="form-row-right">',
        '<input type="text" placeholder="输入Name字段" value="">',
        '</div>',

        '</div>',

        '<div class="form-row-container path-container">',
        '<div class="form-row-left">',
        'Url:',
        '</div>',


        '<div class="form-row-right">',
        '<input type="text" placeholder="输入Url字段" value="">',
        '</div>',
        '</div>',

        '<div class="form-row-container cookie-container">',
        '<div class="form-row-left">',
        'Cookie:',
        '</div>',

        '<div class="form-row-right">',
        '<textarea cols="57" rows="5" name="cookie-content" placeholder="填写Cookie" style="resize: none;"></textarea>',
        '</div>',
        '</div>',

        '<div class="form-row-container default-container">',
        '<div class="form-row-left">',
        'Default:',
        '</div>',

        '<div class="form-row-right">',
        '<input type="checkbox" placeholder="输入Url字段" value="">',
        '</div>',
        '</div>',
        '</div>'
    ];

    $('.modal-body-container').html(dialogContent.join(''));

    $('.modal-title').html('添加站点卡片');

    $('.ensure-btn').one('click', function () {
        // 从表单中采集数据
        var $addForm = $(this).parents('#dialog-container').find('.add-card-container');
        // 发送add含义的post请求
        var Name = $addForm.find('.name-container .form-row-right input').val();
        var Path = $addForm.find('.path-container .form-row-right input').val();
        var Url = $addForm.find('.url-container .form-row-right input').val();
        var Cookie = $addForm.find('.cookie-container .form-row-right textarea').val();
        var Default = $addForm.find('.default-container .form-row-right input').is(':checked');
        var formData = {
            "Name": Name,
            "SitePaths": Path,
            "Url": Url,
            "IsDefault": Default,

            "Cookie": Cookie,
            "Id": Id
        };

        formData = JSON.stringify(formData);

        // post到server
        $.ajax({
            url: '/mock/admin/site',
            type: 'post',
            dataType: 'json',
            contentType: "application/json",

            data: formData,
            success: function (res) {
                // 使用juicer填充列表
                var data = res.Data;
                window.location.reload(); 
            }
        });
    });
};

/*
 * @desc 修改卡片
 * @params Id
 * @params Url, 必填
 * @params Cookie, 必填
 * @params Name, 必填
 * @params IsDefault, 必填
 * @params SitePaths, 必填
 */
var modifyCardFunc = function (Id, Name, Url, Path, Cookie, Default) {
    var checkStr = Default ? 'checked' : '';

    var dialogContent = [
        '<div class="modal-form modify-card-container">',

        '<div class="form-row-container name-container">',
        '<div class="form-row-left">',
        'Name:',
        '</div>',

        '<div class="form-row-right">',
        '<input type="text" placeholder="输入Name字段" value="' + Name + '">',
        '</div>',

        '</div>',

        '<div class="form-row-container url-container">',
        '<div class="form-row-left">',
        'Url:',
        '</div>',

        '<div class="form-row-right">',
        '<input type="text" placeholder="输入Url字段" value="' + Url + '">',
        '</div>',
        '</div>',

        '<div class="form-row-container cookie-container">',
        '<div class="form-row-left">',
        'Cookie:',
        '</div>',

        '<div class="form-row-right">',
        '<textarea cols="57" rows="5" name="cookie-content" placeholder="填写Cookie" style="resize: none;">' + Cookie + '</textarea>',
        '</div>',
        '</div>',

        '<div class="form-row-container default-container">',
        '<div class="form-row-left">',
        'Default:',
        '</div>',

        '<div class="form-row-right">',
        '<input type="checkbox" placeholder="输入Url字段" value="" ' + checkStr + '>',
        '</div>',
        '</div>',
        '</div>'
    ];

    $('.modal-body-container').html(dialogContent.join(''));

    $('.modal-title').html('修改站点卡片');

    $('.ensure-btn').one('click', function () {
        // 从表单中采集数据
        var $addForm = $(this).parents('#dialog-container').find('.modify-card-container');

        // 发送add含义的post请求
        var newName = $addForm.find('.name-container .form-row-right input').val();
        var newPath = $addForm.find('.path-container .form-row-right input').val();
        var newUrl = $addForm.find('.url-container .form-row-right input').val();
        var newCookie = $addForm.find('.cookie-container .form-row-right textarea').val();
        var newDefault = $addForm.find('.default-container .form-row-right input').is(':checked');

        var formData = {
            "Name": newName,
            "SitePaths": newPath,
            "Url": newUrl,
            "IsDefault": newDefault,

            "Cookie": newCookie,
            "Id": Id
        };

        formData = JSON.stringify(formData);


        // put到server
        $.ajax({
            url: '/mock/admin/site/update',
            type: 'post',
            dataType: 'json',
            contentType: "application/json",
            data: formData,
            success: function (res) {
                // 使用juicer填充列表
                var data = res.Data;
                window.location.reload(); 
            }
        });
    });
};

