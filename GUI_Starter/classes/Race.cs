using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Starter.classes
{
    class Race
    {
        public int Index { get; set; }
        public String RaceLabel { get; set; }
        public int Division { get; set; }
        public List<Lane> Lanes {get; set;}

        private String csvString { get; set; }

        public Race(String csvString)
        {
            this.setUpStructure();
            this.parseFromString(csvString);
        }
        public Race()
        {
            this.setUpStructure();
        }

        private void setUpStructure()
        {
            Index = 0;
            RaceLabel = "100";
            Division = 0;
            Lanes = new List<Lane>();
        }
        public void parseFromString(String csvLine)
        {
            List<string> values = csvLine.Split(',').ToList();
            try
            {
                Index = Int32.Parse(values.ElementAt(0));
                RaceLabel = values.ElementAt(1);
                Division = int.Parse(values.ElementAt(2));
                for(int i = 3; i < values.Count; i+=2)
                {
                    try
                    {
                        double delay = double.Parse(values.ElementAt(i + 1), CultureInfo.InvariantCulture);
                        int Number = int.Parse(values.ElementAt(i));
                        Lanes.Add(new Lane(delay, Number));
                    }catch(NullReferenceException e)
                    {
                        Console.WriteLine("Some Data where Currupted \n" + e.Message);
                    }
                }

            }
            catch(FormatException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            this.csvString = csvLine;
        }

        public bool sendSerial()
        {
            Com_Ports port = Com_Ports.Instance;           
            if (port.sendString(this.ToCSVString()))
            {
                Console.WriteLine("Send Success");
                return true;
            }
            else
            {
                Console.WriteLine("Send Failed");
            }
            return false;
        }

        public String ToCSVString()
        {
            String retstr = ""+ this.Index + "," + this.RaceLabel + "," + this.Division;
            for(int i = 0; i < this.Lanes.Count; i++)
            {
                retstr += this.Lanes[i].toCSVString();
            }
            return retstr;
        }
    }
}
