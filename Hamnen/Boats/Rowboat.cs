using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Rowboat : Boat
    {
        public Rowboat(string id, int weight, int speed, int duration, int pos, int maxpass)
        {
            ID = id;
            Weight = weight;
            Speed = speed;
            Duration = duration;
            XFactor = maxpass;
            Position = pos;
        }
    }
}
