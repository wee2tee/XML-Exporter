using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using XML_Exporter.HWModel;
using XML_Exporter.MiscClass;
using XML_Exporter.DataModel;
using System.Collections.Specialized;
using System.Net;
using WebAPI;
using WebAPI.ApiResult;
using Newtonsoft.Json;
using Comm;

namespace XML_Exporter.SubWindow
{
    public partial class RegisterDialog : Form
    {
        
        public XML_Exporter.MiscClass.Licensing.STATUS register_status;
        private ArrayList hdCollection = new ArrayList();
        //private MainForm main_form;

        private string sernum = string.Empty;
        private string contact = string.Empty;
        private string email = string.Empty;
        private string telnum = string.Empty;
        private string slip_file_path = string.Empty;
        private string tax_file_path = string.Empty;
        //private byte[] image_data = null;
        private string remark = string.Empty;
        private SRV_RegisterData srv_regdata = null;

        public RegisterDialog()
        {
            InitializeComponent();
            //this.main_form = main_form;
        }

        private void RegisterDialog_Load(object sender, EventArgs e)
        {
            this.BindingControlEventHandler();
            DriveInformation drv = Licensing.GetDriveInfo();

            this.GetSrvRegData();
            this.lblSN.Text = RegisterDialog.ReadSernum();
            this.lblMachineCode.Text = Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter());
        }

        private void RegisterDialog_Shown(object sender, EventArgs e)
        {
            this.RepaintStatusPanel();
            this.SetControlState();
        }

        private void BindingControlEventHandler()
        {
            this.txtContact.TextChanged += new EventHandler(this.FormFieldTextChange);
            this.txtEmail.TextChanged += new EventHandler(this.FormFieldTextChange);
            this.txtTelnum.TextChanged += new EventHandler(this.FormFieldTextChange);
            this.txtSlip.TextChanged += new EventHandler(this.FormFieldTextChange);
            this.txtTax.TextChanged += new EventHandler(this.FormFieldTextChange);
            this.lblSN.TextChanged += delegate
            {
                this.sernum = this.lblSN.Text;
            };
        }

        private void RepaintStatusPanel()
        {
            this.lblStatus.Text = this.register_status.ToStatusString();

            switch (this.register_status)
            {
                case Licensing.STATUS.NOT_REGISTER:
                    this.panelStatus.BackColor = Color.FromArgb(192, 192, 255);
                    break;
                case Licensing.STATUS.WAIT_FOR_VERIFY:
                    this.panelStatus.BackColor = Color.FromArgb(255, 255, 128);
                    break;
                case Licensing.STATUS.REGISTERED:
                    this.panelStatus.BackColor = Color.FromArgb(0, 192, 0);
                    break;
                default:
                    this.panelStatus.BackColor = Color.FromArgb(192, 192, 255);
                    break;
            }
        }

        private void SetControlState()
        {
            if (this.register_status == Licensing.STATUS.NOT_REGISTER)
            {
                this.btnOK.Visible = true;
                this.btnCancel.Visible = true;
                this.btnClose.Visible = false;

                this.txtContact.Enabled = true;
                this.txtContact.Text = "";
                this.txtTelnum.Enabled = true;
                this.txtTelnum.Text = "";
                this.txtEmail.Enabled = true;
                this.txtEmail.Text = "";
                this.txtSlip.Enabled = true;
                this.txtSlip.Text = "";
                this.btnBrowseSlip.Enabled = true;
                this.txtTax.Enabled = true;
                this.txtTax.Text = "";
                this.btnBrowseTax.Enabled = true;
                this.txtRemark.Enabled = true;
                this.txtRemark.Text = "";
                this.lblBottomText1.Visible = true;
                this.lblBottomText2.Visible = true;

                return;
            }

            if (this.register_status == Licensing.STATUS.WAIT_FOR_VERIFY)
            {
                this.btnOK.Visible = false;
                this.btnCancel.Visible = false;
                this.btnClose.Visible = true;

                this.txtContact.Enabled = false;
                this.txtContact.Text = this.srv_regdata.contact;
                this.txtTelnum.Enabled = false;
                this.txtTelnum.Text = this.srv_regdata.telnum;
                this.txtEmail.Enabled = false;
                this.txtEmail.Text = this.srv_regdata.email;
                this.txtSlip.Enabled = false;
                this.btnBrowseSlip.Enabled = false;
                this.txtTax.Enabled = false;
                this.btnBrowseTax.Enabled = false;
                this.txtRemark.Enabled = false;
                this.lblBottomText1.Visible = true;
                this.lblBottomText2.Visible = true;

                return;
            }

            if (this.register_status == Licensing.STATUS.REGISTERED)
            {
                this.btnOK.Visible = false;
                this.btnCancel.Visible = false;
                this.btnClose.Visible = true;

                this.txtContact.Enabled = false;
                this.txtContact.Text = this.srv_regdata.contact;
                this.txtTelnum.Enabled = false;
                this.txtTelnum.Text = this.srv_regdata.telnum;
                this.txtEmail.Enabled = false;
                this.txtEmail.Text = this.srv_regdata.email;
                this.txtSlip.Enabled = false;
                this.btnBrowseSlip.Enabled = false;
                this.txtTax.Enabled = false;
                this.btnBrowseTax.Enabled = false;
                this.txtRemark.Enabled = false;
                this.lblBottomText1.Visible = false;
                this.lblBottomText2.Visible = false;

                return;
            }
        }

        public static string ReadSernum()
        {
            string serial = string.Empty;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "..\\serial.txt"))
            {
                foreach (string line in File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + "..\\serial.txt", Encoding.GetEncoding("windows-874")))
                {
                    serial = line;
                    break;
                }
            }

            return serial;
        }

        private void FormFieldTextChange(object sender, EventArgs e)
        {
            if ((TextBox)sender == this.txtContact)
                this.contact = ((TextBox)sender).Text.Trim();

            if ((TextBox)sender == this.txtEmail)
                this.email = ((TextBox)sender).Text.Trim();

            if ((TextBox)sender == this.txtTelnum)
                this.telnum = ((TextBox)sender).Text.Trim();

            if ((TextBox)sender == this.txtRemark)
                this.remark = ((TextBox)sender).Text.Trim();

            if ((TextBox)sender == this.txtSlip)
                this.slip_file_path = ((TextBox)sender).Text.Trim();

            if ((TextBox)sender == this.txtTax)
                this.tax_file_path = ((TextBox)sender).Text.Trim();

            if (this.txtContact.Text.Trim().Length > 0 && this.txtEmail.Text.Trim().Length > 0 && this.txtTelnum.Text.Trim().Length > 0 && this.txtSlip.Text.Trim().Length > 0 && this.txtTax.Text.Trim().Length > 0)
            {
                this.btnOK.Enabled = true;
            }
            else
            {
                this.btnOK.Enabled = false;
            }
        }

        //public static string GetMC1(string HDD_Serial)
        //{
        //    string pre_mc1 = string.Empty;
        //    foreach (char c in HDD_Serial.ToCharArray())
        //    {
        //        Console.WriteLine(" >> " + c.ToString() + " => " + (int)c);
        //        pre_mc1 += ((int)c).ToString();
        //    }
        //    pre_mc1 = pre_mc1.Substring(1, pre_mc1.Length - 1) + pre_mc1.Substring(0, 1);
        //    Console.WriteLine(" >>> " + pre_mc1);

        //    string mc1 = string.Empty;
        //    for (int i = 0; i < 16; i += 2)
        //    {
        //        if (i < pre_mc1.Length - 2)
        //        {
        //            if (Convert.ToInt32(pre_mc1.Substring(i, 2)) >= 65 && Convert.ToInt32(pre_mc1.Substring(i, 2)) <= 90)
        //            {
        //                mc1 += ((char)Convert.ToInt32(pre_mc1.Substring(i, 2))).ToString();
        //                Console.WriteLine(" >> " + Convert.ToInt32(pre_mc1.Substring(i, 2)) + " => " + ((char)Convert.ToInt32(pre_mc1.Substring(i, 2))).ToString());
        //            }
        //            else
        //            {
        //                mc1 += GetOneString(Convert.ToInt32(pre_mc1.Substring(i, 2)));
        //                Console.WriteLine(" >> " + Convert.ToInt32(pre_mc1.Substring(i, 2)) + " => " + GetOneString(Convert.ToInt32(pre_mc1.Substring(i, 2))));
        //            }
        //        }
        //    }

        //    return mc1;
        //}

        //public static string GetOneString(int ascii_char)
        //{
        //    string one_string = string.Empty;
        //    if (ascii_char <= 9)
        //    {
        //        return ((char)(ascii_char + 48)).ToString();
        //    }
        //    else
        //    {
        //        if (Convert.ToInt32(ascii_char.ToString().Substring(0, 1)) + Convert.ToInt32(ascii_char.ToString().Substring(1, 1)) > 9)
        //        {
        //            return GetOneString(Convert.ToInt32(ascii_char.ToString().Substring(0, 1)) + Convert.ToInt32(ascii_char.ToString().Substring(1, 1)));
        //        }
        //        else
        //        {
        //            return ((char)((Convert.ToInt32(ascii_char.ToString().Substring(0, 1)) + Convert.ToInt32(ascii_char.ToString().Substring(1, 1))) + 48)).ToString();
        //        }
        //    }

        //    //return one_string;
        //}

        

        //public static string GetMotherBoardSerialNumber()
        //{
        //    string mbInfo = String.Empty;

        //    ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
        //    scope.Connect();
        //    ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

        //    foreach (PropertyData propData in wmiClass.Properties)
        //    {
        //        if (propData.Name == "SerialNumber")
        //            //mbInfo = String.Format("{0,-25}{1}", propData.Name, Convert.ToString(propData.Value));
        //            mbInfo = Convert.ToString(propData.Value).Trim();
        //    }

        //    return mbInfo;
        //}

        //public static string GetCPUSerialNumber()
        //{
        //    string cpuInfo = string.Empty;

        //    ManagementClass mc = new ManagementClass("win32_processor");
        //    ManagementObjectCollection moc = mc.GetInstances();

        //    foreach (ManagementObject mo in moc)
        //    {
        //        cpuInfo = mo.Properties["processorID"].Value.ToString();
        //        break;
        //    }

        //    return cpuInfo;
        //}

        //private void WriteLogHardwareID()
        //{
        //    using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "HardwareInfo.txt", false, Encoding.GetEncoding("utf-8")))
        //    {
        //        sw.WriteLine("HDD. S/N : " + GetHDDSerialNumber());
        //        sw.WriteLine("MB. S/N : " + GetMotherBoardSerialNumber());
        //        sw.WriteLine("CPU. S/N : " + GetCPUSerialNumber());

        //        sw.Close();
        //    }
        //}


        private void ClearImgData()
        {
            this.txtSlip.Text = string.Empty;
        }

        private void btnBrowseSlip_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.RestoreDirectory = true;
            of.Filter = "JPG|*.jpg;*.jpeg|PNG|*.png|BMP|*.bmp|PDF|*.pdf";
            if (of.ShowDialog() == DialogResult.OK)
            {
                this.txtSlip.Text = of.FileName;
            }
        }

        private void btnBrowseTax_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.RestoreDirectory = true;
            of.Filter = "JPG|*.jpg;*.jpeg|PNG|*.png|BMP|*.bmp|PDF|*.pdf";
            if (of.ShowDialog() == DialogResult.OK)
            {
                this.txtTax.Text = of.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            LoadingScreen ls = new LoadingScreen();
            ls.ShowCenterParent(this);

            SRV_RegisterData srv_regdata = new SRV_RegisterData();
            srv_regdata._conn_success = false;
            srv_regdata._failed_reason = SRV_RegisterData.FAILED_REASON.NONE;

            LOC_RegisterData loc_regdata = new LOC_RegisterData();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                DriveInformation drive = Licensing.GetDriveInfo();

                //loc_regdata.token_key = Licensing.CalcMD5(drive.hddSerialNumber);
                //loc_regdata.machine_code = drive.hddSerialNumber;
                loc_regdata.token_key = Licensing.CalcMD5(Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()));
                loc_regdata.machine_code = Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter());
                loc_regdata.comp_name = Licensing.GetServerCompName();
                loc_regdata.reg_type = (Licensing.IsLAN() ? "LAN" : "LOCAL");
                loc_regdata.sernum = this.sernum;
                loc_regdata.contact = this.contact;
                loc_regdata.telnum = this.telnum;
                loc_regdata.email = this.email;
                loc_regdata.remark = this.remark;
                loc_regdata.slip_filename = Path.GetFileName(this.slip_file_path);
                loc_regdata.tax_filename = Path.GetFileName(this.tax_file_path);

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("token_key", loc_regdata.token_key);
                nvc.Add("machine_code", loc_regdata.machine_code);
                nvc.Add("comp_name", loc_regdata.comp_name);
                nvc.Add("reg_type", loc_regdata.reg_type);
                nvc.Add("sernum", loc_regdata.sernum);
                nvc.Add("contact", loc_regdata.contact);
                nvc.Add("telnum", loc_regdata.telnum);
                nvc.Add("email", loc_regdata.email);
                nvc.Add("remark", loc_regdata.remark);
                nvc.Add("slip_filename", loc_regdata.slip_filename);

                try
                {
                    string upload_tax_result = WebComm.UploadMultipart("http://www.esg.co.th/XML-Exporter/upload_tax_file.php", this.tax_file_path, "file1", "image/jpg", nvc);
                    ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(upload_tax_result);
                    nvc.Add("tax_filename", sr.message);

                    string web_result = WebComm.UploadMultipart("http://www.esg.co.th/XML-Exporter/upload_register.php", this.slip_file_path, "file1", "image/jpg", nvc);
                    //WebResult web_result = WebComm.UploadMultipart("http://www.weetee.com:3636/XML-Exporter/upload.php", this.file_path, "file1", "image/jpg", nvc);
                    srv_regdata = JsonConvert.DeserializeObject<SRV_RegisterData>(web_result);
                    //loc_regdata.status_code = srv_regdata.status_code;
                    //loc_regdata.status = srv_regdata.status;
                    loc_regdata.reg_date = DateTime.Parse(srv_regdata.reg_date, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
                    loc_regdata.reg_time = srv_regdata.reg_time;
                    loc_regdata.reg_unixtime = srv_regdata.reg_unixtime;
                }
                catch (Exception ex)
                {
                    srv_regdata._conn_success = false;
                }
            };
            worker.RunWorkerCompleted += delegate
            {
                ls.Close();

                if (srv_regdata._conn_success)
                {
                    if ((int)srv_regdata._failed_reason == (int)SRV_RegisterData.FAILED_REASON.NONE)
                    {
                        //if (Licensing.WriteTokenKey(loc_regdata.token_key, loc_regdata.machine_code, loc_regdata.comp_name, loc_regdata.reg_type, loc_regdata.sernum, loc_regdata.contact, loc_regdata.telnum, loc_regdata.email, loc_regdata.remark, loc_regdata.slip_filename, loc_regdata.status_code, loc_regdata.status, loc_regdata.reg_date.ToString("yyyy-MM-dd", CultureInfo.GetCultureInfo("en-US")), loc_regdata.reg_time, loc_regdata.reg_unixtime))
                        if (Licensing.WriteTokenKey(loc_regdata.token_key, loc_regdata.machine_code, loc_regdata.comp_name, loc_regdata.reg_type, loc_regdata.sernum, loc_regdata.contact, loc_regdata.telnum, loc_regdata.email, loc_regdata.remark, loc_regdata.slip_filename, loc_regdata.reg_date.ToString("yyyy-MM-dd", CultureInfo.GetCultureInfo("en-US")), loc_regdata.reg_time, loc_regdata.reg_unixtime))
                        {
                            MessageBox.Show("ส่งข้อมูลการลงทะเบียนโปรแกรมเรียบร้อย, เมื่อข้อมูลดังกล่าวได้รับการตรวจสอบจากทางบริษัทฯ แล้ว ท่านจะสามารถใช้งานความสามารถทั้งหมดของโปรแกรมได้ในทันที\n(โดยปกติใช้เวลาไม่เกิน 1 วันทำการ)");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        return;
                    }

                    if ((int)srv_regdata._failed_reason == (int)SRV_RegisterData.FAILED_REASON.DUPLICATE_SN)
                    {
                        MessageBox.Show(srv_regdata._failed_reason.ToReasonString());
                        return;
                    }
                }
                
                if(!srv_regdata._conn_success && srv_regdata._failed_reason == SRV_RegisterData.FAILED_REASON.NONE)
                {
                    MessageBox.Show("ไม่สามารถส่งข้อมูลการลงทะเบียนได้, กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            };
            worker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegisterDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.register_status == Licensing.STATUS.NOT_REGISTER)
            {
                if (this.DialogResult != DialogResult.OK)
                {
                    if (MessageBox.Show("ยกเลิกการลงทะเบียนโปรแกรม?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }

        private void btnReloadStatus_Click(object sender, EventArgs e)
        {
            LoadingScreen ls = new LoadingScreen();
            ls.ShowCenterParent(this);

            SRV_RegisterData srv_regdata = null;
            bool is_connected = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                if (!Licensing.IsServerConnected())
                {
                    is_connected = false;
                }
                else
                {
                    is_connected = true;
                }

                //if ((Licensing.IsLAN() && Licensing.IsServer()) || !Licensing.IsLAN())
                //{
                    //srv_regdata = Licensing.GetSrvRegisterData(Licensing.GetLocalRegData());
                    srv_regdata = Licensing.GetSrvRegisterData(RegisterDialog.ReadSernum(), Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()));
                //}
                //System.Threading.Thread.Sleep(500);
            };
            worker.RunWorkerCompleted += delegate
            {
                ls.Close();

                if (!is_connected)
                {
                    MessageBox.Show("ไม่สามารถติดต่อกับเซิร์ฟเวอร์ได้, กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (srv_regdata == null)
                {
                    this.register_status = Licensing.STATUS.NOT_REGISTER;
                }
                else
                {
                    if (srv_regdata.status_code == ((int)Licensing.STATUS.NOT_REGISTER).ToString())
                        this.register_status = Licensing.STATUS.NOT_REGISTER;

                    if (srv_regdata.status_code == ((int)Licensing.STATUS.WAIT_FOR_VERIFY).ToString())
                        this.register_status = Licensing.STATUS.WAIT_FOR_VERIFY;

                    if (srv_regdata.status_code == ((int)Licensing.STATUS.REGISTERED).ToString())
                        this.register_status = Licensing.STATUS.REGISTERED;
                }
                this.RepaintStatusPanel();
                this.SetControlState();
            };
            worker.RunWorkerAsync();
        }

        private void GetSrvRegData()
        {
            this.srv_regdata = Licensing.GetSrvRegisterData(RegisterDialog.ReadSernum(), Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.btnBrowseSlip.Focused || this.btnOK.Focused || this.btnCancel.Focused || this.btnClose.Focused)
                {
                    return false;
                }

                SendKeys.Send("{TAB}");
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
