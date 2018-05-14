using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using XML_Exporter.DataModel;
using XML_Exporter.MiscClass;
using DBFHelper;
using CSGDBDConnectLib;
using ICSharpCode.SharpZipLib;
using System.IO;
using System.Xml.Linq;
using System.Threading;

namespace XML_Exporter.SubWindow
{
    public partial class CSGRequireDialog : Form
    {
        private MainForm main_form;
        private string sender = "";
        private string receiver = "";
        private string excel_path = "";
        private Isprd isprd;
        private Isinfo isinfo;
        private Sccomp selected_comp;
        private List<Glbal> list_glbal;
        private List<Gljnlit> list_gljnlit;
        private List<dataItem4Glacc> list_posting_item;

        public CSGRequireDialog(MainForm main_form, Sccomp selected_comp)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            InitializeComponent();
            this.main_form = main_form;
            this.selected_comp = selected_comp;
        }

        private void ControlFileAddDataDialog_Load(object sender, EventArgs e)
        {
            this.BindingControlEventHandler();
            //try
            //{
                this.list_posting_item = this.main_form.list_glacc_item.ConvertAll(t => t).ToList<dataItem4Glacc>();
                this.isinfo = this.main_form.LoadIsinfo();
                this.isprd = this.main_form.LoadIsprd();

                if (this.isprd != null)
                {
                    int start_year = Convert.ToInt32(this.isprd.beg1.ToString("yyy", CultureInfo.CurrentCulture.DateTimeFormat));
                    int end_year = Convert.ToInt32(this.isprd.end12ny.ToString("yyy", CultureInfo.CurrentCulture.DateTimeFormat));
                    this.cbYear.Items.Clear();
                    for (int i = start_year; i <= end_year; i++)
                    {
                        this.cbYear.Items.Add(i);
                    }
                    this.cbYear.SelectedIndex = 0;

                    this.dtFrom.MinDate = this.isprd.beg1;
                    this.dtFrom.MaxDate = this.isprd.end12ny;
                    this.dtTo.MinDate = this.isprd.beg1;
                    this.dtTo.MaxDate = this.isprd.end12ny;
                }
            //}
            //catch (Exception)
            //{
                
            //}

        }

        private void BindingControlEventHandler()
        {
            this.txtSender.TextChanged += delegate
            {
                this.sender = this.txtSender.Text.Trim();
            };

            this.txtReceiver.TextChanged += delegate
            {
                this.receiver = this.txtReceiver.Text.Trim();
            };

            this.cbYear.SelectedIndexChanged += delegate
            {
                this.dtFrom.Value = DateTime.Parse((int)this.cbYear.SelectedItem + "-01-01", CultureInfo.CurrentCulture.DateTimeFormat);
                this.dtTo.Value = DateTime.Parse((int)this.cbYear.SelectedItem + "-12-31", CultureInfo.CurrentCulture.DateTimeFormat);
            };

            this.txtExcelPath.TextChanged += delegate
            {
                this.excel_path = this.txtExcelPath.Text;
            };
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = (this.main_form.export_path.Trim().Length == 0 ? @AppDomain.CurrentDomain.BaseDirectory : this.main_form.export_path);
            fd.Description = "\tเลือกไดเร็คทอรี่ที่เก็บไฟล์ Excel ของกรมพัฒน์ฯ\n(ดาวน์โหลดด้วยโปรแกรม DBD XBRL in Excel)";
            fd.ShowNewFolderButton = false;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.main_form.export_path = fd.SelectedPath;
                this.txtExcelPath.Text = fd.SelectedPath;
                this.btnOK.Focus();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtSender.Text.Trim().Length == 0)
            {
                MessageBox.Show("กรุณาป้อนข้อมูลในช่อง \"ผู้ส่งข้อมูล (Sender)\"", AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtSender.Focus();
                return;
            }

            if (this.txtReceiver.Text.Trim().Length == 0)
            {
                MessageBox.Show("กรุณาป้อนข้อมูลในช่อง \"ผู้รับข้อมูล (Receiver)\"", AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtReceiver.Focus();
                return;
            }

            if (this.excel_path.Trim().Length == 0)
            {
                MessageBox.Show("กรุณาระบุไดเร็คทอรี่ที่เก็บไฟล์ Excel ที่ดาวน์โหลดจากกรมพัฒน์ฯ", AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnBrowse.Focus();
                return;
            }

            this.Save2CSG();
        }

        private void Save2CSG()
        {
            LoadingScreen ls = new LoadingScreen();
            ls.ShowCenterParent(this);

            BackgroundWorker worker = new BackgroundWorker();
            this.dtFrom.Enabled = false;
            this.dtTo.Enabled = false;
            this.txtSender.Enabled = false;
            this.txtReceiver.Enabled = false;
            this.btnBrowse.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnCancel.Enabled = false;
            worker.DoWork += delegate(object obj, DoWorkEventArgs ev)
            {
                this.list_glbal = this.main_form.LoadGlbal();
                this.list_gljnlit = this.main_form.LoadGljnlit();
            };
            worker.RunWorkerCompleted += delegate(object obj, RunWorkerCompletedEventArgs ev)
            {
                ls.Close();
                this.dtFrom.Enabled = true;
                this.dtTo.Enabled = true;
                this.txtSender.Enabled = true;
                this.txtReceiver.Enabled = true;
                this.btnBrowse.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnCancel.Enabled = true;

                if (this.CreateXML4CSG(this.excel_path + "/TB.xml"))
                {
                    if (this.CreateControlFile(this.excel_path + "/Control_File.xml", this.sender, this.receiver))
                    {
                        if (this.CreateCSGFile(this.main_form.export_path))
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            };
            worker.RunWorkerAsync();
        }

        private bool CreateXML4CSG(string path)
        {
            try
            {
                XElement dataset_elem = new XElement("DataSet");
                XDocument xdoc = new XDocument(new XDeclaration("1.0", "windows-874", "yes"),
                    dataset_elem
                );
                foreach (dataItem4Glacc item in this.list_posting_item.Where(g => g.acctyp == "0").OrderBy(g => g.group).ToList<dataItem4Glacc>())
                {
                    AccAmountInfo amount = this.main_form.GetAccAmountInfo(item, this.dtFrom.Value, this.dtTo.Value, this.list_glbal, this.list_gljnlit);
                    //if (amount.bal_fwd == 0 && amount.carr_fwd == 0 && amount.prd_cr == 0 && amount.prd_dr == 0)
                    //    continue;

                    dataset_elem.Add(
                        new XElement("Data_TB",
                            new XElement("qntaxodesc", this.main_form.taxonomy_list.Find(t => t.taxodesc == item.taxonomy).name),
                            //new XElement("qctaxodesc", item.taxonomy),
                            new XElement("qctaxodesc", (amount.carr_fwd >= 0 ? item.taxonomy : (item.taxonomy2.Trim().Length > 0 ? item.taxonomy2 : item.taxonomy))),
                            new XElement("qcacchart", item.accnum),
                            new XElement("qnacchart", item.accnam.Trim()),
                            new XElement("bfamt", (amount.carr_fwd >= 0 ? amount.bal_fwd.ToString() : (item.taxonomy2.Trim().Length > 0 ? (amount.bal_fwd * -1).ToString() : amount.bal_fwd.ToString()))),
                            //new XElement("perddramt", (amount.carr_fwd >= 0 ? amount.prd_dr.ToString() : (item.taxonomy2.Trim().Length > 0 ? (amount.prd_dr * -1).ToString() : amount.prd_dr.ToString()))),
                            //new XElement("perdcramt", (amount.carr_fwd >= 0 ? amount.prd_cr.ToString() : (item.taxonomy2.Trim().Length > 0 ? (amount.prd_cr * -1).ToString() : amount.prd_cr.ToString()))),
                            new XElement("perddramt", amount.prd_dr.ToString()),
                            new XElement("perdcramt", amount.prd_cr.ToString()),
                            new XElement("cfamt", (amount.carr_fwd >= 0 ? amount.carr_fwd.ToString() : (item.taxonomy2.Trim().Length > 0 ? (amount.carr_fwd * -1).ToString() : amount.carr_fwd.ToString())))
                        )
                    );
                }

                xdoc.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    this.CreateXML4CSG(path);
                }
                return true;
            }
        }

        private bool CreateControlFile(string path, string sender, string receiver)
        {
            try
            {
                CultureInfo cinfo_en = new CultureInfo("en-US");

                StreamWriter sw = new StreamWriter(@path, false, Encoding.GetEncoding("utf-8"));
                //sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sw.WriteLine("<SmartBiz_DBD_Connect>");
                sw.WriteLine("\t<Code_of_juristic_person>" + MainForm.RewriteDataPath(this.selected_comp.path.Trim(), true) + "</Code_of_juristic_person>");
                sw.WriteLine("\t<Name_of_juristic_person>" + this.isinfo.thinam + "</Name_of_juristic_person>");
                sw.WriteLine("\t<Juristic_person_identification_number>" + this.isinfo.taxid + "</Juristic_person_identification_number>");
                sw.WriteLine("\t<Accounting_period>" + DateTime.Parse((int)this.cbYear.SelectedItem + "-01-01", CultureInfo.CurrentCulture.DateTimeFormat).ToString("yyy", cinfo_en) + "</Accounting_period>");
                sw.WriteLine("\t<Start_date_of_current_reporting_period>" + this.dtFrom.Value.ToString("yyy-MM-dd", cinfo_en) + "</Start_date_of_current_reporting_period>");
                sw.WriteLine("\t<End_date_of_current_reporting_period>" + this.dtTo.Value.ToString("yyy-MM-dd", cinfo_en) + "</End_date_of_current_reporting_period>");
                sw.WriteLine("\t<Template>NPAE_COM-OTH</Template>");
                sw.WriteLine("\t<Sender>" + sender + "</Sender>");
                sw.WriteLine("\t<Receiver>" + receiver + "</Receiver>");
                sw.WriteLine("\t<Application_ID>" + "Express" + "</Application_ID>");
                sw.WriteLine("</SmartBiz_DBD_Connect>");
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    this.CreateControlFile(path, sender, receiver);
                }
                return false;
            }
        }

        private bool CreateCSGFile(string path)
        {
            CSGDBDConnectLib.Helper csgHelper = new CSGDBDConnectLib.Helper();
            try
            {
                string public_key = "<BitStrength>2048</BitStrength><RSAKeyValue><Modulus>xqN4d+ZGTSyF4Pph9WD6NcGIhldDgMaj//rkUrK0/XetRsIBtn14nyUculstHsEUdwdhabIIBlsAik16L9MWQ6HAH4hzO5Qz1kUM+bAW79EMHawqwMS0dmDIrYa6JRcHW4Poo+b37ozpQRrvNogt71PLPp4ytNzVYFeuX34PHecXvmFiT1aeMesBiAJapSDBoXZD4gOqzuFkfxivA5anHqhxgkdARoArpnPij1g7d1d0a6ufsw2zLUNaTPyllR6ynTiyJk2ztHCM/NPk/Pujpn+kc5k4Yodf1AXXM5AcDMFTnzIGzLFfFs9uxjjYwRiribZqy0oxjL2KfeODdqswkQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

                if (csgHelper.gmethEncryptFolder(public_key, path))
                //if(csgHelper.gmethEncryptFolder(path))
                {
                    return true;
                }
                else if (!string.IsNullOrEmpty(csgHelper.gstrErrMsg))
                {
                    MessageBox.Show(csgHelper.gstrErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.btnOK.Focused || this.btnCancel.Focused || this.btnBrowse.Focused)
                    return false;

                SendKeys.Send("{TAB}");
                return true;
            }

            if (keyData == Keys.Escape)
            {
                this.btnCancel.PerformClick();
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
