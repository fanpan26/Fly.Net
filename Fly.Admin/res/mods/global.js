layui.define(['layer'], function (exports) {
    var $ = layui.jquery
    , layer = layui.layer;

    var gather = {
        initEvent: function () {
            $('body').on('click', '*[admin-event]', function (e) {
                var othis = $(this), method = othis.attr('admin-event');
                events[method] ? events[method].call(this, othis, e) : '';
            });
        }, json: function (url, data, success, options) {
            options = options || {};

            $.ajax({
                url: url,
                data: data,
                type: options.type || 'get',
                success: function (res) {
                    success(res);
                },
                error: function () {

                }
            });
        }
    };


    //事件
    var events = {
        addCream: function (othis) {
            var artid=othis.data('id');
            var top = othis.data('iscream')=='True'?false:true;
            var userid = othis.data('userid');
            gather.json('/cream', { artid: artid, value: top, toUserId: userid }, function (res) {
                console.log(res);
            }, { type: 'post' });
        },
        addTop: function (othis) {
            var artid = othis.data('id');
            var top = othis.data('istop') == 'True' ? false : true;
            var userid = othis.data('userid');
            gather.json('/top', { artid: artid, value: top, toUserId: userid }, function (res) {
                console.log(res);
            }, { type: 'post' });
        }
    };

    gather.initEvent();

    exports('global', null);

});