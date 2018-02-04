//临时标识管理对象
var unapproveManager = {};

var unapproveTime = '0';

//轮询
unapproveManager.Polling = function () {
    unapproveManager.getdefectCond();
}

unapproveManager.Start = function () {
    this.getdefectCond();
    unapproveTime = setInterval(this.Polling, 20000); //120000为每120秒调用一次方法
}

unapproveManager.End = function () {
    if (unapproveTime != "0") {
        clearInterval(unapproveTime);
        unapproveTime = "0";
    }
}

 

unapproveManager.getdefectCond = function () {
    var _this = this;
    $.ajax({
        url: '/POLICY/POLICY_APPROVE_DASHBOARD/GetPolicyStatisticsInfo',
        cache: false,
        dataType: 'JSON',
        type: 'post',
        success: function (result) {
            //静态图表 开始

            $("#containerYS").html("");

            if (result.statusstatisticsList != null) {

                var wait_handle = "";
                for (var i = 0; i < result.statusstatisticsList.length; i++) {
                    wait_handle += ' <li class="block" index="5" style="margin-left: 7px; margin-right: 7px;">' +
                                       ' <div class="img">' +
                                       '       <p><a class="icon-text" href="#" onclick="view(\'' + result.statusstatisticsList[i].STATUS_NAME + '\',\'' + result.statusstatisticsList[i].STATUS_URL + '\')"><img src="../..//content/images/sp_' + result.statusstatisticsList[i].STATUS_CODE + '.png"></a></p>' +
                                       '       <div  class="count count' + result.statusstatisticsList[i].ACCEPT_COUNT + '"></div>' +
                                       ' </div>' +
                                       '   <a class="icon-text" href="#" onclick="view(\'' + result.statusstatisticsList[i].STATUS_NAME + '\',\'' + result.statusstatisticsList[i].STATUS_URL + '\')"><span>' + result.statusstatisticsList[i].STATUS_NAME + '</span></a>' +
                                   '</li>';
                }
                $("#containercase_desc").html(wait_handle);
            }
            else {
                $("#containercase_desc").html("");
            }
            //静态图表 结束
        }
    });
}

 