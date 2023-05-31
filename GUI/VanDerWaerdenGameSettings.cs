using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class VanDerWaerdenGameSettings : ConfigurationSection
    {
        [ConfigurationProperty("N", DefaultValue = 50)]
        public int N
        {
            get { return (int)this["N"]; }
            set { this["N"] = value; }
        }

        [ConfigurationProperty("K", DefaultValue = 5)]
        public int K
        {
            get { return (int)this["K"]; }
            set { this["K"] = value; }
        }

        [ConfigurationProperty("Player1", DefaultValue = "UCT")]
        public string Player1
        {
            get { return (string)this["Player1"]; }
            set { this["Player1"] = value; }
        }

        [ConfigurationProperty("Player2", DefaultValue = "UCT")]
        public string Player2
        {
            get { return (string)this["Player2"]; }
            set { this["Player2"] = value; }
        }
    }
}
