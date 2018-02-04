using SolutionWeb.Common;
using SolutionWeb.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace SolutionWeb.Areas.COMPANY.Controllers
{
    public class TestController : BaseController
    {
        #region Identity
        private IBaseService bs = null;
        public TestController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion
        // GET: COMPANY/Test
        public ActionResult Index()
        {
            return View();
        }
    }

    public class TestApiController : BaseApiController
    {
        private static List<Infomation> dataList = new List<Infomation>();
        #region Ioc
        private IBaseService bs = null;
        public TestApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        [HttpPost]
        public async Task<int> UploadFiles()
        {
            string root = "D://work//zcdx//trunk//code//SolutionWeb//Content//Upload//";//指定要将文件存入的服务器物理位置
            var provider = new MultipartFormDataStreamProvider(root);
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var item in provider.FileData)
            {
                string fileName = item.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }
                string fileAble = Path.GetExtension(fileName).ToLower();//文件后缀
                var newname = Guid.NewGuid().ToString();
                string savepath = root + newname + fileAble;
                string svaedbpath = "/Content/Upload/" + newname + fileAble;
                File.Copy(item.LocalFileName, savepath);
                File.Delete(item.LocalFileName);
            }
            return 0;
        }
        
        [System.Web.Http.HttpPost]
        public async Task<Response> GetInfoList(Info info)
        {
            Response re = new Response();
            if(dataList.Count == 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    dataList.Add(new Infomation
                    {
                        id = Guid.NewGuid().ToString(),
                        name = Guid.NewGuid().ToString(),
                        index = i.ToString()
                    });

                }
            }
            re.code = "0";
            re.count = dataList.Count;
            re.data = dataList.Skip((info.page - 1) * info.limit).Take(info.limit).ToList();
            re.msg = "";
            return re;
        }
    }
    public class Info
    {
        public int page { get; set; }
        public int limit { get; set; }
    }
    public class Infomation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string index { get; set; }
    }

    public class Response
    {
        public string code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public List<Infomation> data { get; set; }
    }
}
