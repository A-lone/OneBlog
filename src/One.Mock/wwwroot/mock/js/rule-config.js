// 点击总表单的提交按钮
$('.add-config-btn').on('click', function() {
    // 从url中提取site id参数
    var url = location.search;
    var theRequest = {};

    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for(var i = 0; i < strs.length; i ++) {
            theRequest[strs[i].split("=")[0]]=(strs[i].split("=")[1]);
        }
    }

    var siteId = theRequest.Id;

   // 收集表单参数
   var Method = $('.config-form-container .method-bar input').val();
   var Path = $('.config-form-container .path-bar input').val();
   var Query = $('.config-form-container .query-bar input').val();
   var Expression = $('.config-form-container .op-bar textarea').val();
   var Cookie = $('.config-form-container .cookie-bar textarea').val();
   var Json = $('.config-form-container .json-bar textarea').val();

   var ItemId = $('.main-form').data('item-id');

    var formData = {
        "Id": siteId,
        "Method": Method,
        "Path": Path,
        "Query": Query,
        "Expression": Expression,
        "Cookie": Cookie,
        "Json": Json,
        "ItemId": ItemId
    };

    formData = JSON.stringify(formData);

    $.ajax({
        url: '/mock/admin/sitepath',
        type: 'post',
        dataType: 'json',
        contentType: "application/json",

        data: formData,
        success: function (res) {
            // 更新列表项
            var data = res.Data;
            $.each(data, function(index, element) {
                var dialogContent = [
                    '<tr data-item-id="'+ element.ItemId +'">',
                        '<td class="method-col-item">'+ element.Method +'</td>',
                        '<td class="path-col-item">'+ element.Path +'</td>',
                        '<td class="query-col-item">'+ element.Query +'</td>',
                        '<td class="cookie-col-item">'+ element.Cookie +'</td>',
                        '<td class="json-col-item">'+ element.Json +'</td>',
                        '<td class="func-col-item">'+ element.Expression +'</td>',
                    '</tr>'
                ];

                $('.config-items tbody').prepend(dialogContent.join(''));
            });

            // 清空表单
            $('.config-form-container .method-bar input').val('');
            $('.config-form-container .path-bar input').val('');
            $('.config-form-container .query-bar input').val('');
            $('.config-form-container .op-bar textarea').val('');
            $('.config-form-container .cookie-bar textarea').val('');
            $('.config-form-container .json-bar textarea').val('');

            $('.main-form').data('');
        }
    })

});

var $itemContainer = $('.config-items');

// 点击列表中每一项的删除按钮
$itemContainer.on('click', '.remove-btn', function() {
    var $row = $(this).parents('tr');
    // 收集参数
    // 从url中提取site id参数
    var url = location.search;
    var theRequest = {};

    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for(var i = 0; i < strs.length; i ++) {
            theRequest[strs[i].split("=")[0]]=(strs[i].split("=")[1]);
        }
    }

    var siteId = theRequest.Id;

    var ItemId = $row.data('item-id');

    var formData = {
        "Id": siteId,
        "ItemId": ItemId
    };

    formData = JSON.stringify(formData);

    $.ajax({
        url: '/mock/admin/sitepath',
        type: 'delete',
        dataType: 'json',
        contentType: "application/json",

        data: formData,
        success: function (res) {
            $row.remove();
        }
    })

});

// 点击列表中每一项的修改按钮
$itemContainer.on('click', '.modify-btn', function() {
    // 列表item中取数据
    var $row = $(this).parents('tr');

    var ItemId = $row.data('item-id');

    var Method = $row.find('.method-col-item').html();
    var Path = $row.find('.path-col-item').html();
    var Query = $row.find('.query-col-item').html();
    var Expression = $row.find('.func-col-item').html();
    var Cookie = $row.find('.cookie-col-item').html();
    var Json = $row.find('.json-col-item').html();

    // 回塞表单
    $('.config-form-container .method-bar input').val(Method.trim());
    $('.config-form-container .path-bar input').val(Path.trim());
    $('.config-form-container .query-bar input').val(Query.trim());
    $('.config-form-container .op-bar textarea').val(Expression.trim());
    $('.config-form-container .cookie-bar textarea').val(Cookie.trim());
    $('.config-form-container .json-bar textarea').val(Json.trim());
});