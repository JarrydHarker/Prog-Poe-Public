using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data.BST
{
    public class ReportNode
    {
        public Report Data { get; set; }
        public ReportNode Left { get; set; }
        public ReportNode Right { get; set; }

        public ReportNode(Report report)
        {
            Data = report;
            Left = null;
            Right = null;
        }
    }
}
