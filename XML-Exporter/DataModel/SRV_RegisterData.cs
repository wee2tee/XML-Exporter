using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.DataModel
{
    public class SRV_RegisterData
    {
        public enum FAILED_REASON : int
        {
            NONE = 0,
            DUPLICATE_SN = 1
        }

        public bool _conn_success { get; set; }
        public FAILED_REASON _failed_reason { get; set; }
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
        public string status_code { get; set; }
        public string status { get; set; }
        public string reg_date { get; set; }
        public string reg_time { get; set; }
        public string reg_unixtime { get; set; }
    }

    public static class SRV_RegisterDataHelper
    {
        public static string ToReasonString(this SRV_RegisterData.FAILED_REASON failed_reason)
        {
            switch (failed_reason)
            {
                case SRV_RegisterData.FAILED_REASON.NONE:
                    return "";
                case SRV_RegisterData.FAILED_REASON.DUPLICATE_SN:
                    return "หมายเลข Serial Number นี้เคยลงทะเบียนโปรแกรมไว้แล้ว";
                default:
                    return "";
            }
        }
    }
}
