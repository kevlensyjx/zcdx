//关闭窗口
function closePwd() {
    $('#editpass').window('close');
}
function closedTab() {
    var $tab = $('#tabs');
    var tab = $tab.tabs('getSelected');
    var index = $tab.tabs('getTabIndex', tab);
    $tab.tabs('close', index);
}
function serverLogin() {
    $('#form-pass').form('submit', {
        url: '/ADMIN/Admin/EditPass',
        cache: false,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (result) {
            var jsonData = $.parseJSON(result);
            var state = parseInt(jsonData.Statu);
            if (state == 1) {//OK
                $('#form-pass').form('clear');
                $('#editpass').window('close');
                $.alertMsg(jsonData.Msg, '操作提示', null);
            }
            else if (state == 4) {
                $('#form-pass').form('clear');
                $('#editpass').window('close');
                $.alertMsg(jsonData.Msg, '操作提示', function () {
                    if (window != top) {
                        top.location.href = jsonData.BackUrl;
                    }
                    else {
                        window.location.href = jsonData.BackUrl;
                    }
                });
            } else {
                if (jsonData.Msg == '原密码不正确！请重新操作！') {//原密码不匹配
                    $('#txtoldpass').textbox('clear');
                }
                else {//两次输入密码不匹配
                    $('#txtnewpass').textbox('clear');
                    $('#txtrepass').textbox('clear');
                    $('#weak').css({ backgroundColor: '' });
                    $('#middle').css({ backgroundColor: '' });
                    $('#strength').css({ backgroundColor: '' });
                    $('#weak').html('&nbsp;');
                    $('#middle').html('&nbsp;');
                    $('#strength').html('&nbsp;');
                }

                $.alertMsg(jsonData.Msg, '操作提示', null);
            }

        }
    });
}
//打开TAB
function addTabInIndex(subtitle, url, icon) {
    if (!url || url == '#') return false;
    $.ajax({
        url: '/api/Base/IsLogin',
        data: {},
        type: "Post",
        success: function (result) {
            if (result.Msg) {//如果没登录或没权限
                $.procAjaxMsg(result, function () { }, function () {
                    $.alertMsg(result.Msg, '操作提示', null);
                });
                return false;
            } else {
                var $tab = $('#tabs');
                var tabCount = $tab.tabs('tabs').length;
                var hasTab = $tab.tabs('exists', subtitle);
                if ((tabCount <= 10) || hasTab)
                    openTabHandler($tab, hasTab, subtitle, url, icon);
                else
                    $.messager.confirm("系统提示", '<b>您当前打开了太多的页面，如果继续打开，会造成程序运行缓慢，无法流畅操作！</b>', function (b) {
                        if (b)
                            openTabHandler($tab, hasTab, subtitle, url, icon);
                    });
            }
        }
    });
};

function createFrame(url) {
    var framename = url.split('/')[2];
    if (url.split('/')[3] == "GoogleGISList") {
        framename = "GoogleGISList";
    }
    return '<iframe scrolling="no" frameborder="0"  style="width:100%;height:99.4%;border:0px;" src="' + url + '" name="' + framename + '"></iframe>';
}
function createFrameName(url, idname) {
    return '<iframe scrolling="no" id="' + idname + '"  frameborder="0"  style="width:100%;height:99.4%;border:0px;" src="' + url + '" name="' + idname + '"></iframe>';
}
function openTabHandler($tab, hasTab, subtitle, url, icon) {
    if (!hasTab) {
        if (subtitle == '我的桌面') {
            $tab.tabs('add', { title: subtitle, content: createFrame(url), closable: false, icon: icon });
        }
        else {
            $tab.tabs('add', { title: subtitle, content: createFrame(url), closable: true, icon: icon });
        }
    } else {
        $tab.tabs('select', subtitle);
        if (subtitle == "轨迹回放") {
            var urls = url.split('/');
            if (urls[urls.length - 1] != "null") {
                //var currentTab = $tab.tabs('getSelected');
                //var src = $(currentTab.panel('options').content).attr('src');
                //$tab.tabs('update', { tab: currentTab, options: { content: createFrame(url) } });
                GoogleGISList.window.SetPhone(urls[urls.length - 1]);
            }
        }
        if (subtitle == "报警记录" || subtitle.indexOf("计划作业") > -1 ) {
            var currentTab = $tab.tabs('getSelected');
            $tab.tabs('update', { tab: currentTab, options: { content: createFrame(url) } });
        }
    }
};
function setFullScreen() {
    var that = $(this);
    if (that.find('.icon-screen_full').length) {
        if (!document.fullscreenElement && // alternative standard method
    !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) { // current working methods
            if (document.documentElement.requestFullscreen) {
                document.documentElement.requestFullscreen();
            } else if (document.documentElement.msRequestFullscreen) {
                document.documentElement.msRequestFullscreen();
            } else if (document.documentElement.mozRequestFullScreen) {
                document.documentElement.mozRequestFullScreen();
            } else if (document.documentElement.webkitRequestFullscreen) {
                document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            }
        }


        that.find('.icon-screen_full').removeClass('icon-screen_full').addClass('icon-screen_actual');
    } else if ($(this).find('.icon-screen_actual').length) {
        //退出全屏
        if (!document.fullscreenElement && // alternative standard method
        !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) { // current working methods
        } else {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            }
        }

        that.find('.icon-screen_actual').removeClass('icon-screen_actual').addClass('icon-screen_full');
    }
};
function tabRefresh(url) {
    var $tab = $("#tabs");
    var currentTab = $tab.tabs('getSelected');
    var src = $(currentTab.panel('options').content).attr('src');

    $.ajax({
        url: '/api/Base/IsLogin',
        data: {},
        type: "Post",
        success: function (result) {
            if (result.Msg) {//如果没登录或没权限
                $.procAjaxMsg(result, function () { }, function () {
                    $.alertMsg(result.Msg, '操作提示', null);
                });
                return false;
            } else {
                $tab.tabs('update', { tab: currentTab, options: { content: createFrame(src) } });
            }
        }
    });
};
function tabClose() {
    if ($('#tabs').tabs('tabs').length > 0) {
        $.messager.confirm('系统提示', '当前操作会关闭所有打开窗口，确认吗?', function (r) {
            if (r) {
                closeAllTabexceptGIS();
            }
        });
    }
};

function getTabTitles($tab) {
    var titles = [];
    var tabs = $tab.tabs('tabs');
    $.each(tabs, function () { titles.push($(this).panel('options').title); });
    return titles;
};
function toggleFullScreen() {
    if (/msie/.test(navigator.userAgent.toLowerCase())) {
        var WsShell = new ActiveXObject('WScript.Shell')
        WsShell.SendKeys('{F11}');
    } else {
        toggleFullScreen_NoIE();
    }
}
function toggleFullScreen_NoIE() {
    if (!document.fullscreenElement && // alternative standard method
    !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.msRequestFullscreen) {
            document.documentElement.msRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    }
}


//根据选择的子系统获取一级菜单数据
function LoadSystem(systemid, ishasPermission) {
    if (ishasPermission == 'N') {
        $.alertMsg("您没有此模块权限!", '操作提示', null);
        return;
    }
    $.ajax({
        url: '/SYS/Admin/GetMySelectOneMenu/' + systemid,
        async: false,
        type: 'POST',
        success: function (menustr) {
            if (menustr.Msg) {//如果没登录或没权限
                $.procAjaxMsg(menustr, function () { }, function () {
                    $.alertMsg(menustr.Msg, '操作提示', null);
                });
            };
            menuButton(menustr);
            changeLogoandSoOnBySelect(systemid, menustr);
        }
    });
    //$('#showsystem').window('close');
}
//绑定一级菜单数据（上）
function menuButton(menus) {
    var menus = $.parseJSON(menus);
    var menulist = "";
    var firstMenuID = "";
    var hasPermission = false;
    $.each(menus, function (i, n) {
        menulist += utils.formatString('<a href="javascript:void(0)" id="mb{0}" class="easyui-linkbutton" plain="true" data-options="iconCls:\'{1}\'"   onclick="javascript:{3}">{2}</a>',
            (i + 1), n.iconCls, n.text, function () {
                return 'GetMyMenu(\'' + n.id + '\')';
            });

        if (i == 0) {
            hasPermission = true;
        }
        if (n.isdefuopen == 'Y') {
            firstMenuID = n.id;
        }
    });
    $('#MenuOne').html(menulist);

    closeAllTabexceptGIS();

    if (hasPermission) {
        GetMyMenu(firstMenuID);//非GIS
    }
    else {
        ClearMenuAccordion($('#MenuTree'));//没有此系统下任何一个一级菜单权限
    }
    var mb = $('#MenuOne .easyui-linkbutton').linkbutton();
};
//GIS常开，其他关闭
function closeAllTabexceptGIS() {
    var $tab = $('#tabs');
    var titles = getTabTitles($tab);
    $.each(titles, function (i, n) {
        if (n != "我的桌面") {
            $tab.tabs('close', this);
        }
    });
}
//获取二级菜单数据
function GetMyMenu(id) {
    $.ajax({
        url: '/SYS/Admin/GetMyMenu/' + id,
        type: 'POST',
        async:false,//同步执行
        success: function (menustr) {
            if (menustr.Msg) {
                $.procAjaxMsg(menustr, function () { }, function () {
                    $.alertMsg(menustr.Msg, '操作提示', null);
                });
            };
            menuAccordion(menustr);
            if ($('#west').is(':hidden')) {
                $('#my_body').layout('expand', 'west');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) { alert(XMLHttpRequest); }
    });
}
//绑定二级菜单（左）,并且打开已设置的默认页面（在此处是为了加上权限控制）
var accname = "";
function menuAccordion(menus) {
    var menus = $.parseJSON(menus);
    var $obj = $('#MenuTree');
    $obj.accordion({ animate: true, fit: true, border: false });//,multiple:true
    ClearMenuAccordion($obj);
    var selTwoTitle = '';
    var selThreeTitle = '';
    var selThreeUrl = '';
    var selThreeIcon = '';
    $.each(menus, function () {
        accname += this.text + ",";
        var html = '<ul>';
        var temple = '<li><div ><a ref="{0}" href="javascript:void(0)" rel="{1}"><span class="icon {2}">&nbsp;</span><span class="nav">{3}</span></a></div></li>';
        $.each(this.children || [], function () {
            html += utils.formatString(temple, this.id, this.attributes.url, this.iconCls, this.text);
        });
        html += '</ul>';

        $obj.accordion('add', {
            title: this.text,
            content: html,
            iconCls: 'icon ' + this.iconCls,
            border: false
        });

        //if (this.isdefuopen == 'Y') {
        //    selTwoTitle = this.text;
        //}
    });

    //选择二级菜单
    var panels = $obj.accordion('panels');
    if (selTwoTitle == '') {
        selTwoTitle = panels[0].panel('options').title;
    }
    if (panels.length > 0) {
        $obj.accordion('select', selTwoTitle);
    }

    $obj.find('li').click(function () {
        $obj.find('li div').removeClass("selected");
        $(this).children('div').addClass("selected");

        var link = $(this).find('a');
        var title = link.children('.nav').text();
        var url = link.attr("rel");
        var code = link.attr("ref");
        var icon = link.children('.icon').attr('class');

        addTabInIndex(title, url, icon);
    }).hover(function () {
        $(this).children('div').addClass("hover");
    }, function () {
        $(this).children('div').removeClass("hover");
    });
    //打开三级菜单
    if (selThreeTitle != '' && selThreeUrl != '' && selThreeIcon != '') {
        addTabInIndex(selThreeTitle, selThreeUrl, selThreeIcon);
    }
};

//清除并加载左侧菜单
function closeAndShowWestMenu(systemid) {
    GetMyMenu(systemid);
}
//清除二级菜单
function ClearMenuAccordion($obj) {
    if (accname != "") {
        var panelsx = accname.split(',');
        if (panelsx.length > 0) {
            for (var i = 0; i < panelsx.length; i++) {
                if (panelsx[i] != "") {
                    $obj.accordion('remove', panelsx[i]);
                }
            }
        }
    }
    accname = "";
}
//改变logo,help文档需要的Value,选择GIS菜单
function changeLogoandSoOnBySelect(systemid, oneMenus) {
    //改变系统Logo
    if (systemid != '') {
        $('.head-left').css('background', 'url(/Content/images/logo/logo_' + systemid + '.png) no-repeat left');
    }
    //根据选择的子系统自动切换GIS菜单
    if (systemid != '') {
        var selIndex = 0;
        var GISMenus = GIS.window.GISmenu;
        $.each(GISMenus, function (i, n) {
            if (n.id.substring(0, 2) == systemid) selIndex = i;
        });
        GIS.window.SelectSpecialAccordion(selIndex);
    }
}
function noticeAssign(num) {
    if (num == null) {
        $('#weak').css({ backgroundColor: '#dd0000' });
        $('#middle').css({ backgroundColor: '' });
        $('#strength').css({ backgroundColor: '' });
        $('#weak').html('&nbsp;&nbsp;&nbsp;&nbsp;弱');
        $('#middle').html('&nbsp;');
        $('#strength').html('&nbsp;');
    } else

        if (num == 0) {
            $('#weak').css({ backgroundColor: '#dd0000' });
            $('#middle').css({ backgroundColor: '' });
            $('#strength').css({ backgroundColor: '' });
            $('#weak').html('&nbsp;&nbsp;&nbsp;&nbsp;弱');
            $('#middle').html('&nbsp;');
            $('#strength').html('&nbsp;');
        } else

            if (num == -1) {
                $('#weak').css({ backgroundColor: '#ffcc33' });
                $('#middle').css({ backgroundColor: '#ffcc33' });
                $('#strength').css({ backgroundColor: '' });
                $('#weak').html('&nbsp;');
                $('#middle').html('&nbsp;&nbsp;&nbsp;&nbsp;中');
                $('#strength').html('&nbsp;');
            } else

                if (num == 1) {
                    $('#weak').css({ backgroundColor: '#009900' });
                    $('#middle').css({ backgroundColor: '#009900' });
                    $('#strength').css({ backgroundColor: '#009900' });
                    $('#weak').html('&nbsp;');
                    $('#middle').html('&nbsp;');
                    $('#strength').html('&nbsp;&nbsp;&nbsp;很强');
                }
}


function closeProgress() {
    $.messager.progress('close');
}



















$(function () {
    document.oncontextmenu = function (e) {
        return false; // 主页面不允许右键（兼容多浏览器）
    }
    $('#tabs').tabs({
        tools: [{ iconCls: 'icon-arrow_refresh', handler: tabRefresh },
                { id: 'screen_full_but', iconCls: 'icon-screen_full', handler: setFullScreen },//setFullScreen 
                { iconCls: 'panel-tool-close', handler: tabClose }]
    });
    $('#screen_full_but').attr("title", "按F11");
    addTabInIndex("我的桌面", "/POLICY/POLICY_APPROVE_DASHBOARD/Index", "icon-world_key");
    //获取子系统导航页数据
    GetSysMenu();
    //根据用户默认子系统获取一级菜单数据
    $.ajax({
        url: '/SYS/Admin/GetMyOneMenu',
        async: false,
        type: 'POST',
        success: function (menustr) {
            if (menustr.Msg) {//如果没登录或没权限
                $.procAjaxMsg(menustr, function () { }, function () {
                    $.alertMsg(menustr.Msg, '操作提示', null);
                });
            };
            menuButton(menustr);
            //changeLogoandSoOnBySelect('', menustr);
        }
    });

    //设置默认子系统
    $('input[name=defuSys]').click(function () {
        $.ajax({
            url: '/SYS/Admin/SetDefuSystem/' + $('input:radio[name="defuSys"]:checked').val(),
            type: 'POST',
            success: function (result) {
                var result = $.parseJSON(result);
                $.procAjaxMsg(result, function () { $.alertMsg(result.Msg, '操作提示', null); }, function () { });
            }
        });
    });

    $('#changePass').click(function () {
        $('#editpass').window('open');
        $('#form-pass').form('clear');
    });
    closePwd();
    //验证密码强度
    $('#txtnewpass').textbox({
        onChange: function () {
            var val = this.value;
            if (val.length >= 4) {
                if (/[a-zA-Z]+/.test(val) && /[0-9]+/.test(val) && /\W+\D+/.test(val)) {
                    noticeAssign(1);
                } else if (/[a-zA-Z]+/.test(val) || /[0-9]+/.test(val) || /\W+\D+/.test(val)) {
                    if (/[a-zA-Z]+/.test(val) && /[0-9]+/.test(val)) {
                        noticeAssign(-1);
                    } else if (/\[a-zA-Z]+/.test(val) && /\W+\D+/.test(val)) {
                        noticeAssign(-1);
                    } else if (/[0-9]+/.test(val) && /\W+\D+/.test(val)) {
                        noticeAssign(-1);
                    } else {
                        noticeAssign(0);
                    }
                }
            } else {
                noticeAssign(null);
            }
        }
    });

    $('#btnEp').click(function () {
        serverLogin();
    })

    $('#btnCancel').click(function () { closePwd(); })


    $('#loginOut').click(function () {
        $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {
            if (r) {
                $.ajax({
                    url: "/Home/loginOut",
                    type: 'POST',
                    success: function (result) {
                        result = $.parseJSON(result);
                        $.procAjaxMsg(result, function () {
                            window.location.href = result.BackUrl;
                        }, function () {
                            $.alertMsg(result.Msg, '退出失败', null);
                        })
                    }
                });
            }
        });

    })
});

//获取子系统导航页数据
function GetSysMenu() {
    //获取子系统数据
    $.ajax({
        url: '/SYS/Admin/GetMyZeroMenu',
        type: 'POST',
        async: false,
        success: function (sysData) {
            if (sysData.Msg) {//如果没登录或没权限
                $.procAjaxMsg(sysData, function () { }, function () {
                    $.alertMsg(sysData.Msg, '操作提示', null);
                });
            };
            var syss = $.parseJSON(sysData);
            initSystemPage(syss);
        }
    });
}

//绑定子系统导航页元素
function initSystemPage(sysData) {
    var syslist = '<div>';
    if (sysData.length > 21) {
        syslist = '<div style="overflow-y:scroll;height:550px;width:180px;">';
    }
    $.each(sysData, function (i, n) {
        syslist += utils.formatString(
            '<a class="easyui-linkbutton" draggable="false" href="javascript:void(0)" plain="true" icon="{0}" title="{1}" onclick="javascript:LoadSystem(\'{2}\',\'{4}\')"><input name="defuSys" {5} value=\'{2}\'>{1}</a>',
            n.iconCls, n.text, n.id, function () {
                if (n.haspermission == "N") {
                    return 'titleDisable';
                }
                return 'title';
            }, n.haspermission, function () {
                if (n.haspermission == "N") {
                    return 'type="radio" disabled="true"';
                }
                if (n.haspermission == "Y") {
                    if (n.isdefu == "Y") {
                        return 'type="radio" checked="checked"';
                    }
                    return 'type="radio"';
                }
            }
            );
    });
    syslist += '</div>';
    $('#NavSystem').html(syslist);
    var sys = $('#NavSystem .easyui-linkbutton').linkbutton();
}