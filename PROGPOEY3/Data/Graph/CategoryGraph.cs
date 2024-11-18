using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data.Graph
{
    public class CategoryGraph
    {
        private Dictionary<string, CategoryNode> categories = new Dictionary<string, CategoryNode>();

        public CategoryGraph()
        {
            AddCategories();
        }

        public void AddCategories()
        {
            foreach (string category in IssueCategories.Categories)
            {
                categories[category] = new CategoryNode(category);
            }
        }

        public void AddReportToCategory(string category, Report report)
        {
            if (categories.TryGetValue(category, out var categoryNode))
            {
                categoryNode.AddReport(report);
            }
        }

        public void AddCategoryRelation(string category1, string category2)
        {
            if (categories.TryGetValue(category1, out var catNode1) &&
                categories.TryGetValue(category2, out var catNode2))
            {
                catNode1.AddRelatedCategory(catNode2);
                catNode2.AddRelatedCategory(catNode1);
            }
        }

        //Fnding related reports in connected categories using BFS
        public List<Report> GetRelatedReports(string category)
        {
            if (!categories.ContainsKey(category)) return new List<Report>();

            var result = new List<Report>();
            var queue = new Queue<CategoryNode>();
            var visited = new HashSet<string> { category };
            queue.Enqueue(categories[category]);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.AddRange(current.Reports);

                foreach (var neighbor in current.RelatedCategories)
                {
                    if (!visited.Contains(neighbor.Category))
                    {
                        visited.Add(neighbor.Category);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        public void RemoveReport(string reportID)
        {
            foreach (var category in categories.Values)
            {
                // Find the report in the category
                var reportToRemove = category.Reports.FirstOrDefault(r => r.reportID == reportID);

                if (reportToRemove != null)
                {
                    // Remove the report from the list
                    category.Reports.Remove(reportToRemove);
                }
            }
        }
    }
}
