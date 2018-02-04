(function ($) {
    $.fn.extend({
        Scroll: function (opt, callback) {
            //参数初始化
            if (!opt) var opt = {};
            var _this = this.eq(0).find("ul:first");
            var lineH = _this.find("li:first").height(), //获取行高
                    line = opt.line ? parseInt(opt.line, 10) : parseInt(this.height() / lineH, 10), //每次滚动的行数，默认为一屏，即父容器高度
                    speed = opt.speed ? parseInt(opt.speed, 10) : 500, //卷动速度，数值越大，速度越慢（毫秒）
                    timer = opt.timer ? parseInt(opt.timer, 10) : 3000; //滚动的时间间隔（毫秒）
            if (line == 0) line = 1;
            var upHeight = 0 - line * lineH;
            //滚动函数
            scrollUp = function () {
                _this.animate({
                    marginTop: upHeight
                }, speed, function () {
                    for (i = 1; i <= line; i++) {
                        _this.find("li:first").appendTo(_this);
                    }
                    _this.css({ marginTop: 0 });
                });
            }
            //鼠标事件绑定
            _this.hover(function () {
                clearInterval(timerID);
            }, function () {
                timerID = setInterval("scrollUp()", timer);
            }).mouseout();
        }
    })
})(jQuery);
var editIndex = 0;
$(function () {
    document.oncontextmenu = function (e) { return false; }
    var form = layui.form;
    var upload = layui.upload; //得到 upload 对象
    form.render();
    form.verify({
        username: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '用户名不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '用户名首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '用户名不能全为数字';
            }
        }
      , pass: [
        /^[\S]{3,12}$/
        , '密码必须3到12位，且不能出现空格'
      ]
    });

    //监听提交
    form.on('submit(loginBut)', function (data) {
        if ($("#loginBut").attr("class").indexOf("layui-btn-disabled") == "-1") {//重复提交
            saveSubmit(data.field);
        }
        return false;
    });
    $('#loginOut').click(function () {
        layer.confirm('您确定要注销本次登录吗？', {
            shade: false, //不显示遮罩
            icon: 3,
            title: '系统提示',
            btn: ['注销', '取消'] //按钮
        }, function () {
            $.ajax({
                url: "/Users/CompanyLoginOut",
                success: function (result) {
                    result = $.parseJSON(result);
                    if (!result.Statu) { return; }
                    if (result.Statu == "1") {
                        window.location.href = result.BackUrl;
                    } else {
                        layer.msg(result.Msg, { title: '注销失败', icon: 2 });
                    }
                }
            });
        }, function () { });
    });
    $('#editPass').click(function () {
        editIndex = layer.open({
            type: 2,
            title: "修改密码",
            shadeClose: true,
            shade: 0.5,
            maxmin: false,
            area: ['640px', '300px'],
            content: "/COMPANY/Apply/EditPass"
        });
    });
    $('#showCompany').click(function () {
        layer.open({
            type: 2,
            title: "企业信息",
            shadeClose: true,
            shade: 0.5,
            maxmin: false,
            area: ['890px', '600px'],
            content: "/COMPANY/Apply/CompanyXx"
        });
    });

    
    if (typeof loadInit == "function") {//存在且是function
        loadInit();//
    }
    if (typeof loadForm == "function") {//存在且是function
        loadForm(form);//
    }
    if (typeof loadUpload == "function") {//存在且是function
        loadUpload(upload);//
    }
});



function ClickRemoveChangeCode() {
    var code = $("#imgCode").attr("src");
    $("#imgCode").attr("src", code + "1");
}