using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Sailboat : Boat
    {
        public Sailboat(string id, int weight, int speed, int duration, int pos, int length)
        {
            ID = id;
            Weight = weight;
            Speed = speed;
            Duration = duration;
            XFactor = length;
            Position = pos;
        }
    }
}
