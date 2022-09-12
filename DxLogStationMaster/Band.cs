using System;
using System.Collections.Generic;
using System.Text;

namespace DxLogStationMaster
{
    public class Band
    {

        public Band()
        {
            Antennas = new Antenna[0];
        }

        public string Name { get; set; }
        public int StartFrequency { get; set; }
        public int EndFrequency { get; set; }

        public Antenna[] Antennas { get; set; }
    }
}
