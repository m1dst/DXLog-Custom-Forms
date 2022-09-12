using System;
using System.Collections.Generic;
using System.Text;

namespace DxLogStationMaster
{
    public class Antenna  : IEquatable<Antenna>
    {
        public bool IsReceiveOnly { get; set; }
        public string AntennaName { get; set; }
        public string AntennaLabel { get; set; }

        public override bool Equals(object other)
        {
            return Equals(other as Antenna);
        }

        public bool Equals(Antenna otherItem)
        {
            if (otherItem == null)
            {
                return false;
            }
            return otherItem.AntennaName == AntennaName && otherItem.AntennaLabel == AntennaLabel;
        }

        public override int GetHashCode()
        {
            int hash = 19;
            hash = hash * 31 + (AntennaName == null ? 0 : AntennaName.GetHashCode());
            hash = hash * 31 + AntennaLabel.GetHashCode();
            return hash;
        }
    }
}
