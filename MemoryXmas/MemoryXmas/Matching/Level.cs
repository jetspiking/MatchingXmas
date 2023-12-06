using MatchingXmas.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingXmas.Matching
{
    public class Level
    {
        public Images Image { get; set; }
        public Sounds Soundtrack { get; set; }
        public Int32 ToMatch { get; set; }

        public Level(Images image, Sounds soundtrack, int toMatch)
        {
            Image = image;
            Soundtrack = soundtrack;
            ToMatch = toMatch;
        }
    }
}
