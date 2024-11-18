using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data.BST
{
    public class ReportBST
    {
        private ReportNode root;
        private int count;

        public int Count
        {
            get { return count; }
        }

        public ReportBST()
        {
            count = 0;
            root = null;
        }

        // Insert a new Report into the BST
        public void Insert(Report report)
        {
            root = InsertRecursive(root, report);
            count++;
        }

        private ReportNode InsertRecursive(ReportNode node, Report report)
        {
            if (node == null)
                return new ReportNode(report);

            int reportID = int.Parse(report.reportID);
            int nodeID = int.Parse(node.Data.reportID);

            if (reportID < nodeID)
                node.Left = InsertRecursive(node.Left, report);
            else if (reportID > nodeID)
                node.Right = InsertRecursive(node.Right, report);

            return node;
        }

        // Search for a Report by ID
        public Report Search(string reportID)
        {
            return SearchRecursive(root, int.Parse(reportID));
        }

        private Report SearchRecursive(ReportNode node, int reportID)
        {
            if (node == null) return null;
            int nodeID = int.Parse(node.Data.reportID);

            if (reportID == nodeID) return node.Data;
            if (reportID < nodeID) return SearchRecursive(node.Left, reportID);
            return SearchRecursive(node.Right, reportID);
        }

        // In-order traversal to display reports in ascending order by reportID
        public List<Report> GetReportsInOrder()
        {
            return GetOrderedRecursive(root);
        }

        private List<Report> GetOrderedRecursive(ReportNode node)
        {
            List<Report> result = new List<Report>();

            if (node != null)
            {
                result.AddRange(GetOrderedRecursive(node.Left));
                result.Add(node.Data);
                result.AddRange(GetOrderedRecursive(node.Right));
            }

            return result;
        }

        // Method to remove a report based on its reportID
        public void Remove(string reportID)
        {
            root = RemoveRecursive(root, reportID);
            count--;
        }

        private ReportNode RemoveRecursive(ReportNode node, string reportID)
        {
            if (node == null)
                return null;

            int reportIDInt = int.Parse(reportID);
            int nodeIDInt = int.Parse(node.Data.reportID);

            // Traverse the tree to find the node
            if (reportIDInt < nodeIDInt)
            {
                node.Left = RemoveRecursive(node.Left, reportID);
            } else if (reportIDInt > nodeIDInt)
            {
                node.Right = RemoveRecursive(node.Right, reportID);
            } else // Node to be deleted is found
            {
                //Node has no children
                if (node.Left == null && node.Right == null)
                {
                    return null;
                }
                //Node has only one child
                else if (node.Left == null)
                {
                    return node.Right;
                } else if (node.Right == null)
                {
                    return node.Left;
                }
                  //Node has two children
                  else
                {
                    // Find the smallest in the right subtree
                    ReportNode successor = FindMin(node.Right);
                    node.Data = successor.Data; // Replace node's data with successor's data
                    node.Right = RemoveRecursive(node.Right, successor.Data.reportID); // Delete the successor
                }
            }

            return node;
        }

        // Helper function to find the node with the minimum value
        private ReportNode FindMin(ReportNode node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }
    }
}
