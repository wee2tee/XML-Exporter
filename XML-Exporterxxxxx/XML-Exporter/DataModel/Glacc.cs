﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.DataModel
{
    public class Glacc
    {
        public string accnum { get; set; }
        public string accnam { get; set; }
        public string accnam2 { get; set; }
        public int level { get; set; }
        public string parent { get; set; }
        public string group { get; set; }
        public string acctyp { get; set; }
        public string usedep { get; set; }
        public string usejob { get; set; }
        public string nature { get; set; }
        public string consol { get; set; }
        public string status { get; set; }
        public string creby { get; set; }
        public DateTime credat { get; set; } // date value
        public string userid { get; set; }
        public DateTime chgdat { get; set; } // date value
    }
}
