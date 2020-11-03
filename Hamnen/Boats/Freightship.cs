using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Freightship : Boat
    {
        public Freightship(string id, int weight, int speed, int duration,  int pos, int containers)
        {
            ID = id;
            Weight = weight;
            Speed = speed;
            Duration = duration;
            XFactor = containers;
            Position = pos;
        }
    }
}
