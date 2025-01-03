﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data
{
    public class Report
    {
        public string reportID;
        public string location { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public List<string>? attachments = null;

        private string[] statusLabels = { "Pending", "Resolved" };

        public Report() { }

        public Report(string location, string category, string description, List<string> attachments)
        {
            reportID = GenerateID();
            this.location = location;
            this.category = category;
            this.description = description;
            status = 0;

            if (attachments.Count > 0)
            {
                this.attachments = attachments;
            }
        }

        public string GenerateID()
        {
            string output = string.Empty;

            Random random = new Random();

            for(int i = 0; i < 5; i++)
            {
                output += random.Next(9);
            }

            return output;
        }

        public override string ToString()
        {
            return $"{reportID}\t{location}\t{category}\t{description}\t{statusLabels[status]}";
        }

        public void ResolveReport()
        {
            status = 1;
        }

        public string GetStatus()
        {
            return statusLabels[status];
        }
    }
}
