using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Common;
using Model;
using Model.ModelExt;
using System.Data.Entity;
using System.Transactions;
using SxShWeb.Areas.ViewModels;
using AutoMapper;
using System.Threading;
using System.Diagnostics;

namespace SxShWeb.Areas.Models
{
    public class Model_JOB_WARNING
    {
        #region 操作上下文的静态变量
        static OperContext oc = OperContext.CurrentContext;
        #endregion

        
        #region 新报警
        public static bool WARNFLAG = false;//计算报警
        static masService.FtsSoapClient sendSMS = new masService.FtsSoapClient();//启用短信报警发送
        public static void Task()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            WARNFLAG = true;//开始计算报警
            try
            {

                #region 获取计划
                Dictionary<string, int> JOB_AA = new Dictionary<string, int>();//正常计划手机号集合
                List<JOB_PlanWainTime> OverPlan = new List<JOB_PlanWainTime>();

                string sysDept = Model_JOB_PLAN.GetSysDept();
                if (sysDept == "16" || sysDept == "13" || sysDept == "07")//上海合肥
                {
                    GetXiAnJobPlan(OverPlan, 60, -60);//西安//前后60分钟计划
                }
                else if (sysDept == "08")
                {
                    GetJobPlan(OverPlan, 60, -120);//广州惠州,前60后120分的计划
                }
                else
                {
                    GetJobPlan(OverPlan, 60, -60);//前后60分钟计划
                }

                RecordLog.RecordError("报警计算数量：" + System.DateTime.Now + ":" + OverPlan.Count.ToString());

                DateTime dtNow = DateTime.Now;
                DateTime dt1 = dtNow.AddMinutes(-10);
                DateTime dt2 = dtNow.AddMinutes(10);

                Decimal WcError_Mile = Model_JOB_PLAN.GetAllowableErrorMile();// 0.05M;//作业计划允许误差为50米-->
                Decimal douJOB_OverRangeBeforeMile = 1M;//作业计划超范围报警里程 前 1千米-->
                Decimal douJOB_OverRangeEndMile = 1M;//作业计划超范围报警里程 后 1千米--> 
                #endregion


                #region 计算报警
                foreach (JOB_PlanWainTime overplan in OverPlan)
                {
                    int Bj_Minute = 60;//作业前后60分钟内报警

                    int StartBj_Minute = -30;//30分钟准备时间内不报警
                    int EndBj_Minute = 0;//结束后0分钟内不报警

                    int Cd_Minute = -30;//迟到报警时间，负30分表示提前30分没到

                    #region 局参数配置
                    if (overplan.DEPTCODE.Substring(0, 2) == "03" || overplan.DEPTCODE.Substring(0, 2) == "04")//如果是北京或呼和
                    {
                        StartBj_Minute = 0;//无准备时间，早到报警
                    }
                    if (overplan.DEPTCODE.Substring(0, 4) == "0802")//如果是广州
                    {
                        Bj_Minute = 120;//作业前后120分钟内报警
                        StartBj_Minute = -120;//120分钟准备时间内不报警，即作业开始前不报警
                        EndBj_Minute = 60;//结束后60分钟内不报警
                        Cd_Minute = 30;//正30分，计划开始后30分没到
                    }
                    if (overplan.DEPTCODE.Substring(0, 4) == "0702")//如果是合肥
                    {
                        StartBj_Minute = -20;//20分钟准备时间内不报警
                        EndBj_Minute = 20;//结束后20分钟内不报警
                        Cd_Minute = 20;//正20分，计划开始后20分没到
                    }
                    if (overplan.DEPTCODE.Substring(0, 2) == "16")//如果是西安
                    {
                        StartBj_Minute = -40;//40分钟准备时间内不报警
                        EndBj_Minute = 40;//结束后40分钟内不报警
                    }

                    if (overplan.DEPTCODE.Substring(0, 2) == "13")//如果是南昌
                    {
                        Bj_Minute = 60;//作业前后60分钟内报警
                        StartBj_Minute = -60;//60分钟准备时间内不报警，即作业开始前不报警
                        EndBj_Minute = 20;//结束后20分钟内不报警
                        Cd_Minute = 20;//正20分，计划开始后20分没到

                        if (overplan.TYPE == "11" || overplan.TYPE == "13" || overplan.TYPE == "16")
                        {
                            //普速点内作业、普速临时要点：按实际时间报警，无实际时间，各种报警均不报。
                            if (string.IsNullOrEmpty(overplan.WORK_TIME))//如果没上传实际作业时间
                            {
                                if (overplan.PLANTIME_END <= dtNow)//“无实际作业时间”报警，按计划结束时间报警。
                                {
                                    AddPlanWain(overplan, "无实际作业时间");
                                }
                                continue;
                            }
                            
                        }
                    }

                    if (overplan.DEPTCODE.Substring(0, 2) == "18")//如果是太原
                    {
                        EndBj_Minute = 30;//结束后30分钟内不报警

                        //太原专用未上道
                        if (dtNow > overplan.PLANTIME_BEGIN.Value.AddMinutes(30))//施工开始后30分钟算未上道,与其他局的迟到不是一个概念
                        {
                            if (oc.BllSession.IT_SGJHWARNService.Entities.Where(p => p.施工ID.Equals(overplan.ID) && p.手机号.Equals(overplan.PHONE) && p.超范围类型 != "迟到").Count() == 0)
                            {
                                AddPlanWain(overplan, "未上道");
                            }
                        }
                    }

                    if (overplan.DEPTCODE.Substring(0, 4) == "1203")//如果是哈密
                    {
                        StartBj_Minute = -40;//40分钟准备时间内不报警，即作业开始前不报警
                        Cd_Minute = -40;//迟到报警时间，负40分表示提前40分没到

                        int Pic_StartMinute = 40;//如果40-100分钟无图片报警
                        int Pic_EndMinute = 100;//如果40-100分钟无图片报警
                        //计算驻站图片报警
                        if (!string.IsNullOrEmpty(overplan.PHONE_LIST) && overplan.PHONE_LIST.Split(',')[0] != "")//如果有驻站手机号
                        {
                            if (dtNow >= overplan.PLANTIME_BEGIN.Value.AddMinutes(-Pic_StartMinute) && dtNow <= overplan.PLANTIME_BEGIN.Value.AddMinutes(-Pic_StartMinute + 5))//5分钟内计算，40-100分钟无图片报警
                            {
                                DateTime minTime = overplan.PLANTIME_BEGIN.Value.AddMinutes(-Pic_EndMinute);
                                DateTime maxTime = overplan.PLANTIME_BEGIN.Value.AddMinutes(-Pic_StartMinute);
                                string zzphone = overplan.PHONE_LIST.Split(',')[0];//驻站手机号

                                bool isPic = oc.BllSession.IT_MOBILEFILEService.Entities.Where(u => u.手机号 == zzphone && u.时间 >= minTime && u.时间 <= maxTime).Count() == 0;

                                if (isPic)//如果40-100分钟无图片报警
                                {
                                    if (oc.BllSession.IT_SGJHWARNService.Entities.Where(p => p.施工ID.Equals(overplan.ID) && p.手机号.Equals(overplan.PHONE) && p.超范围类型 == "未按时驻站").Count() == 0)
                                    {
                                        AddPlanWain(overplan, "未按时驻站");
                                    }
                                }
                            }
                        }
                    }
                    //判断现在是否为午休时间
                    if (!string.IsNullOrEmpty(overplan.REST_TIME))
                    {
                        try
                        {
                            DateTime startTime = Convert.ToDateTime(overplan.REST_TIME.Replace("－", "-").Split('-')[0].Trim());
                            DateTime endTime = Convert.ToDateTime(overplan.REST_TIME.Replace("－", "-").Split('-')[1].Trim());

                            if (dtNow.Hour >= startTime.Hour && dtNow.Minute >= startTime.Minute
                                && dtNow.Hour <= endTime.Hour && dtNow.Minute <= endTime.Minute)
                            {
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            RecordLog.RecordError("午休报警：" + overplan.REST_TIME);
                        }
                    }
                    #endregion

                    T_MOBILELASTPOS lastpos = oc.BllSession.IT_MOBILELASTPOSService.Entities
                                                        .Where(p => p.手机号.Equals(overplan.PHONE)
                                                                && p.时间 >= dt1 && p.时间 <= dt2
                                                                && p.线名.Equals(overplan.LINE_NAME)
                                                                && p.里程 >= overplan.MILEAGE_BEGIN - douJOB_OverRangeBeforeMile
                                                                && p.里程 <= overplan.MILEAGE_END + douJOB_OverRangeEndMile
                        //&& (p.方式.Equals("GPS")) || (p.方式.Equals("MBS"))
                                                        ).OrderByDescending(p => p.时间).FirstOrDefault();

                    #region 负责人定位报警
                    if (lastpos != null)
                    {
                        double pos = Convert.ToDouble(lastpos.里程);
                        DateTime dtpos = lastpos.时间;
                        bool overRang = false;
                        bool overTime = false;
                        overplan.WAINTIME = dtpos;//报警时间

                        //报警时负责人位置
                        overplan.火星经度 = lastpos.火星经度;
                        overplan.火星纬度 = lastpos.火星纬度;
                        overplan.线名 = lastpos.线名;
                        overplan.行别 = lastpos.行别;
                        overplan.里程 = lastpos.里程;

                        #region 超时间超范围
                        if (overplan.PLANTIME_BEGIN.Value.AddMinutes(-Bj_Minute) <= dtpos &&
                               dtpos <= overplan.PLANTIME_END.Value.AddMinutes(Bj_Minute))//在报警时间段
                        {
                            //作业结束后报警
                            if (overplan.PLANTIME_END.Value.AddMinutes(EndBj_Minute) < dtpos)
                            {
                                overTime = true;
                            }
                            if (overplan.WAIN_TYPE == "1")//早到报警（奎屯不管）
                            {
                                if (dtpos < overplan.PLANTIME_BEGIN.Value.AddMinutes(StartBj_Minute))
                                {
                                    overTime = true;
                                }
                            }


                            //查找里程是否在报警里程范围内
                            if (pos < Convert.ToDouble(overplan.MILEAGE_BEGIN.Value - WcError_Mile) || pos > Convert.ToDouble(overplan.MILEAGE_END.Value + WcError_Mile))
                            {
                                overRang = true;
                                if (overplan.PHONENUM == "驻站负责人")//驻站负责人
                                {
                                    //作业时间开始后-作业结束，无驻站定位数据，或驻站不在车站范围内，报“驻站缺失”。
                                    if (overplan.PLANTIME_BEGIN.Value > dtpos || dtpos > overplan.PLANTIME_END.Value)//
                                    {
                                        overRang = false;
                                    }
                                }
                                else
                                {
                                    if (overplan.PLANTIME_BEGIN.Value.AddMinutes(StartBj_Minute) <= dtpos &&
                                            dtpos <= overplan.PLANTIME_BEGIN.Value)//准备时间N分钟内不报警
                                    {
                                        overRang = false;
                                    }
                                    if (overplan.PLANTIME_END.Value <= dtpos &&
                                            dtpos <= overplan.PLANTIME_END.Value.AddMinutes(EndBj_Minute))//结束后N分钟内不报警
                                    {
                                        overRang = false;
                                    }
                                    if (overplan.DEPTCODE.Substring(0, 2) == "13")//如果是南昌,开始后20分钟内不报超范围
                                    {
                                        if (overplan.PLANTIME_BEGIN.Value.AddMinutes(20) <= dtpos)//开始后20分钟内不报超范围
                                        {
                                            overRang = false;
                                        }
                                    }
                                }
                            }
                            overplan.ERRTYPE = ((overTime ? 1 : 0) + (overRang ? 2 : 0)).ToString();

                            //报警范围内的正常才算正常
                            if (overplan.ERRTYPE == "0" && !JOB_AA.ContainsKey(overplan.PHONE))
                            {
                                JOB_AA.Add(overplan.PHONE, 0);
                            }

                            #region 计算防护过远
                            if (overplan.DEPTCODE.Substring(0, 2) == "13" || overplan.DEPTCODE.Substring(0, 4) == "0702")//如果是南昌或合肥,计算防护过远
                            {
                                if (overplan.PLANTIME_BEGIN.Value.AddMinutes(20) <= dtpos && dtpos <= overplan.PLANTIME_END)//如果进入计划时间//	工地防护员、中间联络员、远端防护员的相关报警，均在作业开始后延长20分钟，再报警。
                                {
                                    if (overplan.PHONENUM == "0")//如果是第一作业负责人
                                    {
                                        if (!string.IsNullOrEmpty(overplan.PHONE_LIST) && overplan.PHONE_LIST.Contains("|"))
                                        {
                                            bool flag = false;
                                            JOB_PLAN_SAFE safe = new JOB_PLAN_SAFE();
                                            safe.SAFE_ID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + overplan.PLAN_ID;
                                            safe.PLAN_ID = overplan.PLAN_ID;
                                            safe.PHONE = lastpos.手机号;
                                            safe.LINE_NAME = lastpos.线名;
                                            safe.DIRECTION = lastpos.行别;
                                            safe.MILEAGE_FZR = lastpos.里程;
                                            safe.TIME_FZR = lastpos.时间;
                                            //负责人|工地防护员|驻站防护员|远端电话号码|中间联络
                                            string[] allphones = overplan.PHONE_LIST.Split('|');//手机

                                            #region 计算工地防护员距离
                                            if (allphones.Length > 1 && allphones[1] != "")
                                            {
                                                string[] fhphones = allphones[1].Split(',');//工地防护员手机
                                                string fhphone = fhphones[0];

                                                T_MOBILELASTPOS fhcpos = oc.BllSession.IT_MOBILELASTPOSService.Entities
                                                    .Where(p => p.手机号.Equals(fhphone)
                                                    && p.时间 >= dt1 && p.时间 <= dt2
                                                    ).OrderByDescending(p => p.时间).FirstOrDefault();

                                                if (fhcpos != null)
                                                {
                                                    flag = true;
                                                    safe.PHONE_SAFEGD = fhphone;
                                                    safe.MILEAGE_SAFEGD = fhcpos.里程;
                                                    safe.TIME_SAFEGD = fhcpos.时间;
                                                    if (overplan.LINE_NAME == fhcpos.线名)
                                                    {
                                                        safe.SAFE_RANGE_LC_GD = Convert.ToDecimal(Math.Abs(Convert.ToDouble(fhcpos.里程) - pos) * 1000);
                                                        //if (safe.SAFE_RANGE_LC_GD > 50)//只记录不报警
                                                        //{
                                                        //    AddPlanWain(overplan, "工地防护过远", fhphone);
                                                        //}
                                                        //里程是否在作业里程范围内
                                                        double fhpos = Convert.ToDouble(fhcpos.里程);
                                                        if (fhpos < Convert.ToDouble(overplan.MILEAGE_BEGIN.Value) - 0.1 || fhpos > Convert.ToDouble(overplan.MILEAGE_END.Value) + 0.1)//延长100米
                                                        {
                                                            AddPlanWain(overplan, "工地防护缺失", fhphone);
                                                        }
                                                    }
                                                    safe.SAFE_RANGE_GD = Convert.ToDecimal(CalcGpsDis.reGpsDis(fhcpos.纬度.Value, fhcpos.经度.Value, lastpos.纬度.Value, lastpos.经度.Value));

                                                }
                                                else
                                                {
                                                    AddPlanWain(overplan, "工地防护缺失", fhphone);
                                                }
                                            }
                                            #endregion

                                            #region 计算中间联络员距离
                                            if (allphones.Length > 4 && allphones[4] != "")
                                            {
                                                string[] fhphones = allphones[4].Split(',');//中间联络员手机
                                                string fhphone = fhphones[0];

                                                T_MOBILELASTPOS fhdpos = oc.BllSession.IT_MOBILELASTPOSService.Entities
                                                    .Where(p => p.手机号.Equals(fhphone)
                                                    && p.时间 >= dt1 && p.时间 <= dt2
                                                    ).OrderByDescending(p => p.时间).FirstOrDefault();

                                                if (fhdpos != null)
                                                {
                                                    flag = true;
                                                    safe.PHONE_SAFEZJ = fhphone;
                                                    safe.MILEAGE_SAFEZJ = fhdpos.里程;
                                                    safe.TIME_SAFEZJ = fhdpos.时间;
                                                    if (overplan.LINE_NAME == fhdpos.线名)
                                                    {
                                                        safe.SAFE_RANGE_LC_ZJ = Convert.ToDecimal(Math.Abs(Convert.ToDouble(fhdpos.里程) - pos) * 1000);
                                                        //里程是否在作业里程范围内±1km内
                                                        double fhpos = Convert.ToDouble(fhdpos.里程);
                                                        if (fhpos < Convert.ToDouble(overplan.MILEAGE_BEGIN.Value) - 1 || fhpos > Convert.ToDouble(overplan.MILEAGE_END.Value) + 1)
                                                        {
                                                            AddPlanWain(overplan, "中间联络缺失", fhphone);
                                                        }
                                                    }
                                                    safe.SAFE_RANGE_ZJ = Convert.ToDecimal(CalcGpsDis.reGpsDis(fhdpos.纬度.Value, fhdpos.经度.Value, lastpos.纬度.Value, lastpos.经度.Value));

                                                }
                                                else
                                                {
                                                    AddPlanWain(overplan, "中间联络缺失", fhphone);
                                                }
                                            }
                                            #endregion

                                            #region 计算远端防护距离
                                            if (allphones.Length > 3 && allphones[3] != "")
                                            {
                                                //只计算两个远端防护
                                                string phonea = allphones[3].Split(',')[0];

                                                T_MOBILELASTPOS fhapos = oc.BllSession.IT_MOBILELASTPOSService.Entities
                                                    .Where(p => p.手机号.Equals(phonea)
                                                    && p.时间 >= dt1 && p.时间 <= dt2
                                                    ).OrderByDescending(p => p.时间).FirstOrDefault();
                                                if (fhapos != null)
                                                {
                                                    flag = true;
                                                    safe.PHONE_SAFEA = phonea;
                                                    safe.MILEAGE_SAFEA = fhapos.里程;
                                                    safe.TIME_SAFEA = fhapos.时间;
                                                    if (overplan.LINE_NAME == fhapos.线名)
                                                    {
                                                        safe.SAFE_RANGE_LC_ONE = Convert.ToDecimal(Math.Abs(Convert.ToDouble(fhapos.里程) - pos) * 1000);
                                                        //里程是否在作业里程范围内±2km内
                                                        double fhpos = Convert.ToDouble(fhapos.里程);
                                                        if (fhpos < Convert.ToDouble(overplan.MILEAGE_BEGIN.Value) - 2 || fhpos > Convert.ToDouble(overplan.MILEAGE_END.Value) + 2)
                                                        {
                                                            AddPlanWain(overplan, "远端防护缺失", phonea);
                                                        }
                                                    }
                                                    safe.SAFE_RANGE_ONE = Convert.ToDecimal(CalcGpsDis.reGpsDis(fhapos.纬度.Value, fhapos.经度.Value, lastpos.纬度.Value, lastpos.经度.Value));

                                                }
                                                else
                                                {
                                                    AddPlanWain(overplan, "远端防护缺失", phonea);
                                                }

                                                if (allphones[3].Contains(","))
                                                {
                                                    string phoneb = allphones[3].Split(',')[1];

                                                    T_MOBILELASTPOS fhbpos = oc.BllSession.IT_MOBILELASTPOSService.Entities
                                                        .Where(p => p.手机号.Equals(phoneb)
                                                        && p.时间 >= dt1 && p.时间 <= dt2
                                                        ).OrderByDescending(p => p.时间).FirstOrDefault();
                                                    if (fhbpos != null)
                                                    {
                                                        flag = true;
                                                        safe.PHONE_SAFEB = phoneb;
                                                        safe.MILEAGE_SAFEB = fhbpos.里程;
                                                        safe.TIME_SAFEB = fhbpos.时间;
                                                        if (overplan.LINE_NAME == fhbpos.线名)
                                                        {
                                                            safe.SAFE_RANGE_LC_TWO = Convert.ToDecimal(Math.Abs(Convert.ToDouble(fhbpos.里程) - pos) * 1000);
                                                            //里程是否在作业里程范围内±2km内
                                                            double fhpos = Convert.ToDouble(fhbpos.里程);
                                                            if (fhpos < Convert.ToDouble(overplan.MILEAGE_BEGIN.Value) - 2 || fhpos > Convert.ToDouble(overplan.MILEAGE_END.Value) + 2)
                                                            {
                                                                AddPlanWain(overplan, "远端防护缺失", phoneb);
                                                            }
                                                        }
                                                        safe.SAFE_RANGE_TWO = Convert.ToDecimal(CalcGpsDis.reGpsDis(fhbpos.纬度.Value, fhbpos.经度.Value, lastpos.纬度.Value, lastpos.经度.Value));

                                                    }
                                                    else
                                                    {
                                                        AddPlanWain(overplan, "远端防护缺失", phoneb);
                                                    }
                                                }

                                            }
                                            if (flag)
                                            {
                                                int s = oc.BllSession.IJOB_PLAN_SAFEService.AddEntity(safe);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        overplan.WAINTIME = dtNow;//报警时间
                        if (overplan.DEPTCODE.Substring(0, 2) == "13" || overplan.DEPTCODE.Substring(0, 4) == "0702")//如果是南昌,合肥
                        {
                            if (overplan.PHONENUM == "驻站负责人")//驻站负责人
                            {
                                //	驻站防护员：【计划时间】开始时，无定位数据，或定位数据不在作业范围内，报警“驻站缺失，请注意！”。
                                AddPlanWain(overplan, "驻站防护缺失");
                            }
                        }
                    }
                    #endregion

                    #region 迟到\未到现场
                    if (overplan.ERRTYPE == "-1" || overplan.ERRTYPE == "0")//如果无其他报警
                    {
                        //RecordLog.RecordError("报警计算无定位：" + dtNow + "-" + overplan.PHONE + "-" + overplan.PLANTIME_END + "-" + overplan.ID);

                        //未到现场报警
                        if (dtNow >= overplan.PLANTIME_END.Value)//施工结束
                        {
                            if (oc.BllSession.IT_SGJHWARNService.Entities.Where(p => p.施工ID.Equals(overplan.ID) && p.手机号.Equals(overplan.PHONE) && p.超范围类型 != "迟到").Count() == 0)
                            {
                                overplan.ERRTYPE = 4.ToString();
                            }
                        }
                        else
                        {
                            //if (overplan.WAIN_TYPE == "2")//当时迟到报警和早到报警为什么冲突,没想清楚,有空再细研究
                            //{
                                //迟到报警（奎屯、广州）
                                if (dtNow >= overplan.PLANTIME_BEGIN.Value.AddMinutes(Cd_Minute))
                                {
                                    if (oc.BllSession.IT_SGJHWARNService.Entities.Where(p => p.施工ID.Equals(overplan.ID) && p.手机号.Equals(overplan.PHONE)).Count() == 0)
                                    {
                                        overplan.ERRTYPE = 5.ToString();
                                    }
                                }
                           // }
                        }
                    }

                    #endregion

                }  
                #endregion

                #region 处理同一人，相临或交叉时间段内有不同施工
                string[] warnType = new string[7] { "正常", "超时间", "超范围", "超时间超范围", "未到现场", "迟到", "未上道" };

                for (int i = OverPlan.Count - 1; i >= 0; i--)
                {
                    JOB_PlanWainTime plan = OverPlan[i];

                    if (plan.ERRTYPE != "-1")
                    {
                        if (plan.ERRTYPE != "0" && JOB_AA.ContainsKey(plan.PHONE))
                        {
                            plan.ERRTYPE = "0";//处理同一人，相临或交叉时间段内有不同施工
                        }
                        AddPlanWain(plan, warnType[Convert.ToInt32(plan.ERRTYPE)]);
                    }
                } 
                #endregion

                #region 其他计划
                if (sysDept == "03")
                {
                    GetCarPlan();//计算 确认车计划报警,北京高铁段专用
                }
                if (sysDept == "08")
                {
                    GetRainPlan();//出巡计划解除,广州专用
                } 
                #endregion

            }
            catch (Exception ex)
            {
                RecordLog.RecordError("报警计算：" + ex.ToString());
            }
            finally
            {
                WARNFLAG = false;//报警计算结束
            }
            watch.Stop();
            RecordLog.RecordError("报警计算时间：" + watch.ElapsedMilliseconds.ToString());
        }

        public static void AddPlanWain(JOB_PlanWainTime plan, string warnType, string warnPhone="")
        {

            #region 南昌专用，驻站报警，只报超范围
            if (plan.DEPTCODE.Substring(0, 2) == "13" && plan.PHONENUM == "驻站负责人")//南昌专用，驻站报警，只报超范围
            {
                if (plan.ERRTYPE == "2" || plan.ERRTYPE == "3")
                {
                    warnType = "驻站防护缺失";
                }
                else
                {
                    return;
                }
            } 
            #endregion

            //此手机号此计划所有报警
            string planWarnPhone = warnPhone == "" ? plan.PHONE : warnPhone;//如果是防护手机
            string planWarnPhoneType = warnPhone == "" ? plan.PHONETYPE : "";//负责人手机类型
            List<T_SGJHWARN> sgjhWarn = oc.BllSession.IT_SGJHWARNService.Entities.Where(p => p.施工ID.Equals(plan.ID) && p.手机号.Equals(planWarnPhone)).ToList();
            
            
            //如果是超范围且已经有迟到则不报超范围
            if (plan.ERRTYPE == "2" && sgjhWarn.Where(p => p.超范围类型 == "迟到").Count() > 0)
            {
                RecordLog.RecordError("报警计算：超范围且已经有迟到则不报超范围施工ID：" + plan.ID + "手机：" + plan.PHONE);
                return;
            }

            #region 报警记录
            T_SGJHWARN warn = new T_SGJHWARN();
            warn.手机号 = planWarnPhone;
            warn.PHONE_TYPE = planWarnPhoneType;//负责人类型
            warn.起始时间 = plan.WAINTIME.Value;//报警时间
            warn.结束时间 = DateTime.Now;//入库时间
            warn.超范围类型 = warnType;
            warn.施工ID = plan.ID;
            warn.消警 = "0";
            warn.PKID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            warn.DEPT_CODE = plan.DEPTCODE;
            warn.类型 = plan.TYPE;
            warn.PLAN_ID = plan.PLAN_ID;
            warn.WAINCONTENT = "开始时间:" + plan.PLANTIME_BEGIN + "；结束时间:" + plan.PLANTIME_END;

            if (plan.火星经度 != null && plan.火星纬度 != null)
            {
                warn.火星经度 = plan.火星经度.Value;
                warn.火星纬度 = plan.火星纬度.Value;
            }
            if (plan.里程 != null)
            {
                warn.线名 = plan.线名;
                warn.行别 = plan.行别;
                warn.里程 = plan.里程.Value;
            } 
            #endregion


            if (sgjhWarn.Where(t => t.超范围类型.Equals(warn.超范围类型)).Count() == 0)
            {
                int a = oc.BllSession.IT_SGJHWARNService.AddEntity(warn);
                
                #region 报警短信发送
                if (warn.超范围类型 != "正常")
                {
                    string sendPhone = warn.手机号;
                    
                    #region 南昌局计算群发(2.16南昌局仅为南昌段)
                    /* */
                    //以下为南昌局计算群发
                    if (plan.DEPTCODE.Length>=4 && plan.DEPTCODE.Substring(0,4) == "1312")//如果是南昌
                    {
                        // 作业负责人、各种防护员、驻站人员：无须设置，默认推送与其相关的报警信息。
                        // 车间级别：无论车间主任、书记、副主任，直接收本车间的报警推送信息。
                        // 段级：路桥科人员接收“路桥作业”的报警信息，路桥车间作业为“路桥作业”。 
                        // 线路科人员接收“线路作业”的报警信息，路桥车间之外的其他车间作业都为“线路作业”。
                        // 段领导及其他科室人员接收所有作业计划的报警信息。
                    
                        #region 获取计划作业负责人等手机号
                        //作业负责人、各种防护员、驻站人员：无须设置，默认推送与其相关的报警信息
                        //2017.2.13 补充
                        //是指同一条作业计划中，作业负责人，把关人接收本条计划中所有人的报警信息，其他人只接收自己的。例如，驻站员只接收自己的报警信息。
                        //List<string> phoneList = plan.PHONE_LIST.Replace('|', ',').Split(',').ToList();
                        //RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + ",负责人等:" + string.Join(",", phoneList));
                        List<string> phoneListByRole = plan.PHONE_LIST.Split('|').ToList();
                        List<string> phoneList = phoneListByRole[0].Split(',').ToList();
                        RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + ",1类角色手机号:" + string.Join(",", phoneList));
                        if (warn.超范围类型.Equals("工地防护缺失") || warn.超范围类型.Equals("驻站防护缺失")
                            || warn.超范围类型.Equals("中间联络缺失") || warn.超范围类型.Equals("远端防护缺失")) 
                        {
                            phoneList.Add(warn.手机号);
                            RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + ",超范围类型:" + warn.超范围类型+",角色手机号:"+warn.手机号);
                        }
                        #endregion
                        #region 2.16新逻辑
                        //获取本车间内 所有手机号码设置
                        List<JOB_PLAN_WARNPHONESET> warnSetList = oc.BllSession.IJOB_PLAN_WARNPHONESETService.Entities.ToList();
                        if (warnSetList != null && warnSetList.Count > 0)
                        {
                            try
                            {
                                //段，及段以下一级
                                List<SYS_DEPT> deptCJList = oc.BllSession.ISYS_DEPTService.Entities.Where(u => u.PARENT_CODE.Equals("1312")
                                                                ||u.DEPT_CODE.Equals("1312"))
                                                            .ToList().Select(u => u.ToPOCO()).ToList();
                                Dictionary<string, List<SYS_DEPT>> deptDic = new Dictionary<string, List<SYS_DEPT>>();
                                deptDic.Add("a", new List<SYS_DEPT>());
                                deptDic.Add("b", new List<SYS_DEPT>());
                                #region 分为a段机关+安调中心（段级）、b路桥车间+线路车间+XX车间+其他车间（车间）
                                foreach (SYS_DEPT dept in deptCJList)
                                {
                                    if (dept.DEPT_NAME.Contains("段机关") 
                                        || dept.DEPT_NAME.Contains("安调中心") || dept.DEPT_NAME.Contains("调度中心")
                                        || dept.DEPT_CODE.Equals("1312"))
                                    {
                                        deptDic["a"].Add(dept);
                                    }
                                    else
                                    {
                                        deptDic["b"].Add(dept);
                                    }
                                }
                                #endregion
                                string lqcjDeptcode = string.Empty;
                                #region 在b车间截取路桥车间编码，用于做为路桥计划判断
                                foreach (SYS_DEPT dept in deptDic["b"])
                                {
                                    if (dept.DEPT_NAME.Contains("路桥"))
                                    {
                                        lqcjDeptcode = dept.DEPT_CODE;
                                        break;
                                    }
                                }
                                #endregion
                                Dictionary<string, List<SYS_DEPT>> deptDuanDic = new Dictionary<string, List<SYS_DEPT>>();
                                deptDuanDic.Add("l", new List<SYS_DEPT>());
                                deptDuanDic.Add("x", new List<SYS_DEPT>());
                                deptDuanDic.Add("o", new List<SYS_DEPT>());
                                List<string> duanParDeptCodeList = new List<string>();
                                #region 细分a段级下的路桥科（l），线路科(x)，其他科(o),其中 其他科含段直属，或者段机关、安调中心直属
                                duanParDeptCodeList = deptDic["a"].Where(u=>u.DEPT_CODE!="1312").Select(u => u.DEPT_CODE).Distinct().ToList();
                                List<SYS_DEPT> duanDeptList = oc.BllSession.ISYS_DEPTService.Entities.Where(u => duanParDeptCodeList.Contains(u.PARENT_CODE)
                                    ||duanParDeptCodeList.Contains(u.DEPT_CODE))
                                    .ToList().Select(u => u.ToPOCO()).ToList();//仅段机关或安调中心及各自的下一级
                                foreach (SYS_DEPT dept in duanDeptList)
                                {
                                    if (dept.DEPT_NAME.Contains("路桥"))
                                    {
                                        deptDuanDic["l"].Add(dept);
                                    }
                                    else
                                    {
                                        if (dept.DEPT_NAME.Contains("线路"))
                                        {
                                            deptDuanDic["x"].Add(dept);
                                        }
                                        else
                                        {
                                            deptDuanDic["o"].Add(dept);
                                        }
                                    }
                                }
                                deptDuanDic["o"].Add(deptCJList.Where(u=>u.DEPT_CODE.Equals("1312")).FirstOrDefault());
                                #endregion

                                List<string> otherWarnNo = new List<string>();//临时储存
                                #region 段级a
                                if (deptDic["a"] != null && deptDic["a"].Count() > 0) 
                                {
                                    //含有段级，解析
                                    List<string> duanPlanDeptCode = new List<string>();
                                    if (deptDuanDic["o"] != null && deptDuanDic["o"].Count() > 0) 
                                    {
                                        //含有段直属人员，安调中心直属，段机关直属
                                        duanPlanDeptCode.AddRange(deptDuanDic["o"].Select(u => u.DEPT_CODE).Distinct().ToList());
                                    }
                                    if (plan.DEPTCODE.Length >= 6)
                                    {
                                        if (!string.IsNullOrEmpty(lqcjDeptcode)
                                        && plan.DEPTCODE.Substring(0, 6).Equals(lqcjDeptcode))
                                        {
                                            //路桥作业
                                            if (deptDuanDic["l"] != null && deptDuanDic["l"].Count() > 0)
                                            {
                                                duanPlanDeptCode.AddRange(deptDuanDic["l"].Select(u => u.DEPT_CODE).Distinct().ToList());
                                            }
                                        }
                                        else
                                        {
                                            //线路作业
                                            if (deptDuanDic["x"] != null && deptDuanDic["x"].Count() > 0)
                                            {
                                                duanPlanDeptCode.AddRange(deptDuanDic["x"].Select(u => u.DEPT_CODE).Distinct().ToList());
                                            }
                                        }
                                    }
                                    else 
                                    {
                                        //局，段归为 线路作业
                                        if (deptDuanDic["x"] != null && deptDuanDic["x"].Count() > 0)
                                        {
                                            duanPlanDeptCode.AddRange(deptDuanDic["x"].Select(u => u.DEPT_CODE).Distinct().ToList());
                                        }
                                    }
                                    RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + ",段级部门编码:" + string.Join(",", duanPlanDeptCode));
                                    foreach (string dept in duanPlanDeptCode) 
                                    {
                                        if (dept.Length > 6)
                                        {
                                            //各科室及科室下
                                            otherWarnNo.AddRange(warnSetList.Where(u => u.DEPT_CODE.StartsWith(dept)).Select(u => u.PHONE_NO).Distinct().ToList());
                                        }
                                        else 
                                        {
                                            //段直属，段机关直属，安调中心直属
                                            //段，段机关，安调中心自己的计划已按线路作业发送
                                            otherWarnNo.AddRange(warnSetList.Where(u => u.DEPT_CODE.Equals(dept)).Select(u => u.PHONE_NO).Distinct().ToList());
                                        }
                                    }
                                    phoneList.AddRange(otherWarnNo);
                                    RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + ",段级手机号码:" + string.Join(",", otherWarnNo));
                                }
                                #endregion
                                otherWarnNo = new List<string>();
                                #region 车间b
                                if (deptDic["b"] != null && deptDic["b"].Count() > 0) 
                                {
                                    List<string> cjParDeptCode = deptDic["b"].Select(u => u.DEPT_CODE).Distinct().ToList();
                                    if (plan.DEPTCODE.Length >= 6) 
                                    {
                                        string planCJdeptCode = plan.DEPTCODE.Substring(0,6);
                                        if (cjParDeptCode.Contains(planCJdeptCode)) 
                                        {
                                            otherWarnNo = warnSetList.Where(u => plan.DEPTCODE.StartsWith(u.DEPT_CODE)).Select(u => u.PHONE_NO).Distinct().ToList();
                                        }
                                    }
                                }
                                phoneList.AddRange(otherWarnNo);
                                RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + ",车间手机号码:" + string.Join(",", otherWarnNo));
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                RecordLog.RecordError(ex.ToString());
                            }
                        } 
                        #endregion
                        #region 发送
                        List<string> hadSend = new List<string>();//已发
                        if (phoneList != null && phoneList.Count > 0) 
                        {
                            #region 获取智能机回复IP,端口号
                            string sendToInfoIP = "221.204.213.61";
                            int sendToInfoPort = 8003;
                            try
                            {
                                sendToInfoIP = ConfigurationManager.AppSettings["sendToInfoIP"];
                                sendToInfoPort = Convert.ToInt32(ConfigurationManager.AppSettings["sendToInfoPort"]);
                            }
                            catch (Exception)
                            {
                            }
                            #endregion

                            #region 获取报警手机对应姓名
                            //6.7 新需求:手机APP的报警模块，报警内容需写出【姓名】和【电话】，目前缺少【姓名】字段
                            //6.19 修改报警内容展示形式
                            string memberContent = "手机号:" + warn.手机号;
                            SYS_MEMBER warnMember = oc.BllSession.ISYS_MEMBERService.Entities.Where(u => u.DEL_FLAG.Equals("0") && u.MOBILE.Equals(warn.手机号)).FirstOrDefault();
                            if (warnMember != null) 
                            {
                                memberContent = warnMember.NAME + ":" + warn.手机号;
                            }
                            #endregion

                            #region 发送
                            string warnContent = "时间:";
                            if (plan.PLANTIME_BEGIN.HasValue)
                            {
                                warnContent += plan.PLANTIME_BEGIN.Value.Month + "/" + plan.PLANTIME_BEGIN.Value.Day + " " + plan.PLANTIME_BEGIN.Value.Hour + ":" + plan.PLANTIME_BEGIN.Value.Minute;
                            }
                            else 
                            {
                                warnContent += "无";
                            }
                            if (plan.PLANTIME_END.HasValue)
                            {
                                warnContent += "-" + plan.PLANTIME_END.Value.Hour + ":" + plan.PLANTIME_END.Value.Minute;
                            }
                            else
                            {
                                warnContent += "-无";
                            }
                            string msg = "【施工报警】" + memberContent + "," + warn.超范围类型 + "_" + warnContent;
                            RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + msg);
                            foreach (string phone in phoneList)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(phone) && !hadSend.Contains(phone))
                                    {
                                        bool b = sendSMS.SendToInfo(phone,
                                                                 WithSProtocol.GetInfo(phone, msg, sendToInfoIP, sendToInfoPort, 2),
                                                                 0);
                                        hadSend.Add(phone);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    RecordLog.RecordError(ex.ToString());
                                    continue;
                                }
                            }
                            RecordLog.RecordInfo("计划ID:" + plan.PLAN_ID + "已发送手机号:" + string.Join(",", hadSend));
                            #endregion
                        }
                        #endregion
                    }
                    /**/
                    #endregion
                    try
                    {
                        if (plan.DEPTCODE.Substring(0, 2) == "07")//上海
                        {
                            bool n = sendSMS.SendToInfo(sendPhone,WithSProtocol.GetInfo(sendPhone, warn.超范围类型, "221.204.213.61", 8017, 2),0);
                        }
                        else
                        {
                         int m = sendSMS.ThingSend(sendPhone, "施工报警", warn.超范围类型, null, 0);
                        }
                    }
                    catch (Exception)
                    {
                        RecordLog.RecordError("报警短信发送失败:" + sendPhone);
                    }
                }
                #endregion
                
            }
        }
        #endregion


        #region 获取正常计划
        /// <summary>
        /// 获取正常计划
        /// </summary>
        /// <param name="OverPlan"></param>
        public static void GetJobPlan(List<JOB_PlanWainTime> OverPlan, int afterMinute, int beforeMinute)
        {

            //取计划：当前时间前后几小时时间段内，计划
            try
            {

                //int intJOB_GetPlanMaxPrepareMinute = 60;// Model_JOB_PLAN.GetPlanMaxPrepareMinute();//作业计划最大时间 90分钟
                DateTime beginTime = DateTime.Now.AddMinutes(beforeMinute);
                DateTime endTime = DateTime.Now.AddMinutes(afterMinute);

                List<JOB_PLAN> sgjh = oc.BllSession.IJOB_PLANService.Entities.Where(p => !string.IsNullOrEmpty(p.PHONE) && p.MARS_LATITUDE != null && p.PLAN_FLAG != "取消" && p.PLAN_FLAG != "只登记不作业" && p.PLAN_FLAG != "已变更" && p.PLAN_FLAG != "配合")//可定位的正常,变更计划
                    .Where(u => u.PLANTIME_BEGIN <= endTime && u.PLANTIME_END >= beginTime).OrderBy(u => u.PLANTIME_BEGIN)
                    .ToList();
                string wainTypeDept = Model_JOB_PLAN.GetPlanWainTypeDept();//"1201,0802,";迟到报警编码，奎屯、广州

                //RecordLog.RecordError("报警计算取计划x：" + sgjh.Count + ":" + wainTypeDept);
                foreach (JOB_PLAN plan in sgjh)
                {

                    JOB_PlanWainTime overPlan = new JOB_PlanWainTime();
                    overPlan.PLAN_ID = plan.PLAN_ID;
                    overPlan.PHONE = plan.PHONE;
                    overPlan.PHONENUM = "0";//第几作业负责人
                    overPlan.LINE_NAME = plan.LINE_NAME;
                    overPlan.DIRECTION = plan.DIRECTION;
                    overPlan.PLANTIME_BEGIN = plan.PLANTIME_BEGIN;
                    overPlan.PLANTIME_END = plan.PLANTIME_END;
                    overPlan.MILEAGE_BEGIN = plan.MILEAGE_BEGIN;
                    overPlan.MILEAGE_END = plan.MILEAGE_END;
                    overPlan.WAINTIME = DateTime.Now;//新扩展的字段，MODEL里必须更新
                    //string time = DateTime.Now.ToString("yyyyMMdd");
                    overPlan.ID = plan.PLAN_TYPE + plan.PLAN_ID;
                    overPlan.DEPTCODE = plan.DEPT_CODE;
                    overPlan.TYPE = plan.PLAN_TYPE;
                    overPlan.ERRTYPE = (-1).ToString();
                    overPlan.UPTIME_BEGIN = plan.UPTIME_BEGIN;
                    overPlan.DOWNTIME_END = plan.DOWNTIME_END;
                    overPlan.WAIN_TYPE = wainTypeDept.IndexOf(plan.DEPT_CODE.Length > 2 ? plan.DEPT_CODE.Substring(0, 4) : plan.DEPT_CODE) > -1 ? "2" : "1";
                    overPlan.REST_TIME = plan.REST_TIME;//午休时间
                    overPlan.PHONE_LIST = plan.PHONE_LIST;//其他人员手机
                    overPlan.WORK_TIME = plan.WORK_TIME;//实际作业时间
                    OverPlan.Add(overPlan);
                }
            }
            catch (Exception e)
            {

                RecordLog.RecordError("报警计算取计划t：" + e.ToString());
            }

        } 
        #endregion

        #region 获取西安计划
        /// <summary>
        /// 获取西安计划
        /// </summary>
        /// <param name="OverPlan"></param>
        public static void GetXiAnJobPlan(List<JOB_PlanWainTime> OverPlan, int afterMinute, int beforeMinute)
        {

            //取计划：当前时间前后几小时时间段内，计划

            //int intJOB_GetPlanMaxPrepareMinute = 60;// Model_JOB_PLAN.GetPlanMaxPrepareMinute();//作业计划最大时间 90分钟
            DateTime beginTime = DateTime.Now.AddMinutes(beforeMinute);
            DateTime endTime = DateTime.Now.AddMinutes(afterMinute);

            List<JOB_PLAN> sgjh = oc.BllSession.IJOB_PLANService.Entities.Where(p => !string.IsNullOrEmpty(p.PHONE) && p.MARS_LATITUDE != null && p.PLAN_FLAG != "取消" && p.PLAN_FLAG != "只登记不作业" && p.PLAN_FLAG != "已变更" && p.PLAN_FLAG != "配合")//可定位的计划
                .Where(u => u.PLANTIME_BEGIN <= endTime && u.PLANTIME_END >= beginTime)
                .Where(p => !string.IsNullOrEmpty(p.PHONE_LIST)).OrderBy(u => u.PLANTIME_BEGIN)
                .ToList();
            string wainTypeDept = Model_JOB_PLAN.GetPlanWainTypeDept();//"1201,1305,";迟到报警编码，西安早到报警，南昌迟到报警

            foreach (JOB_PLAN plan in sgjh)
            {
                string[] phones = plan.PHONE_LIST.Split('|')[0].Split(',');//只取负责人
                if (string.IsNullOrEmpty(plan.PHONELIST_TYPE))
                {
                    plan.PHONELIST_TYPE = "";
                }
                string[] phonetypes = plan.PHONELIST_TYPE.Split(',');//负责人类型
                for (int i = 0; i < phones.Length; i++)
                {
                    JOB_PlanWainTime overPlan = new JOB_PlanWainTime();
                    overPlan.PLAN_ID = plan.PLAN_ID;
                    overPlan.PHONE = phones[i];
                    if (phonetypes.Length > i)
                    {
                        overPlan.PHONETYPE = phonetypes[i];//南昌负责人类型
                    }
                    else
                    {
                        overPlan.PHONETYPE = "";
                    }
                    overPlan.PHONENUM = i.ToString();//第几作业负责人
                    overPlan.LINE_NAME = plan.LINE_NAME;
                    overPlan.DIRECTION = plan.DIRECTION;
                    overPlan.PLANTIME_BEGIN = plan.PLANTIME_BEGIN;
                    overPlan.PLANTIME_END = plan.PLANTIME_END;
                    overPlan.MILEAGE_BEGIN = plan.MILEAGE_BEGIN;
                    overPlan.MILEAGE_END = plan.MILEAGE_END;
                    overPlan.WAINTIME = DateTime.Now;
                    //string time = DateTime.Now.ToString("yyyyMMdd");
                    overPlan.ID = plan.PLAN_TYPE + plan.PLAN_ID;
                    overPlan.DEPTCODE = plan.DEPT_CODE;
                    overPlan.TYPE = plan.PLAN_TYPE;
                    overPlan.ERRTYPE = (-1).ToString();
                    overPlan.UPTIME_BEGIN = plan.UPTIME_BEGIN;
                    overPlan.DOWNTIME_END = plan.DOWNTIME_END;
                    overPlan.WAIN_TYPE = wainTypeDept.IndexOf(plan.DEPT_CODE.Length > 2 ? plan.DEPT_CODE.Substring(0, 4) : plan.DEPT_CODE) > -1 ? "2" : "1";//西安早到报警，南昌迟到报警
                    //启用午休时间，西安计划表需要增加午休字段
                    overPlan.REST_TIME = plan.REST_TIME;//午休时间
                    overPlan.PHONE_LIST = plan.PHONE_LIST;//其他人员手机
                    overPlan.WORK_TIME = plan.WORK_TIME;//实际作业时间
                    OverPlan.Add(overPlan);
                }
                //南昌用，西安局需要增加此字段
                if (plan.DEPT_CODE.Substring(0, 2) == "13" || plan.DEPT_CODE.Substring(0, 4) == "0702")//如果是南昌,合肥
                {
                    if (!string.IsNullOrEmpty(plan.PLAN_OFFICER_PHONE_MILEAGE))//如果要计算驻站
                    {

                        string[] zzinfo = plan.PLAN_OFFICER_PHONE_MILEAGE.Split('|');//取驻站
                        string[] zzphones = zzinfo[0].Split(',');//取驻站手机
                        for (int i = 0; i < zzphones.Length; i++)
                        {
                            JOB_PlanWainTime overPlan = new JOB_PlanWainTime();
                            overPlan.PLAN_ID = plan.PLAN_ID;
                            overPlan.PHONE = zzphones[i];
                            overPlan.PHONETYPE = "驻站负责人";//驻站负责人
                            overPlan.PHONENUM = "驻站负责人";//驻站负责人
                            overPlan.LINE_NAME = zzinfo[1];
                            overPlan.DIRECTION = zzinfo[2];
                            overPlan.PLANTIME_BEGIN = plan.PLANTIME_BEGIN;
                            overPlan.PLANTIME_END = plan.PLANTIME_END;
                            overPlan.MILEAGE_BEGIN = Convert.ToDecimal(zzinfo[3]);
                            overPlan.MILEAGE_END = Convert.ToDecimal(zzinfo[4]);
                            overPlan.WAINTIME = DateTime.Now;
                            //string time = DateTime.Now.ToString("yyyyMMdd");
                            overPlan.ID = plan.PLAN_TYPE + plan.PLAN_ID;
                            overPlan.DEPTCODE = plan.DEPT_CODE;
                            overPlan.TYPE = plan.PLAN_TYPE;
                            overPlan.ERRTYPE = (-1).ToString();
                            overPlan.UPTIME_BEGIN = plan.UPTIME_BEGIN;
                            overPlan.DOWNTIME_END = plan.DOWNTIME_END;
                            overPlan.WAIN_TYPE = wainTypeDept.IndexOf(plan.DEPT_CODE.Length > 2 ? plan.DEPT_CODE.Substring(0, 4) : plan.DEPT_CODE) > -1 ? "2" : "1";//西安早到报警，南昌迟到报警
                            //启用午休时间，西安计划表需要增加午休字段
                            overPlan.REST_TIME = plan.REST_TIME;//午休时间
                            overPlan.PHONE_LIST = plan.PHONE_LIST;//其他人员手机
                            overPlan.WORK_TIME = plan.WORK_TIME;//实际作业时间
                            OverPlan.Add(overPlan);
                        }
                    }
                }
            }

        }


        #endregion

        #region 上道下道弃用

        // 弃用GetJobPlanSDXD(OverPlan);//计算 上道下道时间
        public static void GetJobPlanSDXD(List<JOB_PlanWainTime> jobPlan)//计算 上道下道时间
        {
            //要计算的计划
            List<JOB_PlanWainTime> sxdPlan = jobPlan.Where(u => u.UPTIME_BEGIN == null || u.DOWNTIME_END == null).OrderBy(u => u.PLANTIME_BEGIN).ToList();
            foreach (JOB_PlanWainTime item in sxdPlan)
            {
                bool flag = false;
                JOB_PLAN upPlan = new JOB_PLAN
                {
                    PLAN_ID = item.PLAN_ID,
                    UPTIME_BEGIN = item.UPTIME_BEGIN,
                    DOWNTIME_END = item.DOWNTIME_END
                };
                DateTime DTCheckStart = item.PLANTIME_BEGIN.Value.AddHours(-2);
                DateTime DTCheckEnd = item.PLANTIME_END.Value.AddHours(2);
                //查找同一人有没有连续施工计划
                JOB_PlanWainTime somePlan = jobPlan.Where(u => u.PHONE == item.PHONE && u.PLAN_ID != item.PLAN_ID && DTCheckStart < u.PLANTIME_END && u.PLANTIME_END > item.PLANTIME_BEGIN).OrderBy(u => u.PLANTIME_BEGIN).FirstOrDefault();
                if (somePlan != null)
                {
                    if (somePlan.DOWNTIME_END != null)
                    {
                        DTCheckStart = somePlan.DOWNTIME_END.Value;
                    }
                    else
                    {
                        DTCheckStart = somePlan.PLANTIME_END.Value;
                    }
                }
                //计算上道时间
                T_MOBILEROUTE mobileRoute = oc.BllSession.IT_MOBILEROUTEService.Entities.Where(u => u.手机号 == item.PHONE && u.上道状态 != "" && u.时间 >= DTCheckStart && u.时间 <= DTCheckEnd && u.上道状态 == "上道").OrderBy(u => u.时间).FirstOrDefault();
                if (mobileRoute != null)
                {
                    item.UPTIME_BEGIN = mobileRoute.时间;
                    upPlan.UPTIME_BEGIN = mobileRoute.时间;
                    flag = true;
                }

                //计算下道时间
                if (item.UPTIME_BEGIN != null && item.DOWNTIME_END == null)
                {
                    mobileRoute = oc.BllSession.IT_MOBILEROUTEService.Entities.Where(u => u.手机号 == item.PHONE && u.上道状态 != "" && u.时间 >= item.PLANTIME_END && u.时间 <= DTCheckEnd && u.上道状态 == "下道").OrderBy(u => u.时间).FirstOrDefault();
                    if (mobileRoute != null)
                    {
                        item.DOWNTIME_END = mobileRoute.时间;
                        upPlan.DOWNTIME_END = mobileRoute.时间;
                        flag = true;
                    }
                }
                if (flag)
                {
                    int returnValue = oc.BllSession.IJOB_PLANService.UpdateEntity(upPlan, new string[] { "UPTIME_BEGIN", "DOWNTIME_END" });
                    break;//每次只计算一个上下道时间
                }
            }
        }

        #endregion

        #region 确认车计划
        public static void GetCarPlan()//计算 确认车计划报警
        {
            try
            {
                DateTime dtNow = DateTime.Now;
                DateTime dt1 = dtNow.AddMinutes(-10);
                DateTime dt2 = dtNow.AddMinutes(10);

                masService.FtsSoapClient xx = new masService.FtsSoapClient();

                //要计算的计划
                List<CONFIR_CAR_PLAN> carPlan = oc.BllSession.ICONFIR_CAR_PLANService.Entities.Include("CONFIR_CAR_WARN")
                    .Where(u => u.REMIND_TIME <= dtNow && dtNow <= u.DEPARTURE_STARTTIME && u.MARS_LATITUDE != null && u.MARS_LONGITUDE != null)
                    .OrderBy(u => u.DEPARTURE_STARTTIME).ToList();//现在是提醒与开车时间之间,有经纬度

                //List<string> planId = carPlan.Select(u => u.PKID).ToList();
                //List<CONFIR_CAR_WARN> carWarn = oc.BllSession.ICONFIR_CAR_WARNService.Entities
                //    .Where(u => planId.Contains(u.CARPLAN_PKID)).ToList();//计划报警

                foreach (CONFIR_CAR_PLAN item in carPlan)
                {
                    if (item.CONFIR_CAR_WARN.Where(u => u.WARN_TYPE == "到达车站").Count() > 0)//如果有到达车站记录
                    {
                        continue;
                    }

                    T_MOBILELASTPOS lastpos = oc.BllSession.IT_MOBILELASTPOSService.Entities
                        .Where(p => p.手机号.Equals(item.PHONE) && p.时间 >= dt1 && p.时间 <= dt2).FirstOrDefault();

                    if (lastpos != null)//如果有定位
                    {
                        #region 如果是开车前60分钟，是否到达车站
                        double disLine = CalcGpsDis.reGpsDis(item.MARS_LATITUDE.Value, item.MARS_LONGITUDE.Value, lastpos.纬度.Value, lastpos.经度.Value);
                        if (disLine < 500)//如果到达车站,小于500米
                        {
                            //添加到达车站提示
                            int a = oc.BllSession.ICONFIR_CAR_WARNService.AddEntity(new CONFIR_CAR_WARN()
                            {
                                PKID = item.PKID + "-001",
                                CARPLAN_PKID = item.PKID,
                                DEPT_CODE = item.DEPT_CODE,
                                WARN_TIME = dtNow,
                                WARN_TYPE = "到达车站",
                                WARN_CONTENT = item.TRAIN_NUM + "次添乘人员已到达车站",
                                WARN_STATE = "0"

                            });
                            continue;
                        }
                        //未到车站继续执行流程
                        #endregion


                        #region 1.叫醒：如果现在是提醒时间，发准备短信
                        if (dtNow <= item.REMIND_TIME.Value.AddMinutes(4))//如果现在是提醒时间(10分钟内)，发准备短信
                        {
                            if (item.CONFIR_CAR_WARN.Where(u => u.WARN_TYPE == "添乘准备").Count() == 0)//如果未发准备短信
                            {
                                int a = oc.BllSession.ICONFIR_CAR_WARNService.AddEntity(new CONFIR_CAR_WARN()
                                {
                                    PKID = item.PKID + "-002",
                                    CARPLAN_PKID = item.PKID,
                                    DEPT_CODE = item.DEPT_CODE,
                                    WARN_TIME = dtNow,
                                    WARN_TYPE = "添乘准备",
                                    WARN_CONTENT = item.TRAIN_NUM + "次添乘人员添乘准备",
                                    WARN_STATE = "0"
                                });
                                //发短信
                                try
                                {
                                    xx.ThingSend(item.PHONE, "添乘准备:车次" + item.TRAIN_NUM, "添乘准备:" + item.TRAIN_NUM + "次添乘人员添乘准备", null, 0);
                                }
                                catch (Exception)
                                {
                                    RecordLog.RecordWarn("添乘准备发送短信失败" + item.PHONE + ":" + item.TRAIN_NUM);
                                }
                            }
                            continue;
                        }
                        #endregion

                        #region 2.出发：如果是出发时间，记录此时经纬度
                        if (item.DEPARTURE_TIME.Value.AddMinutes(-4) <= dtNow && dtNow <= item.DEPARTURE_TIME)//如果是出发时间，记录此时经纬度
                        {
                            if (item.MARS_LONGITUDE_PERSON == null || item.MARS_LATITUDE_PERSON == null)
                            {
                                CONFIR_CAR_PLAN plan = new CONFIR_CAR_PLAN()
                                {
                                    PKID = item.PKID,
                                    MARS_LONGITUDE_PERSON = lastpos.经度,
                                    MARS_LATITUDE_PERSON = lastpos.纬度
                                };
                                int a = oc.BllSession.ICONFIR_CAR_PLANService.UpdateEntity(plan, new string[] { "MARS_LONGITUDE_PERSON", "MARS_LATITUDE_PERSON" });
                            }
                            continue;
                        }
                        #endregion

                        #region 3.移动：如果是10分钟移动判定时间，经纬度计算
                        DateTime MOVE_TIME = item.DEPARTURE_TIME.Value.AddMinutes(10);//判断移动时间
                        if (MOVE_TIME <= dtNow && dtNow <= MOVE_TIME.AddMinutes(4))//如果是10分钟移动判定时间，经纬度计算
                        {

                            if (item.CONFIR_CAR_WARN.Where(u => u.WARN_TYPE == "添乘出发").Count() == 0)//如果未发出发短信
                            {
                                bool nomove = true;
                                if (item.MARS_LATITUDE_PERSON != null && item.MARS_LONGITUDE_PERSON != null)//如果有出发时的定位
                                {
                                    nomove = CalcGpsDis.reGpsDis(item.MARS_LATITUDE_PERSON.Value, item.MARS_LONGITUDE_PERSON.Value, lastpos.纬度.Value, lastpos.经度.Value) < 300;
                                }
                                //未移动，发出发短信
                                if (nomove)//如果未移动,小于300米
                                {
                                    int a = oc.BllSession.ICONFIR_CAR_WARNService.AddEntity(new CONFIR_CAR_WARN()
                                    {
                                        PKID = item.PKID + "-003",
                                        CARPLAN_PKID = item.PKID,
                                        DEPT_CODE = item.DEPT_CODE,
                                        WARN_TIME = dtNow,
                                        WARN_TYPE = "添乘出发",
                                        WARN_CONTENT = item.TRAIN_NUM + "次添乘人员未及时出发",
                                        WARN_STATE = "0"
                                    });
                                    //发短信
                                    try
                                    {
                                        xx.ThingSend(item.PHONE, "添乘出发:车次" + item.TRAIN_NUM, "添乘出发:" + item.TRAIN_NUM + "次添乘人员请立即出发", null, 0);
                                    }
                                    catch (Exception)
                                    {
                                        RecordLog.RecordWarn("添乘出发发送短信失败" + item.PHONE + ":" + item.TRAIN_NUM);
                                    }
                                }
                            }
                            continue;
                        }
                        #endregion

                        #region 4.到站：如果是开车前30分钟，是否到达车站
                        if (item.DEPARTURE_STARTTIME.Value.AddMinutes(-30) <= dtNow && disLine >= 500)//如果是开车前30分钟，未到达车站,大于500米
                        {
                            if (item.CONFIR_CAR_WARN.Where(u => u.WARN_TYPE == "未到车站").Count() == 0)//如果未到车站
                            {
                                //添加到达车站提示
                                int a = oc.BllSession.ICONFIR_CAR_WARNService.AddEntity(new CONFIR_CAR_WARN()
                                {
                                    PKID = item.PKID + "-004",
                                    CARPLAN_PKID = item.PKID,
                                    DEPT_CODE = item.DEPT_CODE,
                                    WARN_TIME = dtNow,
                                    WARN_TYPE = "未到车站",
                                    WARN_CONTENT = "动检" + item.TRAIN_NUM + "次添乘人员未按规定时间到达车站",
                                    WARN_STATE = "0"

                                });
                            }
                            continue;
                        }
                        #endregion
                    }
                    else//无定位直接报未按时到达车站
                    {

                        #region 3.移动：如果是10分钟移动判定时间，经纬度计算
                        DateTime MOVE_TIME = item.DEPARTURE_TIME.Value.AddMinutes(10);//判断移动时间
                        if (MOVE_TIME <= dtNow && dtNow <= MOVE_TIME.AddMinutes(4))//如果是10分钟移动判定时间，经纬度计算
                        {
                            if (item.CONFIR_CAR_WARN.Where(u => u.WARN_TYPE == "添乘出发").Count() == 0)//如果未发出发短信
                            {
                                int a = oc.BllSession.ICONFIR_CAR_WARNService.AddEntity(new CONFIR_CAR_WARN()
                                {
                                    PKID = item.PKID + "-003",
                                    CARPLAN_PKID = item.PKID,
                                    DEPT_CODE = item.DEPT_CODE,
                                    WARN_TIME = dtNow,
                                    WARN_TYPE = "添乘出发",
                                    WARN_CONTENT = "动检" + item.TRAIN_NUM + "次添乘人员未及时出发",
                                    WARN_STATE = "0"
                                });
                            }
                            continue;
                        }
                        #endregion

                        #region 如果是开车前30分钟，是否到达车站
                        if (item.DEPARTURE_STARTTIME.Value.AddMinutes(-30) < dtNow)//如果是开车前30分钟，是否到达车站
                        {
                            if (item.CONFIR_CAR_WARN.Where(u => u.WARN_TYPE == "未到车站").Count() == 0)//如果没有未按时到达车站记录
                            {
                                //添加到达车站提示
                                int a = oc.BllSession.ICONFIR_CAR_WARNService.AddEntity(new CONFIR_CAR_WARN()
                                {
                                    PKID = item.PKID + "-004",
                                    CARPLAN_PKID = item.PKID,
                                    DEPT_CODE = item.DEPT_CODE,
                                    WARN_TIME = dtNow,
                                    WARN_TYPE = "未到车站",
                                    WARN_CONTENT = "动检" + item.TRAIN_NUM + "次添乘人员未按规定时间到达车站",
                                    WARN_STATE = "0"

                                });
                            }
                        }
                        #endregion
                    }

                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError("确认车计划：" + ex.ToString());
            }
        }

        #endregion


        #region 出巡计划
        public static void GetRainPlan()//出巡计划解除
        {
            try
            {

                DateTime dtNow = DateTime.Now.AddHours(-48);//解除后48小时还原为正常
                RAIN_DEVICE device = new RAIN_DEVICE()
                {
                    状态 = "正常",
                    更改状态时间 = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };
                List<RAIN_DEVICE> deviceList = oc.BllSession.IRAIN_DEVICEService.Entities.Where(u => u.状态 == "解除" && u.更改状态时间 <= dtNow).ToList();
                if (deviceList.Count() > 0)
                {
                    int a = oc.BllSession.IRAIN_DEVICEService.UpdateListEntity(device, u => u.状态 == "解除" && u.更改状态时间 <= dtNow, new string[] { "状态", "更改状态时间" });
                    foreach (RAIN_DEVICE item in deviceList)
                    {
                        oc.BllSession.IT_RAINWARNService.AddEntity(new T_RAINWARN()
                        {
                            时间 = device.更改状态时间.Value,
                            编号 = item.编号,
                            报警类型 = 0,
                            报警雨量种类 = 1
                        });
                    }
                }

                string planinfo = "";
                DateTime dtMove = DateTime.Now.AddMinutes(-30);//30分钟不移动



                List<RAIN_PLAN> planList = oc.BllSession.IRAIN_PLANService.Entities.Where(u => u.ERRORINFO == null || u.ERRORINFO == "").OrderBy(u => u.PHONE).ThenBy(u => u.MILEAGE_BEGIN_PATROL).ThenBy(u => u.MILEAGE_END_PATROL).ToList();
                int rowline = 1;
                foreach (RAIN_PLAN plan in planList)
                {
                    string thisplaninfo = plan.PHONE + "-" + plan.MILEAGE_BEGIN_PATROL + "-" + plan.MILEAGE_END_PATROL;
                    if (planinfo == thisplaninfo)//如果计划一样则跳出
                    {
                        continue;
                    }
                    planinfo = thisplaninfo;
                    rowline++;
                    List<T_MOBILEROUTE> phoneList = oc.BllSession.IT_MOBILEROUTEService.Entities
                                            .Where(u => u.手机号 == plan.PHONE && u.线名 == plan.LINE_NAME
                                                     && u.行别 == plan.DIRECTION && u.时间 >= dtMove)
                                                .OrderByDescending(u => u.时间).ToList();
                    if (phoneList.Count() < 5)
                    {
                        if (plan.UPDATETIME < dtMove)//已经开始30分钟则直接报未移动
                        {
                            //报警未移动
                            AddRainPlanWain(plan, DateTime.Now.ToString("yyyyMMddHHmmssfff") + rowline + "001", "未移动", "30分钟内未移动");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        decimal max = phoneList.Max(x => x.里程).Value;
                        decimal min = phoneList.Min(x => x.里程).Value;
                        if ((max - min) * 1000 <= 50)//移动小于50M
                        {
                            //报警未移动
                            AddRainPlanWain(plan, DateTime.Now.ToString("yyyyMMddHHmmssfff") + rowline + "002", "未移动", "30分钟内未移动");
                        }
                        string str = "";
                        for (int i = 0; i < 4; i++)
                        {
                            str += (phoneList[i].里程.Value - phoneList[i + 1].里程.Value) > 0 ? "+" : "-";
                        }

                        if (str == "++--" || str == "--++")//如果折返
                        {
                            if (Math.Abs(phoneList[2].里程.Value - plan.MILEAGE_BEGIN_PATROL.Value) * 1000 > 50
                                && Math.Abs(phoneList[2].里程.Value - plan.MILEAGE_END_PATROL.Value) * 1000 > 50)
                            {
                                //报警未到达管界点
                                AddRainPlanWain(plan, DateTime.Now.ToString("yyyyMMddHHmmssfff") + rowline + "003", "未到管界", "未到出巡管界点即返回(里程：" + phoneList[2].里程.Value + ")");
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError("出巡计划解除：" + ex.ToString());
            }
        }


        public static void AddRainPlanWain(RAIN_PLAN plan, string pkid, string warnType, string warnContent)
        {
            //SMSService.FtsSoapClient xx = new SMSService.FtsSoapClient();
            //如果没有报警记录或消警30分钟后则增加
            DateTime dtMove = DateTime.Now.AddMinutes(-30);//30分钟不移动
            if (oc.BllSession.IRAIN_PLAN_WARNService.Entities.Where(t => t.WARN_TYPE.Equals(warnType)
                && t.PHONE.Equals(plan.PHONE)
                && (t.WARN_STATE.Equals("0") || (t.WARN_STATE.Equals("1") && t.RELEASE_TIME > dtMove))).Count() == 0)//如果未消警
            {
                RAIN_PLAN_WARN warn = new RAIN_PLAN_WARN();
                warn.PKID = pkid;//生成主键
                warn.DEPT_CODE = plan.DEPT_CODE;
                warn.DEPT_NAME = plan.DEPT_NAME;
                warn.RAIN_CODE = plan.RAIN_CODE;
                warn.RAIN_NAME = plan.RAIN_NAME;
                warn.PLAN_CONTENT = "线名：" + plan.LINE_NAME + ",行别：" + plan.DIRECTION + ",起始里程：" + plan.MILEAGE_BEGIN_PATROL + ",结束里程：" + plan.MILEAGE_END_PATROL;
                warn.PHONE = plan.PHONE;
                warn.WARN_STATE = "0";
                warn.WARN_TIME = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                warn.WARN_TYPE = warnType;
                warn.WARN_CONTENT = warnContent;

                int c = oc.BllSession.IRAIN_PLAN_WARNService.AddEntity(warn);
            }
        }

        #endregion

        #region 计算当日计划人员图标
        public static void GetAllPhoneIconPlan()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string sysDept = Model_JOB_PLAN.GetSysDept();
            if (!string.IsNullOrEmpty(sysDept))
            {
                Dictionary<string, List<JOB_PLAN_ICON>> planList = new Dictionary<string, List<JOB_PLAN_ICON>>();
                //try
                //{

                #region 南昌
                if (sysDept == "13")//南昌
                {
                    List<JOB_PLAN_ICON> sgjh = oc.BllSession.IJOB_PLANService.Entities
                                                .Where(p => !string.IsNullOrEmpty(p.PHONE_LIST) && p.PLANTIME_BEGIN >= DateTime.Today)//今日正常计划
                                                .OrderBy(u => u.PLANTIME_BEGIN).Select(u => new JOB_PLAN_ICON
                                                {
                                                    PLAN_TYPE = u.PLAN_TYPE,
                                                    DEPT_CODE = u.DEPT_CODE,
                                                    DEPT_CJNAME = u.DEPT_CJNAME,
                                                    PHONE_LIST = u.PHONE_LIST,
                                                    PLANTIME_BEGIN=u.PLANTIME_BEGIN,
                                                    PLANTIME_END = u.PLANTIME_END
                                                }).ToList();
                    foreach (JOB_PLAN_ICON item in sgjh)

                    {
                        string icon = "red";//图标颜色,线路作业（红色）  ,按施工单位的车间名称判断
                                            //图标颜色——区分线路作业（红色）、路桥作业（黄色）、探伤作业（蓝色）、非作业人员（绿色）
                        if (!string.IsNullOrEmpty(item.DEPT_CJNAME))
                        {
                            if (item.DEPT_CJNAME.IndexOf("路桥") > -1)
                            {
                                icon = "yellow";//路桥作业（黄色）
                            }
                            if (item.DEPT_CJNAME.IndexOf("探伤") > -1)
                            {
                                icon = "blue";//探伤作业（蓝色）
                            }
                        }

                        string[] phoneType = item.PHONE_LIST.Split('|');//人员类型
                                                       //图标内的字——区分作业人员的类型，
                                                      //是负责人、把关人、质检员、驻站防护员、工地防护员、中间联络员、远端防护员、巡查巡检人员、添乘人员。
                        //15870644175,15870644314|15870644138,15870643959|15870644218||
                        for (int i = 0; i < phoneType.Length; i++)
                        {
                            #region MyRegion
                            switch (i)//负责人,把关人|工地防护员|驻站防护员|远端电话号码|中间联络
                            //主体作业负责人，共用作业负责人，配合作业负责人|工地防护员|驻站防护员|||质检员
                            {
                                case 0:
                                    item.ICON = icon + "_fzr";//负责人
                                    break;
                                case 1:
                                    item.ICON = icon + "_gd";//工地防护员
                                    break;
                                case 2:
                                    item.ICON = "green_zz";//驻站防护员,统一为绿色
                                    break;
                                case 3:
                                    item.ICON = icon + "_yd";//远端电话号码
                                    break;
                                case 4:
                                    item.ICON = icon + "_zj";//中间联络
                                    break;
                                case 5:
                                    item.ICON = icon + "_zjy";//质检员
                                    break;
                                default:
                                    break;
                            } 
                            #endregion
                            string[] phones = phoneType[i].Split(',');//多人
                            for (int j = 0; j < phones.Length; j++)
                            {
                                #region MyRegion
                                if (i == 0)//负责人细化
                                {
                                    switch (item.PLAN_TYPE.Length)
                                    {
                                        case 1://高速
                                            if (j == 1) { item.ICON = item.ICON.Replace("_fzr", "_gy"); };//共用作业负责人
                                            if (j == 2) { item.ICON = item.ICON.Replace("_fzr", "_ph"); };//配合作业负责人
                                            break;
                                        case 2://普速
                                            if (j == 1) { item.ICON = item.ICON.Replace("_fzr", "_bg"); };//把关人
                                            break;
                                        case 3://巡查巡检
                                            item.ICON = item.ICON.Replace("_fzr", "_xcxj");//巡查巡检人员
                                            break;
                                        case 4://添乘
                                            item.ICON = item.ICON.Replace("_fzr", "_tc");//添乘人员
                                            break;
                                        default:
                                            break;
                                    }
                                } 
                                #endregion
                                #region MyRegion
                                string phone = phones[j];
                                if (!string.IsNullOrEmpty(phone))
                                {
                                    JOB_PLAN_ICON plan = new JOB_PLAN_ICON()
                                    {
                                        ICON = item.ICON,
                                        PLANTIME_BEGIN = item.PLANTIME_BEGIN,
                                        PLANTIME_END = item.PLANTIME_END
                                    };
                                    if (!planList.ContainsKey(phone))
                                    {
                                        List<JOB_PLAN_ICON> phoneList = new List<JOB_PLAN_ICON>();
                                        phoneList.Add(plan);
                                        planList.Add(phone, phoneList);
                                    }
                                    else
                                    {
                                        planList[phone].Add(plan);
                                    }
                                } 
                                #endregion
                            }
                        }
                    }
                }
                #endregion

                //}
                //catch (Exception e)
                //{

                //    RecordLog.RecordError("人员图标计算Exception：" + e.Message);
                //}
                #region 青藏
                if (sysDept.Equals("15")) 
                {
                    List<JOB_PLAN_ICON> planICONList = oc.BllSession.IJOB_PLANService.Entities
                        .Where(u => !string.IsNullOrEmpty(u.PHONE) && u.PLANTIME_BEGIN >= DateTime.Today)
                        .OrderBy(u => u.PLANTIME_BEGIN).ToList()
                        .Select(u => new JOB_PLAN_ICON
                        {
                            PLAN_TYPE = u.PLAN_TYPE,
                            DEPT_CODE = u.DEPT_CODE,
                            DEPT_CJNAME = u.DEPT_CJNAME,
                            LEADER_PHONE=u.PHONE,
                            PHONE_LIST = u.PHONE_LIST,
                            PLANTIME_BEGIN = u.PLANTIME_BEGIN,
                            PLANTIME_END = u.PLANTIME_END,
                            MILEAGE_BEGIN = u.MILEAGE_BEGIN,
                            MILEAGE_END = u.MILEAGE_END,
                            LINE_NAME = u.LINE_NAME,
                            DIRECTION = u.DIRECTION
                        }).ToList();
                    foreach (JOB_PLAN_ICON planIcon in planICONList)
                    {
                        if (!string.IsNullOrEmpty(planIcon.LEADER_PHONE))
                        {
                            if (!planList.ContainsKey(planIcon.LEADER_PHONE))
                            {
                                List<JOB_PLAN_ICON> phoneList = new List<JOB_PLAN_ICON>();
                                phoneList.Add(planIcon);
                                planList.Add(planIcon.LEADER_PHONE, phoneList);
                            }
                            else
                            {
                                planList[planIcon.LEADER_PHONE].Add(planIcon);
                            }
                        }
                    }
                }
                #endregion 
                Constant.phoneIconPlanList.Clear();
                Constant.phoneIconPlanList = planList;
            }

            watch.Stop();
            
            RecordLog.RecordError("人员图标计算时间：" + watch.ElapsedMilliseconds.ToString());
        } 
        #endregion



        #region 南昌动态计算分组
        public static void GetAllPhoneGroup(string typename)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Dictionary<string, Dictionary<string, string>> groupList = new Dictionary<string, Dictionary<string, string>>();
            DateTime enddate = DateTime.Today.AddDays(1);
            List<string> sgjh = oc.BllSession.IJOB_PLANService.Entities.Where(p => p.PLAN_FLAG != "取消" && p.PLAN_FLAG != "只登记不作业" && p.PLAN_FLAG != "已变更")//正常计划
            .Where(u => ((u.PLANTIME_BEGIN >= DateTime.Today && u.PLANTIME_BEGIN <= enddate) || (u.PLANTIME_END >= DateTime.Today && u.PLANTIME_END <= enddate)))//今日计划
            .Where(p => !string.IsNullOrEmpty(p.PHONE_LIST)).OrderBy(u => u.PLANTIME_BEGIN)//有手机号计划
            .Select(u=>u.PHONE_LIST)
            .ToList();

            #region 计算分级
            if (sgjh.Count > 0)
            {
                Dictionary<string, string> phonenameMap = CONVERT_PLAN_PUBLIC.GetlistPhoneName();//手机姓名
                foreach (string PHONE_LIST in sgjh)
                {
                    string[] listphones = PHONE_LIST.Split('|');//
                    if (listphones.Length > 2 && !string.IsNullOrEmpty(listphones[2]))//如果驻站手机不为空
                    {
                        string[] zzphones = listphones[2].Split(',');
                        for (int m = 0; m < zzphones.Length; m++)
                        {
                            string zzphone = zzphones[m];
                            if (phonenameMap.ContainsKey(zzphone))
                            {
                                zzphone += "|" + phonenameMap[zzphone];
                            }
                            else
                            {
                                zzphone += "|" + zzphone;
                            }

                            if (!groupList.ContainsKey(zzphone))//是否有分组
                            {
                                groupList.Add(zzphone, new Dictionary<string, string>());//创建分组
                            }
                            for (int i = 0; i < listphones.Length; i++)
                            {
                                #region 归类
                                if (i != 2)
                                {
                                    string[] fzrphones = listphones[i].Split(',');
                                    for (int j = 0; j < fzrphones.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(fzrphones[j]))
                                        {
                                            if (!groupList[zzphone].ContainsKey(fzrphones[j]))//是否有手机号
                                            {
                                                if (phonenameMap.ContainsKey(fzrphones[j]))
                                                {
                                                    groupList[zzphone].Add(fzrphones[j], phonenameMap[fzrphones[j]]);//添加
                                                }
                                                else
                                                {
                                                    groupList[zzphone].Add(fzrphones[j], fzrphones[j]);//添加
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            } 

            #endregion

            #region 分组发送
            foreach (KeyValuePair<string, Dictionary<string, string>> item in groupList)
            {
                UrlHttpRequest urlRequest = new UrlHttpRequest();


                #region 一对一分组 
                //string name = "name1=" + item.Key.Split('|')[1];
                //string phone = "phone1=" + item.Key.Split('|')[0];
                //int i = 2;
                //foreach (KeyValuePair<string, string> kv in item.Value)
                //{
                //    name += "&name" + i + "=" + kv.Value;
                //    phone += "&phone" + i + "=" + kv.Key;
                //    i++;
                //} 
                #endregion

                string admin = "admin=SXSH201705261442";
                string name = "&name=" + item.Key.Split('|')[1] + item.Key.Split('|')[0];
                string users = "&users=[{\"imei\":\"" + item.Key.Split('|')[0] + "\",\"name\":\"" + item.Key.Split('|')[1] + "\",\"phone\":\"" + item.Key.Split('|')[0] + "\"}";
                string team = "&team=<" + item.Key.Split('|')[0] +">";
                //users=[{"imei":"867453020081371","name":"","phone":"13932145636"},{"imei":"861038030783516","name":"房3","phone":"18665805273"}]&name=组XXX&team=<13932145636><18665805273><14735114757><15070414607><PC2541973702>

                foreach (KeyValuePair<string, string> kv in item.Value)
                {
                    users += ",{\"imei\":\"" + kv.Key + "\",\"name\":\"" + kv.Value + "\",\"phone\":\"" + kv.Key + "\"}";
                    team += "<" + kv.Key + ">";
                }

                users += "]";
                team += "<SXSH201705261442>";

                string ljurl = ConfigurationManager.AppSettings["NCLJURL"];
                if (string.IsNullOrEmpty(ljurl))
                {
                    //ljurl = "http://223.82.245.209/team/private.php?";
                    ljurl = "http://223.82.245.209/team/make.php";
                }
                string rednum = System.DateTime.Now.ToString("yyyyMMddhhmmssfff");
                //string url = ljurl + name + "&" + phone + "&_=" + rednum;
                string url = admin + name + users + team;
                try
                {
                    //string resultget = urlRequest.GetRequest(url, urlRequest.utf8, "", "");
                    string result = urlRequest.PostRequest(ljurl + "?id=" + rednum, urlRequest.utf8, "", url);
                    List<ModelPhoneGroup> ss = ObjToJson.GetToJson<ModelPhoneGroup>(result);
                    RecordLog.RecordInfo("发送成功:" + url + result);
                }
                catch (Exception e)
                {
                    RecordLog.RecordInfo("发送失败:"+ url + e.Message.ToString());
                }
            } 
            #endregion

            watch.Stop();
            RecordLog.RecordError(typename+"动态分组计算时间：" + watch.ElapsedMilliseconds.ToString());
        }
        #endregion
        #region 南昌固定分组
        public static void GetNCPhoneGroup(string zuname, string userphone, string username, string typename)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            UrlHttpRequest urlRequest = new UrlHttpRequest();


            string admin = "admin=SXSH201705261442";
            string name = "&name=" + zuname;
            string users = "&users=[";
            string team = "&team=";
            //users=[{"imei":"867453020081371","name":"","phone":"13932145636"},{"imei":"861038030783516","name":"房3","phone":"18665805273"}]&name=组XXX&team=<13932145636><18665805273><14735114757><15070414607><PC2541973702>

            string[] userphones = userphone.Split(',');
            string[] usernames = username.Split(',');
            for (int i = 0; i < userphones.Length; i++)
            {
                users += "{\"imei\":\"" + userphones[i] + "\",\"name\":\"" + usernames[i] + "\",\"phone\":\"" + userphones[i] + "\"}";
                if (i != userphones.Length - 1)
                {
                    users += ",";
                }
                team += "<" + userphones[i] + ">";
            }

            users += "]";
            team += "<SXSH201705261442>";

            string ljurl = ConfigurationManager.AppSettings["NCLJURL"];
            if (string.IsNullOrEmpty(ljurl))
            {
                //ljurl = "http://223.82.245.209/team/private.php?";
                ljurl = "http://223.82.245.209/team/make.php";
            }
            string rednum = System.DateTime.Now.ToString("yyyyMMddhhmmssfff");
            string url = admin + name + users + team;
            try
            {
                string result = urlRequest.PostRequest(ljurl + "?id=" + rednum, urlRequest.utf8, "", url);
                List<ModelPhoneGroup> ss = ObjToJson.GetToJson<ModelPhoneGroup>(result);
                RecordLog.RecordInfo("发送成功:" + url + result);
            }
            catch (Exception e)
            {
                RecordLog.RecordInfo("发送失败:" + url + e.Message.ToString());
            }

            watch.Stop();
            RecordLog.RecordError(typename + "计算时间：" + watch.ElapsedMilliseconds.ToString());
        }
        #endregion
    }
    public class ModelPhoneGroup
    {
        public string id { get; set; }
        public string admin { get; set; }
        public string name { get; set; }
        public string team { get; set; }
        public List<PhoneGroup> users { get; set; }
        public string extime { get; set; }
        public string md5 { get; set; }
    }
    public class PhoneGroup
    {
        public string imei { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
    }
}