using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Catamaran : Boat
    {
        public Catamaran(string id, int weight, int speed, int duration, int pos, int beds)
        {
            ID = id;
            Weight = weight;
            Speed = speed;
            Duration = duration;
            XFactor = beds;
            Position = pos;
        }
    }
}
