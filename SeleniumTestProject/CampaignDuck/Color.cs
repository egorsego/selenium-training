using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignDuck
{
    class Color
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public Color(int[] colorArr)
        {
            Red = colorArr[0];
            Green = colorArr[1];
            Blue = colorArr[2];
        }

        public bool IsGrey()
        {
            return Red == Green && Green == Blue;
        }

        public bool IsRed()
        {
            return Red > 0 && Green == 0 && Blue == 0;
        }

        public bool AreEqualColors(Color a, Color b)
        {
            return a.Red == b.Red && a.Green == b.Green && a.Blue == b.Blue;
        }
    }

}
