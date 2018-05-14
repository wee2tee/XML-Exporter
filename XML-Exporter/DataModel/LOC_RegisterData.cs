using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.DataModel
{
    public class LOC_RegisterData
    {
        public int id { get; set; }
        public string token_key { get; set; }
        public string machine_code { get; set; }
        public string comp_name { get; set; }
        public string reg_type { get; set; }
        public string sernum { get; set; }
        public string contact { get; set; }
        public string telnum { get; set; }
        public string email { get; set; }
        public string remark { get; set; }
        public string slip_filename { get; set; }
        public string tax_filename { get; set; }
        //public string status_code { get; set; }
        //public string status { get; set; }
        public DateTime reg_date { get; set; }
        public string reg_time { get; set; }
        public string reg_unixtime { get; set; }
    }
}
