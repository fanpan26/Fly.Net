/**

 @Name: 用户模块

 */
 
layui.define(['laypage', 'fly'], function (exports) {

    var $ = layui.jquery;
    var layer = layui.layer;
    var util = layui.util;
    var laytpl = layui.laytpl;
    var form = layui.form;
    var laypage = layui.laypage;
    var fly = layui.fly;

    var gather = {}, dom = {
        mine: $('#LAY-mine')
      , mineview: $('.mine-view')
      , minemsg: $('#LAY_minemsg')
      , infobtn: $('#LAY_btninfo')
      , userhome: $('#LAY-userhome')
    };

    //我的相关数据
    gather.minelog = {};
    gather.mine = function (index, type) {
        if (!type) return;
        var tpl = [
          //求解
          '{{# for(var i = 0; i < d.rows.length; i++){ }}\
      <li>\
        {{# if(d.rows[i].iscream == true){ }}\
        <span class="fly-jing">精</span>\
        {{# } }}\
        {{# if(d.rows[i].istop == true){ }}\
        <span class="fly-stick">置顶</span>\
        {{# } }}\
        {{# if(d.rows[i].isover== true){ }}\
        <span class="jie-status jie-status-ok">已解决</span>\
        {{# } }}\
        <a class="jie-title" href="/detail/{{d.rows[i].id}}" target="_blank">{{= d.rows[i].title}}</a>\
        <i>{{d.rows[i].addtimestr}}</i>\
        {{# if(d.rows[i].isself == true){ }}\
        <a class="mine-edit" href="/jie/edit/{{d.rows[i].id}}">采纳</a>\
        {{# } }}\
        <em>{{d.rows[i].visitor_count}}阅/{{d.rows[i].comment_count}}答</em>\
      </li>\
      {{# } }}'
        ];
        function view(res) {
            var html = laytpl(tpl[index]).render(res);
            dom.mine.find('a').eq(index).find('cite').html(res.count);
            dom.mineview.eq(index).html(res.rows.length === 0 ? '<div class="fly-msg">没有相关数据</div>' : html);
        }
        function page(now) {
            var curr = now || 1;
            if (gather.minelog[type + 'page-' + curr]) {
                view(gather.minelog[type + 'page-' + curr]);
            } else {
                fly.json('/api/' + type + '/', {
                    pageIndex: curr
                }, function (res) {
                    view(res.data);
                    gather.minelog[type + 'page-' + curr] = res.data;
                    now || laypage({
                        cont: 'LAY-page'
                      , pages: res.data.pages
                      , skin: 'fly'
                      , curr: curr
                      , jump: function (e, first) {
                          if (!first) {
                              page(e.curr);
                          }
                      }
                    });
                });
            }
        }
        if (!gather.minelog[type]) {
            page();
        }
    };

    //显示当前tab
    gather.tabshow = function (index, hash) {
        var a = dom.mine.find('a');
        if (hash) {
            a.each(function (i, item) {
                if ($(this).attr('hash') === hash) {
                    index = i;
                    return false;
                }
            });
        }
        a.eq(index).addClass('tab-this').siblings().removeClass('tab-this');
        dom.mineview.hide();
        dom.mineview.eq(index).show();
        gather.mine(index, a.eq(index).attr('type'));
    };

    dom.mine.find('a').on('click', function () {
        var othis = $(this), index = othis.index();
        var type = othis.attr('type'), hash = othis.attr('hash');
        if (othis.attr('href') !== 'javascript:;') {
            return;
        }

        gather.tabshow(index);
        gather.minelog[type] = true;
        if (hash) {
            location.hash = hash;
        }
    });
    dom.mine[0] && gather.tabshow(0, location.hash.replace(/^#/, ''));

    //根据ip获取城市
    if ($('#LAY_city').val() === '') {
        $.getScript('http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js', function () {
            $('#LAY_city').val(remote_ip_info.city);
        });
    }

    //上传图片
    if ($('.upload-img')[0]) {
        layui.use('upload', function (upload) {
            var avatarAdd = $('.avatar-add');
            layui.upload({
                elem: '.upload-img input'
              , method: 'post'
              , url: '/api/uploadphoto/'
              , before: function () {
                  avatarAdd.find('.loading').show();
              }
              , success: function (res) {
                  if (res.status == 0) {
                      updatePhoto(res.data.src);
                  } else {
                      layer.msg(res.msg, { icon: 5 });
                  }
                  avatarAdd.find('.loading').hide();
              }
              , error: function () {
                  avatarAdd.find('.loading').hide();
              }
            });
        });
    }

    function updateInfo(data) {
        var cacheUser = localStorage.getItem("fly_user");
        if (cacheUser) {
            var u = JSON.parse(cacheUser);

            u.nickname = data.nickname;
            u.sex = data.sex;
            u.sign = data.sign;
            u.city = data.city;

            localStorage.setItem("fly_user", JSON.stringify(u));
            $('#global_username').text(u.nickname);
        }
    }

    function updatePhoto(photo) {
        var cacheUser = localStorage.getItem("fly_user");
        if (cacheUser) {
            var u = JSON.parse(cacheUser);

            u.photo = photo;

            localStorage.setItem("fly_user", JSON.stringify(u));
            $('img[name="global_userphoto"]').attr('src', photo);
        }
    }
    //提交成功后刷新
    fly.form['/user/update'] = function (data, required) {
        console.log(data);
        updateInfo(data);

        layer.msg('修改成功', {
            icon: 1
          , time: 1000
          , shade: 0.1
        }, function () {
           // location.reload();
        });
    }
    //提交成功后刷新
    fly.form['/user/updatepwd'] = function (data, required) {
        localStorage.clear();
        layer.msg('修改成功,请重新登录', {
            icon: 1
          , time: 1000
          , shade: 0.1
        }, function () {
            location.reload();
        });
    }

    //帐号绑定
    $('.acc-unbind').on('click', function () {
        var othis = $(this), type = othis.attr('type');
        layer.confirm('整的要解绑' + ({
            qq_id: 'QQ'
          , weibo_id: '微博'
        })[type] + '吗？', { icon: 5 }, function () {
            fly.json('/api/unbind', {
                type: type
            }, function (res) {
                if (res.status === 0) {
                    layer.alert('已成功解绑。', {
                        icon: 1
                      , end: function () {
                          location.reload();
                      }
                    });
                } else {
                    layer.msg(res.msg);
                }
            });
        });
    });

    //我的消息
    gather.minemsg = function () {
        var delAll = $('#LAY_delallmsg')
        , tpl = '{{# var len = d.data.length;\
    if(len === 0){ }}\
      <div class="fly-none">您暂时没有最新消息</div>\
    {{# } else { }}\
      <ul class="mine-msg">\
      {{# for(var i = 0; i < len; i++){ }}\
        <li data-id="{{d.data[i].id}}"><a href="{{d.data[i].href}}" target="_blank">{{ d.data[i].msg}}</a><p><span>{{d.data[i].addtimestr}}</span><a href="javascript:;" class="layui-btn layui-btn-small fly-delete">删除</a></p></li>\
      {{# } }}\
      </ul>\
    {{# } }}'
        , delEnd = function (clear) {
            if (clear || dom.minemsg.find('.mine-msg li').length === 0) {
                dom.minemsg.html('<div class="fly-none">您暂时没有最新消息</div>');
            }
        }


        fly.json('/msg', {}, function (res) {
            var html = laytpl(tpl).render(res);
            dom.minemsg.html(html);
            if (res.data.length > 0) {
                delAll.removeClass('layui-hide');
            }
        }, { type: 'get' });

        //阅读后删除
        dom.minemsg.on('click', '.mine-msg li .fly-delete', function () {
            var othis = $(this).parents('li'), id = othis.data('id');
            fly.json('/delmsg', { id: id }, function (res) {
                if (res.status === 0) {
                    othis.remove();
                    delEnd();
                }
            });
        });

        //删除全部
        $('#LAY_delallmsg').on('click', function () {
            var othis = $(this);
            layer.confirm('确定清空吗？', function (index) {
                fly.json('/delmsg', {}, function (res) {
                    if (res.status === 0) {
                        layer.close(index);
                        othis.addClass('layui-hide');
                        delEnd(true);
                    }
                });
            });
        });

    };
    //处理评论格式
    gather.handlecomments = function () {
        var interval = setInterval(function () {
            //会有face没加载完的问题
            if (fly.faces) {
                clearInterval(interval);
                $('.home-dacontent').each(function (i, item) {
                    var content = $(this).html();
                    $(this).html(fly.content(content));
                });
            }
        }, 100);
    }
    //加载我的消息
    dom.minemsg[0] && gather.minemsg();
    //加载我的回复等
    dom.userhome[0] && gather.handlecomments();

    exports('user', null);

});