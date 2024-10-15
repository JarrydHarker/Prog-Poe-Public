using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data.Event
{
    public class Event
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public float Fee { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public string Venue { get; set; }

        public override string ToString()
        {
            string feeString = Fee > 0 ? $"R{Fee.ToString("F2")}" : "Free";

            return $"{Name}\t{Category}\t{Description}\t{feeString}\t{Date}\t{Time}\t{Venue}";
        }
    }
}
