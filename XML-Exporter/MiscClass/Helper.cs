using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XML_Exporter.SubWindow;

namespace XML_Exporter.MiscClass
{
    public static class Helper
    {
        //public static string ToStatusString(this RegisterDialog.STATUS status)
        //{
        //    switch (status)
        //    {
        //        case RegisterDialog.STATUS.REGISTER_REQUESTED:
        //            return "ขอรับรหัสการทำรายการแล้ว";
        //        case RegisterDialog.STATUS.REGISTER_CHECKING:
        //            return "กำลังทำการตรวจสอบข้อมูลการลงทะเบียน";
        //        case RegisterDialog.STATUS.REGISTER_APPROVED:
        //            return "ส่งรหัสลงทะเบียนให้ทางอีเมล์แล้ว";
        //        case RegisterDialog.STATUS.REGISTER_UNAPPROVE:
        //            return "ไม่ผ่านการตรวจสอบข้อมูลการลงทะเบียน";
        //        case RegisterDialog.STATUS.CANCELED:
        //            return "ยกเลิกการขอรับรหัสลงทะเบียน";
        //        case RegisterDialog.STATUS.UNREGISTER_REQUESTED:
        //            return "ขอถอนการลงทะเบียน";
        //        case RegisterDialog.STATUS.UNREGISTER_CHECKING:
        //            return "กำลังตรวจสอบข้อมูลเพื่อถอนการลงทะเบียน";
        //        case RegisterDialog.STATUS.UNREGISTER_APPROVE:
        //            return "ถอนการลงทะเบียนแล้ว";
        //        case RegisterDialog.STATUS.UNREGISTER_UNAPPROVE:
        //            return "ไม่ผ่านการตรวจสอบข้อมูลเพื่อถอนการลงทะเบียน";
        //        default:
        //            return "";
        //    }
        //}

        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        public static string RemoveBracketFromPath(this string comp_path)
        {
            string path = comp_path;

            int last_backslash = comp_path.LastIndexOf("\\");
            //Console.WriteLine(" >>> last_backslash : " + last_backslash);

            int last_open_bracket = comp_path.LastIndexOf("(");
            //Console.WriteLine(" >>> last_open_bracket : " + last_open_bracket);

            if (last_open_bracket > -1 && last_open_bracket > last_backslash)
            {
                path = comp_path.Substring(0, comp_path.Substring(last_open_bracket).Length - 1);
            }

            //Console.WriteLine(" ..... " + path);
            return path.Trim();
        }
    }
}
