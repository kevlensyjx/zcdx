using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using SolutionWeb.Model.SYS;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_POLICY_MAIN_INFO
    {
        #region 获取当前数据可审批节点
        public VIEW_POLICY_MAIN_INFO_APPROVE GetApprovelInfo(string id)
        {
            VIEW_POLICY_MAIN_INFO_APPROVE mainModel = new VIEW_POLICY_MAIN_INFO_APPROVE();

            POLICY_MAIN_INFO mModel =
                bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID.Equals(id));
            if (mModel != null)
            {
                List<POLICY_STATUS_CHANGE> statusChangeList = mModel.POLICY_STATUS_CHANGE.ToList();
                mainModel.CORP_NAME = mModel.CORP_NAME;
                mainModel.APPLY_NUMBER = mModel.APPLY_NUMBER;
                mainModel.APPLY_ITEM_TYPE = mModel.APPLY_ITEM_TYPE;
                mainModel.APPLY_ITEM_NAME = mModel.APPLY_ITEM_NAME;

                mainModel.IS_SHOW_3 = "0";
                mainModel.IS_SHOW_4 = "0";
                if (mModel.APPLY_ITEM_TYPE == "评审类")
                {
                    mainModel.IS_SHOW_3 = "1";
                    mainModel.IS_SHOW_4 = "1";
                }


                #region 窗口受理--old

                //POLICY_STATUS_CHANGE status_change_model_1 = mModel.POLICY_STATUS_CHANGE
                //.Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.窗口受理).FirstOrDefault();

                //if (status_change_model_1 != null)
                //{
                //    string handleContent = "";
                //    if (status_change_model_1.IS_HANDLE == "0" &&
                //        status_change_model_1.DEPT_CODE == oc.CurrentUser.DEPT_CODE)
                //    {
                //        handleContent =
                //            "<textarea style='height:70px; width:98%;' maxlength='300' id='textarea_handle_content'></textarea>" +
                //            "<br> <input name='radio_approveresult' style='width: 15px;' type = 'radio'  value = '1' />通过" +
                //            "<input name = 'radio_approveresult' style = 'width: 15px;' type = 'radio'  value = '0' />驳回";
                //        mainModel.MAIN_INFO_STATUS_CODE = status_change_model_1.STATUS_CODE;
                //    }
                //    else if (status_change_model_1.IS_HANDLE == "1")
                //    {
                //        handleContent = "<div style='width: 100%; '>" + status_change_model_1.HANDLE_CONTENT + "</div>" +
                //                        "<div style = 'width:100%;text-align:right' > 承办人:"
                //                        + "<span style='margin-right:50px;'>" + status_change_model_1.HANDLE_NAME +
                //                        "</span>" +
                //                        "<br/><span style='margin-right:30px;'>" +
                //                        Convert.ToDateTime(status_change_model_1.HANDLE_DT).ToString("yyyy年MM月dd日") +
                //                        "</span></div> ";
                //    }
                //    mainModel.HANDLE_CONTENT_1 = handleContent;
                //}

                #endregion
                #region 窗口受理

                List<POLICY_STATUS_CHANGE> status_change_list_1 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.窗口受理).OrderBy(o => o.CREATE_DT).ToList();

                if (status_change_list_1.Count > 0)
                {
                    string handleContent = "";
                    foreach (var item in status_change_list_1.Where(o => o.IS_HANDLE == "1").ToList())
                    {
                        string handle_result = item.HANDLE_RESULT == "1" ? "通过" : "驳回";
                        handleContent += "<div id = 'seal_" + item.SID + "'><div  style='width: 100%; '>审批结果：" + handle_result + "；意见：" + item.HANDLE_CONTENT + "</div>" +
                                       "<div style = 'width:100%;text-align:right' > 承办人:"
                                       + "<span style='margin-right:50px;'>" + item.HANDLE_NAME +
                                       "</span>" +
                                       "<br/><span style='margin-right:30px;'>" +
                                       Convert.ToDateTime(item.HANDLE_DT).ToString("yyyy年MM月dd日") +
                                       "</span></div></div> ";
                    }


                    POLICY_STATUS_CHANGE status_change_model_1 = status_change_list_1.Where(o => o.IS_HANDLE == "0").FirstOrDefault();



                    if (status_change_model_1 != null)
                    {
                        if (status_change_model_1.DEPT_CODE == oc.CurrentUser.DEPT_CODE)
                        {
                            string appply_result =
                           "<input name='radio_approveresult' style='width: 15px;' type = 'radio'  value = '1' />通过" +
                           "<input name = 'radio_approveresult' style = 'width: 15px;' type = 'radio'  value = '0' />驳回";
                            //disabled
                            handleContent +=
                                    "<div class='part00'>" +
                                     "    <div  class='part11' id = 'seal_" + status_change_model_1.SID + "'>" +
                                     "       <div class='part14'> <textarea class='part15' maxlength='300' id='textarea_handle_content'></textarea>" +
                                     "        <br>" + appply_result +
                                     "    </div></div>" +
                                     "    <div  class='part12' >" +
                                     "        <div class='part13'  id = '" + status_change_model_1.SID + "'>" + status_change_model_1.DEPT_NAME + "</br>" +
                                     "        负责人:" + oc.CurrentUser.ZSNAME + "<br/>" +
                                            Convert.ToDateTime(DateTime.Now).ToString("yyyy年MM月dd日") +
                                     "        <br/><input type='button' value='审批' id='seal_button' name ='seal_button' onclick=\"seal('" + status_change_model_1.SID + "','seal_" + status_change_model_1.SID + "')\" />" +
                                     "</div></div></div> ";
                            mainModel.MAIN_INFO_STATUS_CODE = status_change_model_1.STATUS_CODE;
                        }
                    }

                    mainModel.HANDLE_CONTENT_1 = handleContent;
                }

                #endregion

                #region 业务审核

                POLICY_STATUS_CHANGE status_change_model_2 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.业务核定).FirstOrDefault();
                if (status_change_model_2 != null)
                {
                    string handleContent = "";
                    if (status_change_model_2.IS_HANDLE == "0" &&
                        status_change_model_2.DEPT_CODE == oc.CurrentUser.DEPT_CODE)
                    {
                        string appply_result = "";
                        if (status_change_model_2.ITEM_TYPE == "评审类")
                            appply_result = "<input name='radio_approveresult' style='width: 15px;' checked type = 'radio'  value = '1' />已审";
                        else
                            appply_result = "<input name='radio_approveresult' style='width: 15px;' checked type = 'radio'  value = '1' />已审<input name = 'radio_approveresult' style = 'width: 15px;' type = 'radio'  value = '0' />驳回";

                        handleContent +=
                                    "<div class='part00'>" +
                                     "    <div  class='part11' id = 'seal_" + status_change_model_2.SID + "'>" +
                                     "       <div class='part14'> <textarea class='part15' maxlength='300' id='textarea_handle_content'></textarea>" +
                                     "        <br>" + appply_result +
                                     "    </div></div>" +
                                     "    <div  class='part12' >" +
                                     "        <div class='part13'  id = '" + status_change_model_2.SID + "'>" + status_change_model_2.DEPT_NAME + "</br>" +
                                     "        负责人:" + oc.CurrentUser.ZSNAME + "<br/>" +
                                            Convert.ToDateTime(DateTime.Now).ToString("yyyy年MM月dd日") +
                                     "        <br/><input type='button' value='审批' id='seal_button' name ='seal_button' onclick=\"seal('" + status_change_model_2.SID + "','seal_" + status_change_model_2.SID + "')\" />" +
                                     "</div></div></div> ";
                        mainModel.MAIN_INFO_STATUS_CODE = status_change_model_2.STATUS_CODE;
                    }
                    else if (status_change_model_2.IS_HANDLE == "1")
                    {
                        string handle_result = status_change_model_2.HANDLE_RESULT == "1" ? "通过" : "驳回";
                        handleContent = "<div class='part00'><div  class='part11' id = 'seal_" + status_change_model_2.SID + "'>审批结果：" + handle_result + "；意见：" + status_change_model_2.HANDLE_CONTENT + "</div>" +
                                        "<div  class='part12' ><div class='part13'  id = '" + status_change_model_2.SID + "'>" + status_change_model_2.DEPT_NAME + "</br>" +
                                        "负责人:" + status_change_model_2.HANDLE_NAME + 
                                        "<br/>" +
                                        Convert.ToDateTime(status_change_model_2.HANDLE_DT).ToString("yyyy年MM月dd日") +
                                        "</div></div></div> ";
                    }
                    mainModel.HANDLE_CONTENT_2 = handleContent;
                }

                #endregion

                #region 部门会审
                if (mainModel.APPLY_ITEM_TYPE == "评审类" || mainModel.APPLY_ITEM_TYPE == "普惠类")
                { mainModel.IS_SHOW_3 = "1"; }


                List<POLICY_STATUS_CHANGE> status_change_list_3 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.部门会审).OrderBy(o => o.CREATE_DT).ToList();
                if (status_change_list_3.Count > 0)
                {

                    POLICY_STATUS_CHANGE status_change_model_3 =
                        status_change_list_3.Where(o => o.DEPT_CODE == oc.CurrentUser.DEPT_CODE).FirstOrDefault();
                    if (status_change_model_3 != null)
                    {

                        string handleContent = "";
                        List<POLICY_STATUS_CHANGE> hs_statusChangeList = status_change_list_3
                            .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.部门会审 && o.DEPT_CODE != oc.CurrentUser.DEPT_CODE).ToList();

                        #region 判断显示：通过或驳回 意见框
                        string appply_result = "";
                        if (mainModel.APPLY_ITEM_TYPE == "评审类")
                        {
                            appply_result = "<input name='radio_approveresult' style='width: 15px;' checked type = 'radio'  value = '1' />已审";
                        }
                        else
                        {
                            appply_result = "<input name='radio_approveresult' style='width: 15px;' type = 'radio'  value = '1' />通过" +
                                "<input name = 'radio_approveresult' style = 'width: 15px;' type = 'radio'  value = '0' />驳回";
                        }
                        #endregion

                        /*
                         * 其他部门的无论是否处理，都显示信息。
                         */
                        foreach (var hsmodel in hs_statusChangeList)
                        {
                            string a = string.IsNullOrEmpty(hsmodel.HANDLE_CONTENT) ? "" : hsmodel.HANDLE_CONTENT;
                            string b = string.IsNullOrEmpty(hsmodel.HANDLE_NAME) ? "" : hsmodel.HANDLE_NAME;
                            string c = !hsmodel.HANDLE_DT.HasValue ? "" : Convert.ToDateTime(hsmodel.HANDLE_DT).ToString("yyyy年MM月dd日");
                            string handle_result = "";
                            if (hsmodel.IS_HANDLE == "1")
                                handle_result = (hsmodel.HANDLE_RESULT == "1") ? "结果:通过" : "结果:驳回";
                            else
                                handle_result = "暂未审批";

                            handleContent +=
                                "<div class='part01'>" +
                                "  <div  class='part11' id = 'seal_" + hsmodel.SID + "'> &nbsp; &nbsp;" + handle_result + "; " + a + " </div>" +
                                "  <div  class='part12' ><div class='part13'  id = '" + hsmodel.SID + "'>" + hsmodel.DEPT_NAME +
                                "   </br>" + b + "&nbsp;&nbsp;</br>" + c + "&nbsp;</div>" +
                                "</div></div></div>";
                        }
                        if (status_change_model_3.IS_HANDLE == "0")
                        {
                            handleContent +=
                                "<div class='part00'>" +
                                 "    <div  class='part11' id = 'seal_" + status_change_model_3.SID + "'>" +
                                 "       <div class='part14'> <textarea class='part15' maxlength='300' id='textarea_handle_content'></textarea>" +
                                 "        <br>" + appply_result +
                                 "    </div></div>" +
                                 "    <div  class='part12' >" +
                                 "        <div class='part13'  id = '" + status_change_model_3.SID + "'>" + status_change_model_3.DEPT_NAME + "</br>" +
                                 "        负责人:" + oc.CurrentUser.ZSNAME + "<br/>" +
                                        Convert.ToDateTime(DateTime.Now).ToString("yyyy年MM月dd日") +
                                 "        <br/><input type='button' value='审批' id='seal_button' name ='seal_button' onclick=\"seal('" + status_change_model_3.SID + "','seal_" + status_change_model_3.SID + "')\" />" +
                                 "</div></div></div> ";
                        }
                        else
                        {
                            string a = string.IsNullOrEmpty(status_change_model_3.HANDLE_CONTENT) ? "" : status_change_model_3.HANDLE_CONTENT;
                            string b = string.IsNullOrEmpty(status_change_model_3.HANDLE_NAME) ? "" : status_change_model_3.HANDLE_NAME;
                            string c = !status_change_model_3.HANDLE_DT.HasValue ? "" : Convert.ToDateTime(status_change_model_3.HANDLE_DT).ToString("yyyy年MM月dd日");
                            string handle_result = (status_change_model_3.HANDLE_RESULT == "1") ? "通过" : "驳回";
                            handleContent +=
                                "<div class='part00'>" +
                                "  <div  class='part11' id = 'seal_" + status_change_model_3.SID + "'> &nbsp; &nbsp;" + "结果: " + handle_result + "; " + a + " </div>" +
                                "  <div  class='part12' ><div class='part13'  id = '" + status_change_model_2.SID + "'>" + status_change_model_3.DEPT_NAME +
                                "   </br>" + b + "&nbsp;&nbsp;</br>" + c + "&nbsp;</div>" +
                                "</div></div></div>";
                        }

                        //disabled
                        mainModel.HANDLE_CONTENT_3 = handleContent;
                        mainModel.MAIN_INFO_STATUS_CODE = status_change_model_3.STATUS_CODE;
                    }
                    else {
                        string handleContent = "";
                        foreach (var hsmodel in status_change_list_3)
                        {
                            string a = string.IsNullOrEmpty(hsmodel.HANDLE_CONTENT) ? "" : hsmodel.HANDLE_CONTENT;
                            string b = string.IsNullOrEmpty(hsmodel.HANDLE_NAME) ? "" : hsmodel.HANDLE_NAME;
                            string c = !hsmodel.HANDLE_DT.HasValue ? "" : Convert.ToDateTime(hsmodel.HANDLE_DT).ToString("yyyy年MM月dd日");
                            string handle_result = "";
                            if (hsmodel.IS_HANDLE == "1")
                                handle_result = (hsmodel.HANDLE_RESULT == "1") ? "结果:通过" : "结果:驳回";
                            else
                                handle_result = "暂未审批";

                            handleContent +=
                                "<div class='part01'>" +
                                "  <div  class='part11' id = 'seal_" + hsmodel.SID + "'> &nbsp; &nbsp;" + handle_result + "; " + a + " </div>" +
                                "  <div  class='part12' ><div class='part13'  id = '" + hsmodel.SID + "'>" + hsmodel.DEPT_NAME +
                                "   </br>" + b + "&nbsp;&nbsp;</br>" + c + "&nbsp;</div>" +
                                "</div></div></div>";
                        }
                        mainModel.HANDLE_CONTENT_3 = handleContent;
                        mainModel.MAIN_INFO_STATUS_CODE = "";
                    }

                }

                #endregion

                #region 项目评价
                if (mainModel.APPLY_ITEM_TYPE == "评审类" )
                { mainModel.IS_SHOW_4 = "1"; }
                POLICY_STATUS_CHANGE status_change_model_4 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.项目评价).FirstOrDefault();
                if (status_change_model_4 != null)
                {
                    string handleContent = "";
                    if (status_change_model_4.IS_HANDLE == "0" &&
                        status_change_model_4.DEPT_CODE == oc.CurrentUser.DEPT_CODE)
                    {
                        string appply_result =
                            "<input name='radio_approveresult' style='width: 15px;' type = 'radio'  value = '1' />通过" +
                            "<input name = 'radio_approveresult' style = 'width: 15px;' type = 'radio'  value = '0' />驳回";
                        //disabled
                        handleContent +=
                                "<div class='part00'>" +
                                 "    <div  class='part11' id = 'seal_" + status_change_model_4.SID + "'>" +
                                 "       <div class='part14'> <textarea class='part15' maxlength='300' id='textarea_handle_content'></textarea>" +
                                 "        <br>" + appply_result +
                                 "    </div></div>" +
                                 "    <div  class='part12' >" +
                                 "        <div class='part13'  id = '" + status_change_model_4.SID + "'>" + status_change_model_4.DEPT_NAME + "</br>" +
                                 "        负责人:" + oc.CurrentUser.ZSNAME + "<br/>" +
                                        Convert.ToDateTime(DateTime.Now).ToString("yyyy年MM月dd日") +
                                 "        <br/><input type='button' value='审批' id='seal_button' name ='seal_button' onclick=\"seal('" + status_change_model_4.SID + "','seal_" + status_change_model_4.SID + "')\" />" +
                                 "</div></div></div> ";
                        mainModel.MAIN_INFO_STATUS_CODE = status_change_model_4.STATUS_CODE;
                    }
                    else if (status_change_model_4.IS_HANDLE == "1")
                    {
                        string a = string.IsNullOrEmpty(status_change_model_4.HANDLE_CONTENT) ? "" : status_change_model_4.HANDLE_CONTENT;
                        string b = string.IsNullOrEmpty(status_change_model_4.HANDLE_NAME) ? "" : status_change_model_4.HANDLE_NAME;
                        string c = !status_change_model_4.HANDLE_DT.HasValue ? "" : Convert.ToDateTime(status_change_model_4.HANDLE_DT).ToString("yyyy年MM月dd日");
                        string handle_result = (status_change_model_4.HANDLE_RESULT == "1") ? "通过" : "驳回";
                        handleContent +=
                            "<div class='part00'>" +
                            "  <div  class='part11' id = 'seal_" + status_change_model_4.SID + "'> &nbsp; &nbsp;" + "结果: " + handle_result + "; " + a + " </div>" +
                            "  <div  class='part12' ><div class='part13'  id = '" + status_change_model_4.SID + "'>" + status_change_model_4.DEPT_NAME +
                            "   </br>" + b + "&nbsp;&nbsp;</br>" + c + "&nbsp;</div>" +
                            "</div></div></div>";
                    }
                    mainModel.HANDLE_CONTENT_4 = handleContent;
                }

                #endregion

                #region 分管领导审批

                POLICY_STATUS_CHANGE status_change_model_5 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.分管领导审批).FirstOrDefault();
                if (status_change_model_5 != null)
                {
                    string handleContent = "";
                    if (status_change_model_5.IS_HANDLE == "0" &&
                        status_change_model_5.DEPT_CODE == oc.CurrentUser.DEPT_CODE)
                    {
                        string appply_result =
                            "<input name='radio_approveresult' style='width: 15px;' type = 'radio'  value = '1' />通过" +
                            "<input name = 'radio_approveresult' style = 'width: 15px;' type = 'radio'  value = '0' />驳回";
                        //disabled
                        handleContent +=
                                "<div class='part00'>" +
                                 "    <div  class='part11' id = 'seal_" + status_change_model_5.SID + "'>" +
                                 "       <div class='part14'> <textarea class='part15' maxlength='300' id='textarea_handle_content'></textarea>" +
                                 "        <br>" + appply_result +
                                 "    </div></div>" +
                                 "    <div  class='part12' >" +
                                 "        <div class='part13'  id = '" + status_change_model_5.SID + "'>" + status_change_model_5.DEPT_NAME + "</br>" +
                                 "        负责人:" + oc.CurrentUser.ZSNAME + "<br/>" +
                                        Convert.ToDateTime(DateTime.Now).ToString("yyyy年MM月dd日") +
                                 "        <br/><input type='button' value='审批' id='seal_button' name ='seal_button' onclick=\"seal('" + status_change_model_5.SID + "','seal_" + status_change_model_5.SID + "')\" />" +
                                 "</div></div></div> ";

                        mainModel.MAIN_INFO_STATUS_CODE = status_change_model_5.STATUS_CODE;
                    }
                    else if (status_change_model_5.IS_HANDLE == "1")
                    {
                        string a = string.IsNullOrEmpty(status_change_model_5.HANDLE_CONTENT) ? "" : status_change_model_5.HANDLE_CONTENT;
                        string b = string.IsNullOrEmpty(status_change_model_5.HANDLE_NAME) ? "" : status_change_model_5.HANDLE_NAME;
                        string c = !status_change_model_5.HANDLE_DT.HasValue ? "" : Convert.ToDateTime(status_change_model_5.HANDLE_DT).ToString("yyyy年MM月dd日");
                        string handle_result = (status_change_model_5.HANDLE_RESULT == "1") ? "通过" : "驳回";
                        handleContent +=
                            "<div class='part00'>" +
                            "  <div  class='part11' id = 'seal_" + status_change_model_5.SID + "'> &nbsp; &nbsp;" + "结果: " + handle_result + "; " + a + " </div>" +
                            "  <div  class='part12' ><div class='part13'  id = '" + status_change_model_5.SID + "'>" + status_change_model_5.DEPT_NAME +
                            "   </br>" + b + "&nbsp;&nbsp;</br>" + c + "&nbsp;</div>" +
                            "</div></div></div>";
                    }
                    mainModel.HANDLE_CONTENT_5 = handleContent;
                }

                #endregion

                #region 事项公示

                POLICY_STATUS_CHANGE status_change_model_6 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.事项公示).FirstOrDefault();
                if (status_change_model_6 != null)
                {
                    string handleContent = "";
                    if (status_change_model_6.IS_HANDLE == "1")
                    {
                        handleContent = "<div style='width: 100%; '>" + "公示日期：" + Convert.ToDateTime(status_change_model_6.POLICY_MAIN_INFO.POLICITY_BEGIN_DT).ToString("yyyy-MM-dd") +
                            "至" + Convert.ToDateTime(status_change_model_6.POLICY_MAIN_INFO.POLICITY_END_DT).ToString("yyyy-MM-dd") +
                            ";状态：" + status_change_model_6.POLICY_MAIN_INFO.POLICITY_STATUS +
                            ";备注：" + status_change_model_6.POLICY_MAIN_INFO.POLICITY_STOP_REASON + "</div>";
                    }
                    mainModel.HANDLE_CONTENT_6 = handleContent;
                }

                #endregion

                #region 财政拨款

                POLICY_STATUS_CHANGE status_change_model_7 = mModel.POLICY_STATUS_CHANGE
                    .Where(o => o.STATUS_CODE == Constant.POLICY_STATUS.财政拨付).FirstOrDefault();
                if (status_change_model_7 != null)
                {
                    string handleContent = "";
                    if (status_change_model_7.IS_HANDLE == "0" &&
                        status_change_model_7.DEPT_CODE == oc.CurrentUser.DEPT_CODE)
                    {
                        string appply_result =
                             "<input name='radio_approveresult' style='width: 15px;' checked type = 'radio'  value = '1' />通过" ;
                        //disabled
                        handleContent +=
                                "<div class='part00'>" +
                                 "    <div  class='part11' id = 'seal_" + status_change_model_7.SID + "'>" +
                                 "       <div class='part14'> <textarea class='part15' maxlength='300' id='textarea_handle_content'></textarea>" +
                                 "        <br>" + appply_result +
                                 "    </div></div>" +
                                 "    <div  class='part12' >" +
                                 "        <div class='part13'  id = '" + status_change_model_7.SID + "'>" + status_change_model_7.DEPT_NAME + "</br>" +
                                 "        负责人:" + oc.CurrentUser.ZSNAME + "<br/>" +
                                        Convert.ToDateTime(DateTime.Now).ToString("yyyy年MM月dd日") +
                                 "        <br/><input type='button' value='审批' id='seal_button' name ='seal_button' onclick=\"seal('" + status_change_model_7.SID + "','seal_" + status_change_model_7.SID + "')\" />" +
                                 "</div></div></div> ";

                        mainModel.MAIN_INFO_STATUS_CODE = status_change_model_7.STATUS_CODE;
                    }
                    else if (status_change_model_7.IS_HANDLE == "1")
                    {
                        string a = string.IsNullOrEmpty(status_change_model_7.HANDLE_CONTENT) ? "" : status_change_model_7.HANDLE_CONTENT;
                        string b = string.IsNullOrEmpty(status_change_model_7.HANDLE_NAME) ? "" : status_change_model_7.HANDLE_NAME;
                        string c = !status_change_model_7.HANDLE_DT.HasValue ? "" : Convert.ToDateTime(status_change_model_7.HANDLE_DT).ToString("yyyy年MM月dd日");
                        string handle_result = (status_change_model_7.HANDLE_RESULT == "1") ? "通过" : "驳回";
                        handleContent +=
                            "<div class='part00'>" +
                            "  <div  class='part11' id = 'seal_" + status_change_model_7.SID + "'> &nbsp; &nbsp;" + "结果: " + handle_result + "; " + a + " </div>" +
                            "  <div  class='part12' ><div class='part13'  id = '" + status_change_model_7.SID + "'>" + status_change_model_7.DEPT_NAME +
                            "   </br>" + b + "&nbsp;&nbsp;</br>" + c + "&nbsp;</div>" +
                            "</div></div></div>";
                    }
                    mainModel.HANDLE_CONTENT_7 = handleContent;
                }

                #endregion
            }
            //todo:是否启用电子签章
            string seal_flag = ConfigurationManager.AppSettings["SEAL_FLAG"];
            BASE_CONFIG configModel = bs.Entities<BASE_CONFIG>().FirstOrDefault(o => o.CONFIG_TYPE == "DZQZ");
            if (configModel != null)
            {
                seal_flag = configModel.CONFIG_VALUE;
            }
            mainModel.SEAL_FLAG = seal_flag;
            return mainModel;
        }
        #endregion

        #region 查看企业申请信息
        public AjaxMsgModel GetApplyCorpInfo(string id)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "录入失败！";
            try
            {
                VIEW_POLICY_MAIN_INFO mainModel = new VIEW_POLICY_MAIN_INFO();
                POLICY_MAIN_INFO mModel =
                    bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID.Equals(id));
                if (mModel != null)
                {
                    mainModel = new VIEW_POLICY_MAIN_INFO
                    {
                        SID = mModel.SID,
                        APPLY_NUMBER = mModel.APPLY_NUMBER,
                        CORP_NAME = mModel.CORP_NAME,
                        SOCIAL_CREDIT_CODE = mModel.SOCIAL_CREDIT_CODE,
                        REGISTERED_ADDRESS = mModel.REGISTERED_ADDRESS,
                        LEGAL_PERSON = mModel.LEGAL_PERSON,
                        LEGAL_PERSON_PHONE = mModel.LEGAL_PERSON_PHONE,
                        OPERATOR = mModel.OPERATOR,
                        OPERATOR_PHONE = mModel.OPERATOR_PHONE,
                        OPERATOR_ID_NO = mModel.OPERATOR_ID_NO,
                        EMIAL = mModel.EMIAL,
                        REGISTERED_CAPITAL = mModel.REGISTERED_CAPITAL,
                        APPLY_ITEM_TYPE = mModel.APPLY_ITEM_TYPE,
                        APPLY_ITEM_NAME = mModel.APPLY_ITEM_NAME,
                        APPLY_MONEY_WORDS = mModel.APPLY_MONEY_WORDS,
                        APPLY_MONEY_NUMBER = mModel.APPLY_MONEY_NUMBER,
                        BANK_NAME = mModel.BANK_NAME,
                        BANK_ACOUNT = mModel.BANK_ACOUNT,
                        CORP_PROMISE = mModel.CORP_PROMISE,
                        VAT_NO = mModel.VAT_NO
                    };
                    List<POLICY_APPLY_FILE> fileList =
                        bs.Entities<POLICY_APPLY_FILE>().Where(o => o.MAIN_SID == mModel.SID).ToList();
                    foreach (var item in fileList)
                    {
                        item.FILE_PATH = ConfigurationManager.AppSettings["COMPANY_PATH"] + item.FILE_PATH;
                    }
                    mainModel.fileList = fileList;
                    amm.Statu = AjaxStatu.ok;
                }

                amm.Data = mainModel;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
            return amm;
        } 
        #endregion

        #region 事项公示
        public AjaxMsgModel SelectPolicity(List<string> id_list)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "公示失败！";
            try
            {

                var p_model_list =
                    bs.Entities<POLICY_MAIN_INFO>().Where(o => id_list.Contains(o.SID)).ToList();
                var status_change_list = bs.Entities<POLICY_STATUS_CHANGE>().Where(o => id_list.Contains(o.POLICY_SID) && o.STATUS_CODE == Constant.POLICY_STATUS.事项公示 && o.IS_HANDLE == "0").ToList();
                //todo:公示时间--需要做配置，数据表保存 
                int intervalday = 5;
                BASE_CONFIG configModel = bs.Entities<BASE_CONFIG>().FirstOrDefault(o => o.CONFIG_TYPE == "SXGS");
                if (configModel != null)
                {
                    intervalday = Convert.ToInt16(configModel.CONFIG_VALUE);
                }
                DateTime dt = DateTime.Now;
                foreach (var item in p_model_list)
                {
                    item.POLICITY_STATUS = "正常";
                    item.POLICITY_BEGIN_DT = Convert.ToDateTime(dt.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
                    item.POLICITY_END_DT = Convert.ToDateTime(dt.AddDays(intervalday).ToString("yyyy-MM-dd 17:30:00"));
                }
                foreach (var item in status_change_list)
                {
                    item.IS_HANDLE = "1";
                    item.HANDLE_DT = dt;
                    item.HANDLE_SID = oc.CurrentUser.USER_NAME;
                    item.HANDLE_NAME = oc.CurrentUser.ZSNAME;
                }

                int index = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var item in status_change_list)
                    {
                        index += bs.UpdateEntity(item, new string[] { "IS_HANDLE", "HANDLE_DT", "HANDLE_SID", "HANDLE_NAME" });
                    }

                    foreach (var item in p_model_list)
                    {
                        index += bs.UpdateEntity(item, new string[] { "POLICITY_STATUS", "POLICITY_BEGIN_DT", "POLICITY_END_DT" });
                    }


                    ts.Complete();
                }

                if (index >= 2)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "公示成功！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;//这一行要加，因为验证时已更改
                    amm.Msg = "公示失败！";
                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
            return amm;
        }
        #endregion

        #region 归档备案
        public AjaxMsgModel SelectFile(List<string> id_list)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "备案归档失败！";
            try
            {

                var p_model_list =
                    bs.Entities<POLICY_MAIN_INFO>().Where(o => id_list.Contains(o.SID)).ToList();
                var status_change_list = bs.Entities<POLICY_STATUS_CHANGE>().Where(o => id_list.Contains(o.POLICY_SID)
                    && o.STATUS_CODE == Constant.POLICY_STATUS.备案归档 && o.IS_HANDLE == "0").ToList();

                DateTime dt = DateTime.Now;
                foreach (var item in p_model_list)
                {
                    item.IS_FILE = "是";
                    item.FILE_DT = dt;
                    item.STATUS_CODE = Constant.POLICY_STATUS.备案归档;
                    item.STATUS_NAME = "备案归档";
                }
                foreach (var item in status_change_list)
                {
                    item.IS_HANDLE = "1";
                    item.HANDLE_DT = dt;
                    item.HANDLE_SID = oc.CurrentUser.USER_NAME;
                    item.HANDLE_NAME = oc.CurrentUser.ZSNAME;
                }

                int index = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var item in status_change_list)
                    {
                        index += bs.UpdateEntity(item, new string[] { "IS_HANDLE", "HANDLE_DT", "HANDLE_SID", "HANDLE_NAME" });
                    }

                    foreach (var item in p_model_list)
                    {
                        index += bs.UpdateEntity(item, new string[] { "IS_FILE", "FILE_DT", "STATUS_CODE", "STATUS_NAME" });
                    }


                    ts.Complete();
                }

                if (index >= 2)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "备案归档成功！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;//这一行要加，因为验证时已更改
                    amm.Msg = "备案归档失败！";
                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
            return amm;
        }
        #endregion

        #region 保存公示状态
        public AjaxMsgModel SavePolicityStatus(POLICY_MAIN_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "公示操作失败！";
            try
            {

                POLICY_MAIN_INFO model =
                    bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID.Equals(data.SID));
                if (model == null)
                {
                    amm.Statu = AjaxStatu.err;//这一行要加，因为验证时已更改
                    amm.Msg = "公示操作失败！";
                    return amm;
                }
                int index = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    model.UPDATE_DT = DateTime.Now;
                    model.UPDATE_BY = oc.CurrentUser.ZSNAME;
                    model.POLICITY_STATUS = data.POLICITY_STATUS;

                    if (model.POLICITY_STATUS == "继续")
                    {
                        model.POLICITY_BEGIN_DT = DateTime.Now;
                        model.POLICITY_END_DT = DateTime.Now.AddDays(5);
                        model.POLICITY_STOP_REASON = model.POLICITY_STOP_REASON + model.POLICITY_STATUS + ",时间:" +
                                                    Convert.ToDateTime(model.UPDATE_DT).ToString("yyyy-MM-dd HH:mm") +
                                                    ";原因:" + data.POLICITY_STOP_REASON + "; ";
                        model.POLICITY_STATUS = "正常";
                    }
                    else if (model.POLICITY_STATUS == "终止")
                    {
                        model.POLICITY_STOP_REASON = model.POLICITY_STOP_REASON + model.POLICITY_STATUS + ",时间:" +
                                                     Convert.ToDateTime(model.UPDATE_DT).ToString("yyyy-MM-dd HH:mm") +
                                                     ";原因:" + data.POLICITY_STOP_REASON + "; ";
                    }
                    else if (model.POLICITY_STATUS == "testpublicity")
                    {
                        model.POLICITY_STATUS = "公示结束";
                        model.POLICITY_BEGIN_DT = DateTime.Now.AddDays(-1);
                        model.STATUS_CODE = Constant.POLICY_STATUS.企业请拨;
                        model.STATUS_NAME = "企业请拨";
                    }


                    index += bs.UpdateEntity(model,
                        new string[] { "POLICITY_STATUS", "POLICITY_BEGIN_DT", "POLICITY_END_DT", "POLICITY_STOP_REASON", "UPDATE_DT", "UPDATE_BY", "STATUS_CODE", "STATUS_NAME" });

                    ts.Complete();
                }

                if (index > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "公示操作成功！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;//这一行要加，因为验证时已更改
                    amm.Msg = "公示操作失败！";
                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
            return amm;
        }
        #endregion

        #region 保存驳回状态
        public AjaxMsgModel SaveReject(POLICY_STATUS_CHANGE data)
        {
            //todo:需要接入短信接口
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "驳回处理失败！";
            try
            {
                var statusModel = bs.Entities<POLICY_STATUS_CHANGE>().FirstOrDefault(o => o.SID == data.SID);
                POLICY_MAIN_INFO mainModel = null;
                if (statusModel != null)
                {
                    mainModel = bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID == statusModel.POLICY_SID);
                    if (mainModel != null)
                    {
                        mainModel.REJECT_REASON = data.HANDLE_CONTENT;
                        mainModel.APPLY_STATUS = "驳回";
                        statusModel.IS_HANDLE = "1";
                        statusModel.HANDLE_DT = DateTime.Now;
                        statusModel.HANDLE_SID = oc.CurrentUser.USER_NAME;
                        statusModel.HANDLE_NAME = oc.CurrentUser.ZSNAME;
                        statusModel.HANDLE_CONTENT = data.HANDLE_CONTENT;
                    }
                }

                int index = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    index += bs.UpdateEntity(statusModel,
                        new string[] {"IS_HANDLE", "HANDLE_DT", "HANDLE_SID", "HANDLE_NAME", "HANDLE_CONTENT" });
                    index += bs.UpdateEntity(mainModel,
                       new string[] { "REJECT_REASON", "APPLY_STATUS" });
                    ts.Complete();
                }

                if (index >= 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "驳回处理成功！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;//这一行要加，因为验证时已更改
                    amm.Msg = "驳回处理失败！";
                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
            return amm;
        }
        #endregion

        #region 我的桌面----相关信息

        public ResultData GetPolicyStatisticsInfo()
        {
            ResultData resultData = new ResultData();
            try
            {
                List<BASE_STATUS_DIC> statusdicList = bs.Entities<BASE_STATUS_DIC>().ToList();
                //根据所属角色查菜单
                List<string> userrole =
                    bs.Entities<SYS_USER_ROLE_MAP>().Where(o => o.USER_NAME == oc.CurrentUser.USER_NAME).Select(o=>o.ROLE_ID).ToList();
                if (userrole.Count > 0 )
                {
                    List<string> role_manu1 =
                        bs.Entities<SYS_ROLE_MENU_MAP>()
                            .Where(o => userrole.Contains(o.ROLE_ID))
                            .Select(o => o.MENU_ID)
                            .ToList();

                    

                    List<POLICY_STATUS_STATISTICS_DATA> statisticsList = new List<POLICY_STATUS_STATISTICS_DATA>();
                    var sysmenuList = bs.Entities<SYS_MENU>().Where(o => o.PARENT_ID.Equals("020101") && role_manu1.Contains(o.MENU_ID)).ToList();
                    foreach (var item in statusdicList)
                    {
                        POLICY_STATUS_STATISTICS_DATA sdata = new POLICY_STATUS_STATISTICS_DATA();
                        SYS_MENU menuModel =
                            sysmenuList.FirstOrDefault(o => o.ACTION.ToUpper().Replace("INDEX/", "").Equals(item.S_CODE));
                        if (menuModel != null)
                        {
                            sdata.STATUS_NAME = menuModel.MENU_NAME;
                           

                            #region 计算数字

                            string count_str = "";
                            var statusList =
                                bs.Entities<POLICY_STATUS_CHANGE>()
                                    .Where(
                                        o =>
                                            o.DEPT_CODE == oc.CurrentUser.DEPT_CODE && o.STATUS_CODE == item.S_CODE &&
                                            o.IS_HANDLE == "0")
                                    .ToList();
                            int handle_count = 0;

                            if (item.S_CODE == Constant.POLICY_STATUS.注册审批)
                            {
                                handle_count =  bs.Entities<CORPORATION_BASE_INFO>().Where(o => string.IsNullOrEmpty(o.APPLY_RESULT)).ToList().Count();
                            }
                            else
                                handle_count = statusList.Count();

                            if (handle_count < 10 && handle_count > 0)
                            {
                                count_str = handle_count.ToString();
                                sdata.STATUS_URL = "/" + menuModel.AREA + "/" + menuModel.CONTROLLER + "/" +
                                              menuModel.ACTION;
                            }
                            else if (handle_count >= 10)
                            {
                                count_str = "10";
                                sdata.STATUS_URL = "/" + menuModel.AREA + "/" + menuModel.CONTROLLER + "/" +
                                                   menuModel.ACTION;
                            }
                            else
                            {
                                sdata.STATUS_NAME = menuModel.MENU_NAME + ".";
                                sdata.STATUS_URL = "/" + menuModel.AREA + "/" + menuModel.CONTROLLER + "_HIS/" +
                                                   menuModel.ACTION;
                            }

                            #endregion

                            sdata.STATUS_CODE = item.S_CODE;
                            sdata.ACCEPT_COUNT = count_str;
                            statisticsList.Add(sdata);
                        }
                    }

                    resultData.statusstatisticsList = statisticsList;
                }
            }
            catch (Exception ex)
            {
                RecordLog.RecordException(ex);
            }
            return resultData;
        }
        #endregion

        #region 定时计算事项公示是否结束
        public void Task()
        {
            /*
              * 1、找出所有事项公示状态正常，且当前申请状态为事项公示状态，且当前日期大于事项公示结束日期
              * 2、进行企业申请下一状态,企业请拨，并将事项公示状态改为：公示结束。
              * 3、新增一条状态变化记录，企业请拨状态
              * 4、企业进行请拨操作后，更新企业请拨状态数据、日期等信息
              */

            RecordLog.RecordInfo("事项公示开始计算：" + DateTime.Now.ToString());
            try
            {
                List<POLICY_MAIN_INFO> taskList = bs.Entities<POLICY_MAIN_INFO>().
                Where(o => o.STATUS_CODE.Equals(Constant.POLICY_STATUS.事项公示) &&
                o.POLICITY_STATUS.Equals("正常") && o.POLICITY_END_DT < DateTime.Now
                ).ToList();

                List<POLICY_STATUS_CHANGE> status_changeList = new List<POLICY_STATUS_CHANGE>();
                if (taskList.Count > 0)
                {
                    RecordLog.RecordInfo("事项公示开始计算：" + DateTime.Now.ToString());
                    foreach (var item in taskList)
                    {
                        item.STATUS_CODE = Constant.POLICY_STATUS.企业请拨;
                        item.STATUS_NAME = "企业请拨";
                        item.POLICITY_STATUS = "公示结束";
                        #region 事项公示

                        POLICY_STATUS_CHANGE nextstatusModel = new POLICY_STATUS_CHANGE();
                        nextstatusModel.SID = Guid.NewGuid().ToString();
                        nextstatusModel.POLICY_SID = item.SID;
                        nextstatusModel.ITEM_TYPE = item.APPLY_ITEM_TYPE;
                        nextstatusModel.STATUS_CODE = Constant.POLICY_STATUS.企业请拨;
                        nextstatusModel.STATUS_NAME = "企业请拨";
                        nextstatusModel.CREATE_DT = DateTime.Now;
                        nextstatusModel.IS_HANDLE = "0";
                        nextstatusModel.IS_CALCULATE = "0";
                        nextstatusModel.TIME_LIMIT = 0;
                        nextstatusModel.DEPT_CODE = "0000-0000";
                        nextstatusModel.DEPT_NAME = "企业经办人处理";
                        status_changeList.Add(nextstatusModel);
                        #endregion
                    }

                    int index = 0;
                    using (TransactionScope ts = new TransactionScope())
                    {

                        foreach (var item in taskList)
                        {
                            index += bs.UpdateEntity(item, new string[] { "STATUS_CODE", "STATUS_NAME", "POLICITY_STATUS" });
                        }
                        index += bs.AddListEntity(status_changeList);

                        ts.Complete();
                    }

                }
            }
            catch (Exception ex)
            {
                RecordLog.RecordError("事项公示出错：" + ex.ToString());
            }
            //todo:批量更新问题  记录日志  短信发送日志
        }
        #endregion
    }
    public class ResultData
    {
        public ResultData() { }
        //public POLICY_STATUS_STATISTICS_DATA statusDataInfo { set; get; }
        public List<POLICY_STATUS_STATISTICS_DATA> statusstatisticsList { set; get; }
        
    }
    public class POLICY_STATUS_STATISTICS_DATA
    {
        public string STATUS_CODE { get; set; }
        public string STATUS_NAME { get; set; }
        public string DEPT_CODE { get; set; }
        public string STATUS_URL { get; set; }
        public string ACCEPT_COUNT { set; get; }
        //public List<CHART_DATA> DATA_LIST { get; set; }
    }
}