using SolutionWeb.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Controllers
{
    public class CopyController : Controller
    {
        // GET: Copy

        public ActionResult Index()
        {
            List<EasyUIComBoBoxNode> AreasList = new List<EasyUIComBoBoxNode>();
            List<EasyUIComBoBoxNode> DllList = new List<EasyUIComBoBoxNode>();

            DirectoryInfo theFolder = new DirectoryInfo(HttpRuntime.AppDomainAppPath.Replace("SolutionWeb\\", ""));
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                if (NextFolder.Name.IndexOf("SolutionWeb.Areas.") > -1)
                {
                    AreasList.Add(new EasyUIComBoBoxNode() { id = NextFolder.Name.Replace("SolutionWeb.Areas.", ""), text = NextFolder.Name });
                }
                else if (NextFolder.Name.IndexOf("SolutionWeb.Model.") > -1)
                {
                    DllList.Add(new EasyUIComBoBoxNode() { id = NextFolder.Name.Replace("SolutionWeb.Model.", ""), text = NextFolder.Name });
                }
            }

            var viewModel = new
            {
                extForm = new//扩展类
                {
                    extA = AreasList,
                    extB = DllList
                }
            };
            return View(viewModel);
        }

        public string CopyToArea(string id)
        {

            string path = Path.Combine(HttpRuntime.AppDomainAppPath.Replace("SolutionWeb\\", "SolutionWeb.Areas." + id), "bin\\SolutionWeb.Areas." + id + ".dll");
            string areapath = Path.Combine(HttpRuntime.AppDomainAppPath, "bin\\SolutionWeb.Areas." + id + ".dll");

            string sourceDirName = Path.Combine(HttpRuntime.AppDomainAppPath.Replace("SolutionWeb\\", "SolutionWeb.Areas." + id), "Areas\\" + id + "\\Views");
            string destDirName = Path.Combine(HttpRuntime.AppDomainAppPath, "Areas\\" + id + "\\Views");


            try
            {
                System.IO.File.Copy(path, areapath, true);
                CopyDirectory(sourceDirName, destDirName);
                return "复制成功";
            }
            catch (Exception ex)
            {
                return "复制失败:" + ex.Message.ToString();
            }
            
        }
        public void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                System.IO.File.SetAttributes(destDirName, System.IO.File.GetAttributes(sourceDirName));
            }

            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                if (".scc" != Path.GetExtension(Path.GetFileName(file)))//文件后缀
                {
                    if (System.IO.File.Exists(destDirName + Path.GetFileName(file)))
                    {
                        FileInfo srcfi = new FileInfo(file);
                        FileInfo desfi = new FileInfo(destDirName + Path.GetFileName(file));
                        if (srcfi.LastWriteTime == desfi.LastWriteTime)
                        {//如果修改时间一样则不复制
                            continue;
                        }
                    }
                    System.IO.File.Copy(file, destDirName + Path.GetFileName(file), true);
                    System.IO.File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
        public string CopyToDll(string id)
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath.Replace("SolutionWeb\\", "SolutionWeb.Model." + id), "bin\\Debug\\SolutionWeb.Model." + id + ".dll");
            string binpath = Path.Combine(HttpRuntime.AppDomainAppPath, "bin\\SolutionWeb.Model." + id + ".dll");
            try
            {

                System.IO.File.Copy(path, binpath, true);
                return "复制成功";
            }
            catch (Exception e)
            {

                return "复制失败:" + e.Message.ToString();
            }
        }
    }
}