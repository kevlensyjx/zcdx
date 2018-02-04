using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionWeb.Common
{
    public class ViewModel
    {
        public int total { get; set; }
        public object rows { get; set; }
    }
    public class ViewEditModel
    {
        public int index { get; set; }
        public object row { get; set; }
    }
    public class ViewModelLayui
    {
        public string code { get; set; }
        public string msg { get; set; }
        public int total { get; set; }
        public object rows { get; set; }
    }
}
