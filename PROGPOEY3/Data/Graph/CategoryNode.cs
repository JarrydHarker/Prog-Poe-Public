using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data.Graph
{
    public class CategoryNode
    {
        public string Category { get; }
        public List<Report> Reports { get; } = new List<Report>();
        public List<CategoryNode> RelatedCategories { get; } = new List<CategoryNode>();

        public CategoryNode(string category)
        {
            Category = category;
        }

        public void AddRelatedCategory(CategoryNode categoryNode)
        {
            RelatedCategories.Add(categoryNode);
        }

        public void AddReport(Report report)
        {
            Reports.Add(report);
        }
    }
}
