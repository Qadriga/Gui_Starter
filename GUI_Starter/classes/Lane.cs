using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Starter.classes
{
    class Lane
    {
        public double delay {get; set;} = 0;
        public int LaneNumber { get; set; } = 1;
        public Lane()
        {
        }
        public Lane(double time_delay , int Lane)
        {
            this.delay = time_delay;
            this.LaneNumber = Lane;
        }

        public string toCSVString()
        {
            return "," + this.LaneNumber + "," + this.delay ;
        }       
    }
}
