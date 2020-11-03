using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Motorboat : Boat
    {
        public Motorboat(string id, int weight, int speed, int duration, int pos, int HP)
        {
            ID = id;
            Weight = weight;
            Speed = speed;
            Duration = duration;
            XFactor = HP;
            Position = pos;
        }
    }
}
