using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.DataModel
{
    public class Gljnlit
    {
        public string voucher { get; set; }
        public string seqit { get; set; }
        public DateTime voudat { get; set; } // date value
        public string accnum { get; set; }
        public string depcod { get; set; }
        public string jobcod { get; set; }
        public string phase { get; set; }
        public string coscod { get; set; }
        public string descrp { get; set; }
        public string trntyp { get; set; }
        public double amount { get; set; }
        public DateTime chgdat { get; set; } // date value
        public string chgtim { get; set; }
        public string adjust { get; set; }
        public string chgaccfrom { get; set; }
    }
}
