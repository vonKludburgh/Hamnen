using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Pier
    {
        public int TotalWeight { get; set; }
        public int AverageSpeed { get; set; }
        public double[] Space { get; set; }
        public int Rejected { get; set; }
        public string Message { get; set; }
        public List<Boat> BoatsAtPier = new List<Boat>();
        public List<Boat> BoatsInPier = new List<Boat>();
        public Pier()
        {
            Rejected = 0;
        }
    }
}
