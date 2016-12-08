/**

 @Name: 求解板块

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
        jieda: $('#jieda')
      , content: $('#L_content')
      , jiedaCount: $('#jiedaCount')
    };

    //提交回答
    fly.form['/reply'] = function (data, required) {
        var tpl = '<li>\
      <div class="detail-about detail-about-reply">\
        <a class="jie-user" href="/user/">\
          <img src="{{= d.user.avatar}}" alt="{{= d.user.username}}">\
          <cite>{{d.user.username}}</cite>\
        </a>\
        <div class="detail-hits">\
          <span>刚刚</span>\
        </div>\
      </div>\
      <div class="detail-body jieda-body">\
        {{ d.content}}\
      </div>\
    </li>';
        var at = dom.content.attr('placeholder');
        if (at != '我要回答') {
            data.content = at + ' ' + data.content;
        }
        data.content = fly.content(data.content);
        laytpl(tpl).render($.extend(data, {
            user: layui.cache.user
        }), function (html) {
            required[0].value = '';
            dom.jieda.find('.fly-none').remove();
            dom.jieda.append(html);

            var count = dom.jiedaCount.text() | 0;
            dom.jiedaCount.html(++count);
        });
        dom.content.attr('placeholder', '我要回答');
        $('#replyid').val(0);
        $('#touserid').val(0);
    };

    //求解管理
    gather.jieAdmin = {
        //删求解
        del: function (div) {
            layer.confirm('确认删除该求解么？', function (index) {
                layer.close(index);
                fly.json('/api/jie-delete/', {
                    id: div.data('id')
                }, function (res) {
                    if (res.status === 0) {
                        location.href = '/jie/';
                    } else {
                        layer.msg(res.msg);
                    }
                });
            });
        }

        //设置置顶、状态
      , set: function (div) {
          var othis = $(this);
          fly.json('/api/jie-set/', {
              id: div.data('id')
            , rank: othis.attr('rank')
            , field: othis.attr('field')
          }, function (res) {
              if (res.status === 0) {
                  location.reload();
              }
          });
      }
    };
    //读取评论
    gather.getComment = function (artid) {
        fly.json('/comment', {
            artid: artid, userid: layui.cache.user.uid
        }, function (result) {
            if (result.data && result.data.length) {
                gather.bindComment(result.data);
            }
            //绑定点击事件
            gather.bindEvent();
        });
    }
    //根据模板绑定解答
    gather.bindComment = function (data) {
        console.log(layui.cache.user);
        var tpl = ' {{# for(var i = 0, len = d.length; i < len; i++){ }}\
          <li data-id="{{ d[i].id }}" data-userid="{{ d[i].userid }}" class="jieda-daan">\
   <a name="item-121212121212"></a>\
          <div class="detail-about detail-about-reply">\
            <a class="jie-user" href="">\
              <img src="{{ d[i].photo }}" alt="">\
              <cite>\
                <i>{{ d[i].nickname }}</i>\
                {{# if(d[i].author==true){ }}\
                    <em>(楼主)</em>\
                    {{#  } }}\
              </cite>\
            </a>\
            <div class="detail-hits">\
              <span>{{ d[i].addtimestr }}</span>\
            </div>\
          {{# if(d[i].isbest==true) { }}\
            <i class="iconfont icon-caina" title="最佳答案"></i>\
          {{# } }}\
          </div>\
          <div class="detail-body jieda-body">\
            <p>{{ d[i].comments }}</p>\
          </div>\
          <div class="jieda-reply">\
            <span class="jieda-zan {{# if(d[i].selfrecommend==true){ }} zanok{{# } }}" type="zan"><i class="iconfont icon-zan"></i><em>{{ d[i].recount }}</em></span>\
           {{# if(d[i].isself==false&&d[i].isdel!=true){ }} <span type="reply"><i class="iconfont icon-svgmoban53"></i>回复</span>{{# } }}\
          </div>\
            <div class="jieda-admin">\
            {{# if(d[i].isadmin==true&&d[i].isover==false){}}\
              <span class="jieda-accept" type="accept">采纳</span>\
            {{#}}}\
            </div>\
        </li>\
        {{# } }}';

        /*
      {{# if(d[i].author==true) }}\
                      <em>(楼主)</em>\
                      {{#  } }}\
          <!-- <em>(楼主)</em>\
                  <em style="color:#5FB878">(管理员)</em>\
                  <em style="color:#FF9E3F">（活雷锋）</em>\
                  <em style="color:#999">（该号已被封）</em> -->\
        //<!-- <div class="jieda-admin">\
              //  <span type="edit">编辑</span>\
              //  <span type="del">删除</span>\
              //  <span class="jieda-accept" type="accept">采纳</span>\
              //</div> -->
        */
        for (var i = 0; i < data.length; i++) {
            data[i].comments = fly.content(data[i].comments);
        }
        if (data.length) {
            dom.jieda.find('.fly-none').remove();

            laytpl(tpl).render(data, function (html) {
                dom.jieda.append(html);
                //总条数
                dom.jiedaCount.html(data.length);
            });
        }
    }

    $('.jie-admin').on('click', function () {
        var othis = $(this), type = othis.attr('type');
        gather.jieAdmin[type].call(this, othis.parent());
    });

    //解答操作
    gather.jiedaActive = {
        zan: function (li) { //赞
            var othis = $(this), ok = othis.hasClass('zanok');
            fly.json('/good', {
                commentid: li.data('id'), artid: $('#jid').val(), touserid: li.data('userid')
            }, function (res) {
                if (res.status === 0) {
                    var zans = othis.find('em').html() | 0;
                    othis[ok ? 'removeClass' : 'addClass']('zanok');
                    othis.find('em').html(ok ? (--zans) : (++zans));
                } else {
                    layer.msg(res.msg);
                }
            });
        }
      , reply: function (li) { //回复
          var val = dom.content.val();
          var aite = '@' + li.find('.jie-user cite i').text().replace(/\s/g, '');
          dom.content.focus()
          if (val.indexOf(aite) !== -1) return;
          //改为只回复一个人
          dom.content.attr('placeholder', aite);;
          // dom.content.val(aite + ' ' + val);
          console.log("回复的id是：" + li.data('id'));
          var replyid = li.data('id') || 0;
          var touserid = li.data('userid') || 0;
          $('#replyid').val(replyid);
          $('#touserid').val(touserid);
      }
      , accept: function (li) { //采纳
          var othis = $(this);
          layer.confirm('是否采纳该回答为最佳答案？', function (index) {
              layer.close(index);
              fly.json('/api/jieda-accept/', {
                  id: li.data('id')
              }, function (res) {
                  if (res.status === 0) {
                      $('.jieda-accept').remove();
                      li.addClass('jieda-daan');
                      li.find('.detail-about').append('<i class="iconfont icon-caina" title="最佳答案"></i>');
                  } else {
                      layer.msg(res.msg);
                  }
              });
          });
      }
      , edit: function (li) { //编辑
          fly.json('/jie/getDa/', {
              id: li.data('id')
          }, function (res) {
              var data = res.rows;
              layer.prompt({
                  formType: 2
               , value: data.content
               , maxlength: 100000
              }, function (value, index) {
                  fly.json('/jie/updateDa/', {
                      id: li.data('id')
                    , content: value
                  }, function (res) {
                      layer.close(index);
                      li.find('.detail-body').html(fly.content(value));
                  });
              });
          });
      }
      , del: function (li) { //删除
          layer.confirm('确认删除该回答么？', function (index) {
              layer.close(index);
              fly.json('/api/jieda-delete/', {
                  id: li.data('id')
              }, function (res) {
                  if (res.status === 0) {
                      var count = dom.jiedaCount.text() | 0;
                      dom.jiedaCount.html(--count);
                      li.remove();
                      //如果删除了最佳答案
                      if (li.hasClass('jieda-daan')) {
                          $('.jie-status').removeClass('jie-status-ok').text('求解中');
                      }
                  } else {
                      layer.msg(res.msg);
                  }
              });
          });
      }
    };
  
    gather.bindEvent = function () {
        $('.jieda-reply span').on('click', function () {
            if (layui.cache.user.uid == -1) {
                layer.alert("你还没有登录哦");
                return;
            }
            var othis = $(this), type = othis.attr('type');
            gather.jiedaActive[type].call(this, othis.parents('li'));
        });
    }
    $(function () {
        gather.getComment(articleId);
    });

    exports('jie', null);
});