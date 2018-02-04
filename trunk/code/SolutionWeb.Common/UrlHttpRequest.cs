using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace SolutionWeb.Common
{

    /// <summary>
    /// 操作HTTP协议类
    /// </summary>
    public class UrlHttpRequest
    {
        public CookieCollection cookie = null;
        public CookieContainer cookies = null;
        public Encoding gb2312 = Encoding.GetEncoding("gb2312");
        public Encoding utf8 = Encoding.GetEncoding("utf-8");

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UrlHttpRequest()
        {
            cookie = new CookieCollection();
            cookies = new CookieContainer();
        }
        #endregion


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="geturl">请求网址</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="referer">跳转网址</param>
        /// <param name="requestStream">无用参数</param>
        /// <returns>返回结果</returns>
        public string GetRequest(string geturl, Encoding encoding, string referer, string requestStream)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(geturl);
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.CookieContainer = cookies;
                request.Referer = referer;
                request.Headers.Set("Accept-Language", "zh-CN");
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
                response = (HttpWebResponse)request.GetResponse();
                cookie = response.Cookies;
                cookies.Add(cookie);
                reader = new StreamReader(response.GetResponseStream(), encoding);
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    try
                    {
                        reader.Close();
                        reader = null;
                    }
                    catch
                    {
                    }
                }
                if (response != null)
                {
                    try
                    {
                        response.Close();
                        response = null;
                    }
                    catch
                    {
                    }
                }
                if (request != null)
                {
                    try
                    {
                        request.Abort();
                        request = null;
                    }
                    catch
                    {
                    }
                }
            }
        }
        #endregion

        #region  POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="postturl">请求网址</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="referer">跳转网址</param>
        /// <param name="requestStream">跳转内容</param>
        /// <returns>返回结果</returns>
        public string PostRequest(string postturl, Encoding encoding, string referer, string requestStream)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(postturl);
                request.CookieContainer = cookies;
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
                request.Referer = referer;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                stream = request.GetRequestStream();
                byte[] bytes = encoding.GetBytes(requestStream);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                response = (HttpWebResponse)request.GetResponse();
                cookie = response.Cookies;
                cookies.Add(cookie);
                reader = new StreamReader(response.GetResponseStream(), encoding);
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    try
                    {
                        reader.Close();
                        reader = null;
                    }
                    catch
                    {
                    }
                }
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                        stream = null;
                    }
                    catch
                    {
                    }
                }
                if (response != null)
                {
                    try
                    {
                        response.Close();
                        response = null;
                    }
                    catch
                    {
                    }
                }
                if (request != null)
                {
                    try
                    {
                        request.Abort();
                        request = null;
                    }
                    catch
                    {
                    }
                }
            }
        }
        #endregion
    }
}
