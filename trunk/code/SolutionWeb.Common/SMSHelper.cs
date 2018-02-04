using SolutionWeb.Common.com.ums86.api.sms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionWeb.Common
{
    public class SMSHelper
    {
        public bool SendSMS(string msg, string phones)
        {
            Sms sms = new Sms();
            string result = sms.CallSms(ConfigurationManager.AppSettings["SMS_ID"], ConfigurationManager.AppSettings["SMS_USER"], ConfigurationManager.AppSettings["SMS_PASS"], msg, phones, DateTime.Now.ToString("yyyyMMddHHmmssffff"), null, "1", null, null, null);

            if (!result.StartsWith("0"))
            {
                RecordLog.RecordError(string.Format("手机号：{0},内容：{1}发送失败", phones, msg));
                return false;
            }
            return true;
        }
    }
}
