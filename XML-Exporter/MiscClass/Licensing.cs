using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using XML_Exporter.DataModel;
using XML_Exporter.MiscClass;
using XML_Exporter.SubWindow;
using CSGDBDConnectLib;
using Comm;
using Newtonsoft.Json;
using System.Management;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace XML_Exporter.MiscClass
{
    public class Licensing
    {
        //[StructLayout(LayoutKind.Sequential)]
        //public struct SYSTEM_INFO
        //{
        //    public uint dwOemId;
        //    public uint dwPageSize;
        //    public uint lpMinimumApplicationAddress;
        //    public uint lpMaximumApplicationAddress;
        //    public uint dwActiveProcessorMask;
        //    public uint dwNumberOfProcessors;
        //    public uint dwProcessorType;
        //    public uint dwAllocationGranularity;
        //    public uint dwProcessorLevel;
        //    public uint dwProcessorRevision;
        //}

        //[DllImport("kernel32.dll")]
        //public static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

        [Flags]
        public enum FileSystemFeature : uint
        {
            /// <summary>
            /// The file system supports case-sensitive file names.
            /// </summary>
            CaseSensitiveSearch = 1,
            /// <summary>
            /// The file system preserves the case of file names when it places a name on disk.
            /// </summary>
            CasePreservedNames = 2,
            /// <summary>
            /// The file system supports Unicode in file names as they appear on disk.
            /// </summary>
            UnicodeOnDisk = 4,
            /// <summary>
            /// The file system preserves and enforces access control lists (ACL).
            /// </summary>
            PersistentACLS = 8,
            /// <summary>
            /// The file system supports file-based compression.
            /// </summary>
            FileCompression = 0x10,
            /// <summary>
            /// The file system supports disk quotas.
            /// </summary>
            VolumeQuotas = 0x20,
            /// <summary>
            /// The file system supports sparse files.
            /// </summary>
            SupportsSparseFiles = 0x40,
            /// <summary>
            /// The file system supports re-parse points.
            /// </summary>
            SupportsReparsePoints = 0x80,
            /// <summary>
            /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
            /// </summary>
            VolumeIsCompressed = 0x8000,
            /// <summary>
            /// The file system supports object identifiers.
            /// </summary>
            SupportsObjectIDs = 0x10000,
            /// <summary>
            /// The file system supports the Encrypted File System (EFS).
            /// </summary>
            SupportsEncryption = 0x20000,
            /// <summary>
            /// The file system supports named streams.
            /// </summary>
            NamedStreams = 0x40000,
            /// <summary>
            /// The specified volume is read-only.
            /// </summary>
            ReadOnlyVolume = 0x80000,
            /// <summary>
            /// The volume supports a single sequential write.
            /// </summary>
            SequentialWriteOnce = 0x100000,
            /// <summary>
            /// The volume supports transactions.
            /// </summary>
            SupportsTransactions = 0x200000,
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetVolumeInformation(
          string RootPathName,
          StringBuilder VolumeNameBuffer,
          int VolumeNameSize,
          out uint VolumeSerialNumber,
          out uint MaximumComponentLength,
          out FileSystemFeature FileSystemFlags,
          StringBuilder FileSystemNameBuffer,
          int nFileSystemNameSize);



        public enum STATUS : int
        {
            //REGISTER_REQUESTED = 0,
            //REGISTER_CHECKING = 1,
            //REGISTER_APPROVED = 2,
            //REGISTER_UNAPPROVE = 3,
            //CANCELED = 4,
            //UNREGISTER_REQUESTED = 5,
            //UNREGISTER_CHECKING = 6,
            //UNREGISTER_APPROVE = 7,
            //UNREGISTER_UNAPPROVE = 8
            NOT_REGISTER = 1,
            WAIT_FOR_VERIFY = 2,
            REGISTERED = 3
        }

        public static DriveInformation GetDriveInfo()
        {
            //try
            //{
            //    StringBuilder volname = new StringBuilder(261);
            //    StringBuilder fsname = new StringBuilder(261);
            //    uint sernum, maxlen;
            //    FileSystemFeature flags;
            //    if (!GetVolumeInformation("c:\\", volname, volname.Capacity, out sernum, out maxlen, out flags, fsname, fsname.Capacity))
            //        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            //    string volnamestr = volname.ToString();
            //    string fsnamestr = fsname.ToString();

            //    Console.WriteLine(" >>>> volnamestr : " + volnamestr);
            //    Console.WriteLine(" >>>> fsnamestr : " + fsnamestr);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            string start_path = AppDomain.CurrentDomain.BaseDirectory;
            DriveInformation drive = new DriveInformation();

            // In case running app from \\comp_name\dir\exe_file.exe
            if (start_path.StartsWith("\\\\"))
            {
                //int first_sep = start_path.IndexOf("\\", 2);
                drive.isNetworkDrive = true;
                drive.compName = Licensing.GetServerCompName(); //start_path.Substring(2, first_sep - 2);
                if (Licensing.IsLAN() && Licensing.IsServer())
                {
                    drive.correctMachine = true;
                    drive.hddSerialNumber = Licensing.GetHDDSerialNumber(Licensing.GetLocalDriveLetterFromUnc(Licensing.GetUncPath(new FileInfo(@"CL.DLL"))));
                }
                return drive;
            }

            // In case running app from drive(Local, Network)
            string drive_letter = start_path.Substring(0, 3);

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drv in drives)
            {
                string driveName = drv.Name; // C:\, E:\, etc:\
                if (drv.Name == drive_letter)
                {
                    System.IO.DriveType driveType = drv.DriveType;
                    switch (driveType)
                    {
                        case System.IO.DriveType.CDRom:
                            // CD Rom
                            drive.isCdRomDrive = true;
                            break;
                        case System.IO.DriveType.Fixed:
                            // Local Drive
                            drive.isLocalDrive = true;
                            drive.hddSerialNumber = Licensing.GetHDDSerialNumber(drv.Name.Substring(0,1));
                            drive.correctMachine = true;
                            break;
                        case System.IO.DriveType.Network:
                            // Mapped Drive
                            string unc_path = Licensing.GetUncPath(new FileInfo(@"CL.DLL"));
                            drive.isNetworkDrive = true;
                            drive.compName = Licensing.GetServerCompName();
                            if (Licensing.IsLAN() && Licensing.IsServer())
                            {
                                drive.correctMachine = true;
                                drive.hddSerialNumber = Licensing.GetHDDSerialNumber(Licensing.GetLocalDriveLetterFromUnc(Licensing.GetUncPath(new FileInfo(@"CL.DLL"))));
                            }
                            else if (Licensing.IsLAN() && !Licensing.IsServer())
                            {
                                drive.correctMachine = false;
                                drive.hddSerialNumber = (Licensing.GetLocalRegData() != null ? Licensing.GetLocalRegData().machine_code : "");
                            }
                            //ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");
                            //ObjectQuery query = new ObjectQuery("SELECT * From Win32_LogicalDisk Where DeviceID=\"" + drive_letter.Replace("\\", "") + "\"");
                            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                            //foreach (ManagementObject obj in searcher.Get())
                            //{
                            //    if (obj["ProviderName"] != null)
                            //    {
                            //        string path = obj["ProviderName"].ToString();
                            //        int first_sep = path.IndexOf("\\", 2);
                            //        drive.compName = path.Substring(2, first_sep - 2);
                            //    }
                            //}
                            //drive.isNetworkDrive = true;
                            break;
                        case System.IO.DriveType.NoRootDirectory:
                            drive.isNoRootDirectory = true;
                            break;
                        case System.IO.DriveType.Ram:
                            drive.isRamDrive = true;
                            break;
                        case System.IO.DriveType.Removable:
                            // Usually a USB Drive
                            drive.isRemovableDrive = true;
                            break;
                        case System.IO.DriveType.Unknown:
                            drive.isUnknowDrive = true;
                            break;
                    }
                }
            }

            return drive;
        }

        public static string GetSystemDriveLetter()
        {
            string system_drive = string.Empty;
            try
            {
                system_drive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
            }
            catch (Exception ex)
            {
                system_drive = "C:\\";
            }

            return system_drive.Substring(0, 1);
        }

        public static string GetHDDSerialNumber(string drive_letter)
        {
            string hddInfo = string.Empty;
            try
            {
                #region Get installed logical drive
                var logicalDiskId = drive_letter + ":"; //AppDomain.CurrentDomain.BaseDirectory.ToString().Substring(0, 2);
                var deviceId = string.Empty;

                var query = "ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + logicalDiskId + "'} WHERE AssocClass = Win32_LogicalDiskToPartition";
                var queryResults = new ManagementObjectSearcher(query);
                var partitions = queryResults.Get();

                foreach (var partition in partitions)
                {
                    query = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                    queryResults = new ManagementObjectSearcher(query);
                    var drives = queryResults.Get();

                    foreach (var drive in drives)
                    {
                        deviceId = drive["DeviceID"].ToString().Replace("\\", "").Replace(".", "");
                    }

                    ManagementObjectSearcher searcher_diskdrive = new ManagementObjectSearcher("Select * From Win32_PhysicalMedia");
                    foreach (ManagementObject media in searcher_diskdrive.Get())
                    {
                        if (media["Tag"].ToString().Replace("\\", "").Replace(".", "") == deviceId)
                        {
                            hddInfo = media["SerialNumber"].ToString().Trim();
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                return hddInfo;
                #endregion Get installed logical drive
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetHDDSerialNumberByWMIProcess()
        {
            string result = string.Empty;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"c:\windows\system32\wbem\WMIC.exe";
            psi.Arguments = @" diskdrive get serialnumber";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;

            using (Process proc = Process.Start(psi))
            {
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                using (StreamReader reader = proc.StandardOutput)
                {
                    result = reader.ReadToEnd().ToUpper().Replace("SERIALNUMBER", "").Trim();
                }
            }

            return result;
        }

        public static string CalcMD5(string input)
        {
            byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(input);
            byte[] hashedBytes = MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static void CreateTokenKeyDB()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "CL.DLL"))
            {
                SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "CL.DLL");

                try
                {
                    string conn_str = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "CL.DLL;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                    {
                        conn.Open();

                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            // Set pragma user_version
                            cmd.CommandText = @"PRAGMA user_version=1";
                            cmd.ExecuteNonQuery();

                            // Create a new table if not exist
                            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS config(id INTEGER PRIMARY KEY, token_key TEXT, machine_code TEXT, comp_name TEXT, reg_type TEXT, sernum TEXT, contact TEXT, telnum TEXT, email TEXT, remark TEXT, slip_filename TEXT, status_code TEXT, status TEXT, reg_date TEXT, reg_time TEXT, reg_unixtime TEXT)";
                            cmd.ExecuteNonQuery();
                        }

                        conn.ChangePassword("weetee.dev");
                        conn.Close();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static bool WriteTokenKey(string token_key, string machine_code, string comp_name, string reg_type, string sernum, string contact, string telnum, string email, string remark, string slip_filename, string reg_date, string reg_time, string reg_unixtime)
        {
            try
            {
                string conn_str = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "CL.DLL;Version=3;";
                using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                {
                    conn.SetPassword("weetee.dev");
                    conn.Open();

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        //cmd.CommandText = "INSERT OR REPLACE INTO config(id, token_key, machine_code, reg_type, date, time) VALUES(1, '" + token_key + "','" + machine_code + "','" + reg_type + "','" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + "','" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture) + "')";
                        cmd.CommandText = "INSERT OR REPLACE INTO config(id, token_key, machine_code, comp_name, reg_type, sernum, contact, telnum, email, remark, slip_filename, reg_date, reg_time, reg_unixtime) VALUES(1,";
                        cmd.CommandText += "'" + token_key + "',";
                        cmd.CommandText += "'" + machine_code + "',";
                        cmd.CommandText += "'" + comp_name + "',";
                        cmd.CommandText += "'" + reg_type + "',";
                        cmd.CommandText += "'" + sernum + "',";
                        cmd.CommandText += "'" + contact + "',";
                        cmd.CommandText += "'" + telnum + "',";
                        cmd.CommandText += "'" + email + "',";
                        cmd.CommandText += "'" + remark + "',";
                        cmd.CommandText += "'" + slip_filename + "',";
                        //cmd.CommandText += "'" + status_code + "',";
                        //cmd.CommandText += "'" + status + "',";
                        cmd.CommandText += "'" + reg_date + "',";
                        cmd.CommandText += "'" + reg_time + "',";
                        cmd.CommandText += "'" + reg_unixtime + "')";
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static LOC_RegisterData GetLocalRegData()
        {
            LOC_RegisterData local_reg = null;

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "CL.DLL"))
            {
                return local_reg;
            }

            try
            {
                string conn_str = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "CL.DLL;Version=3;";
                using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                {
                    conn.SetPassword("weetee.dev");
                    conn.Open();

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        // Reading data
                        cmd.CommandText = @"Select * From config";
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                local_reg = new LOC_RegisterData();
                                local_reg.token_key = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim();
                                local_reg.machine_code = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim();
                                local_reg.comp_name = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim();
                                local_reg.reg_type = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim();
                                local_reg.sernum = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim();
                                local_reg.contact = reader.IsDBNull(6) ? "" : reader.GetString(6).Trim();
                                local_reg.telnum = reader.IsDBNull(7) ? "" : reader.GetString(7).Trim();
                                local_reg.email = reader.IsDBNull(8) ? "" : reader.GetString(8).Trim();
                                local_reg.remark = reader.IsDBNull(9) ? "" : reader.GetString(9).Trim();
                                local_reg.slip_filename = reader.IsDBNull(10) ? "" : reader.GetString(10).Trim();
                                //local_reg.status_code = reader.IsDBNull(11) ? "" : reader.GetString(11).Trim();
                                //local_reg.status = reader.IsDBNull(12) ? "" : reader.GetString(12).Trim();
                                local_reg.reg_date = reader.IsDBNull(13) ? DateTime.Now.AddYears(50) : reader.GetDateTime(13);
                                local_reg.reg_time = reader.IsDBNull(14) ? "" : reader.GetString(14).Trim();
                                local_reg.reg_unixtime = reader.IsDBNull(15) ? "" : reader.GetString(15).Trim();
                            }
                        }
                    }
                    conn.Close();
                }

            }
            catch (SQLiteException ex)
            {
                // do nothing.
            }
            return local_reg;
        }

        public static SRV_RegisterData GetSrvRegisterData(LOC_RegisterData local_regdata)
        {
            if (local_regdata == null)
                return null;

            SRV_RegisterData srv_regdata = new SRV_RegisterData();
            srv_regdata._conn_success = false;
            srv_regdata._failed_reason = SRV_RegisterData.FAILED_REASON.NONE;

            try
            {
                string json_data = "{\"token_key\":\"" + local_regdata.token_key + "\"}";
                string result = WebComm.PostData("http://www.esg.co.th/XML-Exporter/get_registered_data.php", json_data);
                srv_regdata = JsonConvert.DeserializeObject<SRV_RegisterData>(result);
            }
            catch (Exception)
            {
                return null;
            }

            return srv_regdata;
        }

        public static SRV_RegisterData GetSrvRegisterData(string sernum, string hdd_serial)
        {
            if (sernum.Trim().Length < 12)
            {
                return null;
            }

            //SRV_RegisterData srv_regdata = new SRV_RegisterData();
            //srv_regdata._conn_success = false;
            //srv_regdata._failed_reason = SRV_RegisterData.FAILED_REASON.NONE;

            SRV_RegisterData srv_regdata = null;

            try
            {
                string json_data = "{\"sernum\":\"" + sernum.Trim() + "\",";
                json_data += "\"hdd_serial\":\"" + hdd_serial.Trim() + "\"}";
                string result = WebComm.PostData("http://www.esg.co.th/XML-Exporter/get_registered_data.php", json_data);
                srv_regdata = JsonConvert.DeserializeObject<SRV_RegisterData>(result);
            }
            catch (Exception)
            {
                return null;
            }

            return srv_regdata;
        }

        public static bool CheckLicense()
        {
            if (!Licensing.IsServerConnected())
            {
                MessageBox.Show("กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต");
                return false;
            }

            SRV_RegisterData srv_regdata = Licensing.GetSrvRegisterData(Licensing.GetLocalRegData());
            
            if (srv_regdata == null)
            {
                return false;
            }
            else
            {
                if (Licensing.IsLAN())
                {
                    //MessageBox.Show("Licensing.GetSErverCompName() : " + Licensing.GetServerCompName() + "\nsrv_regdata.comp_name : " + srv_regdata.comp_name);

                    if (Licensing.GetServerCompName() == srv_regdata.comp_name && srv_regdata.reg_type == "LAN")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string drive_letter = AppDomain.CurrentDomain.BaseDirectory.Substring(0, 1).ToUpper();
                    if (Licensing.GetHDDSerialNumber(drive_letter) == srv_regdata.machine_code && srv_regdata.reg_type == "LOCAL")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsServerConnected()
        {
            WebResult res = new WebResult();
            res.result = false;

            try
            {
                string web_result = WebComm.PostData("http://www.esg.co.th/XML-Exporter/test_connection.php", "");
                res = JsonConvert.DeserializeObject<WebResult>(web_result);
            }
            catch (Exception)
            {
                // do nothing.
            }

            return res.result;
        }

        public static bool IsLAN()
        {
            if (!File.Exists(@"CL.DLL"))
            {
                MessageBox.Show("ค้นหาไฟล์ CL.DLL ไม่พบ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string unc_path = Licensing.GetUncPath(new FileInfo(@"CL.DLL"));
            return unc_path.StartsWith(@"\\");
        }

        public static bool IsServer()
        {
            if (Licensing.GetCurrentCompName() == Licensing.GetServerCompName())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetCurrentCompName()
        {
            return System.Environment.MachineName.ToLower();
        }

        public static string GetServerCompName()
        {
            string srv_name = string.Empty;
            
            if(!File.Exists(@"CL.DLL"))
                return srv_name;

            string unc_path = Licensing.GetUncPath(new FileInfo(@"CL.DLL"));

            if (Licensing.IsLAN())
            {
                int first_sep = unc_path.IndexOf("\\", 2); //start_path.IndexOf("\\", 2);
                srv_name = unc_path.Substring(2, first_sep - 2);
            }
            else
            {
                srv_name = Licensing.GetCurrentCompName();
            }

            return srv_name.ToLower();
        }

        public static string GetUncPath(FileInfo fileInfo)
        {
            string filePath = fileInfo.FullName;

            if (filePath.StartsWith(@"\\"))
                return filePath;

            if (new DriveInfo(Path.GetPathRoot(filePath)).DriveType != DriveType.Network)
                return filePath;

            string drivePrefix = Path.GetPathRoot(filePath).Substring(0, 2);
            string uncRoot;

            using (var managementObject = new ManagementObject())
            {
                var managementPath = string.Format("Win32_LogicalDisk='{0}'", drivePrefix);
                managementObject.Path = new ManagementPath(managementPath);
                uncRoot = (string)managementObject["ProviderName"];
            }

            return filePath.Replace(drivePrefix, uncRoot);
        }

        public static string GetLocalDriveLetterFromUnc(string uncPath)
        {
            try
            {
                // remove the "\\" from the UNC path and split the path
                uncPath = uncPath.Replace(@"\\", "");
                string[] uncParts = uncPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (uncParts.Length < 2)
                    return "[UNRESOLVED UNC PATH: " + uncPath + "]";
                // Get a connection to the server as found in the UNC path
                ManagementScope scope = new ManagementScope(@"\\" + uncParts[0] + @"\root\cimv2");
                // Query the server for the share name
                SelectQuery query = new SelectQuery("Select * From Win32_Share Where Name = '" + uncParts[1] + "'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                // Get the path
                string path = string.Empty;
                foreach (ManagementObject obj in searcher.Get())
                {
                    path = obj["path"].ToString();
                }

                // Append any additional folders to the local path name
                if (uncParts.Length > 2)
                {
                    for (int i = 2; i < uncParts.Length; i++)
                        path = path.EndsWith(@"\") ? path + uncParts[i] : path + @"\" + uncParts[i];
                }

                return path.Substring(0,1);
            }
            catch (Exception ex)
            {
                //return "[ERROR RESOLVING UNC PATH: " + uncPath + ": " + ex.Message + "]";
                return string.Empty;
            }
        }
    }

    public static class LicensingHelper
    {
        public static string ToStatusString(this Licensing.STATUS status_code)
        {
            switch (status_code)
            {
                case Licensing.STATUS.NOT_REGISTER:
                    return "ยังไม่ได้ลงทะเบียนโปรแกรม";
                case Licensing.STATUS.WAIT_FOR_VERIFY:
                    return "อยู่ระหว่างการตรวจสอบข้อมูลการลงทะเบียน";
                case Licensing.STATUS.REGISTERED:
                    return "ลงทะเบียนโปรแกรมเรียบร้อยแล้ว";
                default:
                    return "";
            }
        }
    }
}
