using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMDReplay.Models
{
    public class SwitchConfig
    {
        public SwitchConfig() { }
        public SwitchConfig(int number, bool on)
        {
            Number = number;
            On = on;
        }
        public int Number { get; set; }
        public bool On { get; set; }
    }
}
