using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamnen
{
    class Boat : IEqualityComparer<Boat>
    {
        public string ID { get; set; }
        public int Weight { get; set; }
        public int Speed { get; set; }
        public int Duration { get; set; }
        public int Position { get; set; }
        public int XFactor { get; set; }

        public string Path { get; set; }
        public string posAdd { get; set; }
        public string xAdd { get; set; }

        public bool Equals(Boat x, Boat y)
        {
            return x.Position == y.Position;
        }

        public int GetHashCode(Boat obj)
        {
            return obj.Position.GetHashCode() - obj.ID.GetHashCode() - obj.XFactor.GetHashCode();
        }
    }
}
