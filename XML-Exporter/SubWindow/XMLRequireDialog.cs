using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Globalization;
using DBFHelper;
using XML_Exporter.DataModel;
using XML_Exporter.MiscClass;
using System.IO;
using System.Threading;

namespace XML_Exporter.SubWindow
{
    public partial class XMLRequireDialog : Form
    {
        private List<dataItem4Glacc> list_posting_item;
        private MainForm main_form;
        private Sccomp selected_comp;
        private Isinfo isinfo;
        private Isprd isprd;
        private List<Glbal> list_glbal;
        private List<Gljnlit> list_gljnlit;

        public XMLRequireDialog(MainForm main_form, Sccomp selected_sccomp)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            InitializeComponent();

            this.main_form = main_form;
            this.selected_comp = selected_sccomp;
        }

        private void XMLRequireDialog_Load(object sender, EventArgs e)
        {
            this.BindingControlEventHandler();

            this.list_posting_item = this.main_form.list_glacc_item.ConvertAll(t => t).ToList<dataItem4Glacc>();
            this.isinfo = this.main_form.LoadIsinfo();
            this.isprd = this.main_form.LoadIsprd();

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

        private void BindingControlEventHandler()
        {
            this.cbYear.SelectedIndexChanged += delegate
            {
                this.dtFrom.Value = DateTime.Parse((int)this.cbYear.SelectedItem + "-01-01", CultureInfo.CurrentCulture.DateTimeFormat);
                this.dtTo.Value = DateTime.Parse((int)this.cbYear.SelectedItem + "-12-31", CultureInfo.CurrentCulture.DateTimeFormat);
            };
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = "xml";
            sf.Filter = "xml files (*.xml)|*.xml";
            sf.RestoreDirectory = true;
            sf.CheckPathExists = true;
            sf.AddExtension = true;
            sf.InitialDirectory = (this.main_form.export_path.Trim().Length == 0 ? @AppDomain.CurrentDomain.BaseDirectory : this.main_form.export_path);

            if (sf.ShowDialog() == DialogResult.OK)
            {
                this.main_form.export_path = Path.GetDirectoryName(sf.FileName);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += delegate(object obj, DoWorkEventArgs ev)
                {
                    this.list_glbal = this.main_form.LoadGlbal();
                    this.list_gljnlit = this.main_form.LoadGljnlit();
                    this.isinfo = this.main_form.LoadIsinfo();
                };
                worker.RunWorkerCompleted += delegate(object obj, RunWorkerCompletedEventArgs ev)
                {
                    if (this.CreateXML4Excel(sf.FileName))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                };
                worker.RunWorkerAsync();
            }
        }

        private bool CreateXML4Excel(string path)
        {
            try
            {
                XNamespace ns_m = "urn:schemas-microsoft-com:office:spreadsheet";
                XNamespace ns_o = "urn:schemas-microsoft-com:office:office";
                XNamespace ns_x = "urn:schemas-microsoft-com:office:excel";
                XNamespace ns_ss = "urn:schemas-microsoft-com:office:spreadsheet";

                XElement table_elem = new XElement(ns_ss + "Table", new XAttribute(ns_ss + "ExpandedColumnCount", "10"), new XAttribute(ns_ss + "ExpandedRowCount", (this.list_posting_item.Where(g => g.acctyp == "0").Count<dataItem4Glacc>() + 2).ToString()),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                    new XElement(ns_ss + "Row", new XAttribute(ns_ss + "row_type", "report-header"),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "1"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ตั้งแต่วันที่")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "2"), new XAttribute(ns_ss + "StyleID", "s20"),
                    //new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "DateTime"), this.dtFrom.Value.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture.DateTimeFormat))
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "DateTime"), this.dtFrom.Value)
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "3"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ถึงวันที่")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "4"), new XAttribute(ns_ss + "StyleID", "s20"),
                    //new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "DateTime"), this.dtTo.Value.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture.DateTimeFormat))
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "DateTime"), this.dtTo.Value)
                        )
                    ),
                    new XElement(ns_ss + "Row", new XAttribute(ns_ss + "row_type", "column-header"),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "1"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "Taxonomy Name")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "2"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "Taxonomy Description")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "3"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "เลขที่บัญชี")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "4"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ชื่อบัญชี")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "5"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ยอดยกมา (เดบิท)")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "6"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ยอดยกมา (เครดิต)")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "7"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ยอดเคลื่อนไหว (เดบิท)")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "8"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ยอดเคลื่อนไหว (เครดิต)")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "9"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ยอดคงเหลือสิ้นงวด (เดบิท)")
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "10"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ยอดคงเหลือสิ้นงวด (เครดิต)")
                        )
                    )
                );
                XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(ns_m + "Workbook", new XAttribute("xmlns", ns_m), new XAttribute(XNamespace.Xmlns + "o", ns_o), new XAttribute(XNamespace.Xmlns + "x", ns_x), new XAttribute(XNamespace.Xmlns + "ss", ns_ss),
                        new XElement(ns_ss + "Styles", new XElement(ns_ss + "Style", new XAttribute(ns_ss + "ID", "s20"), new XElement(ns_ss + "NumberFormat", new XAttribute(ns_ss + "Format", "Short Date")))),
                        new XElement(ns_ss + "Worksheet", new XAttribute(ns_ss + "Name", "Express_GL_Data"),
                            table_elem
                        )
                    )
                );

                foreach (dataItem4Glacc item in this.list_posting_item.Where(g => g.acctyp == "0").OrderBy(g => g.group).ToList<dataItem4Glacc>())
                {
                    AccAmountInfo amount = this.main_form.GetAccAmountInfo(item, this.dtFrom.Value, this.dtTo.Value, this.list_glbal, this.list_gljnlit);
                    if (amount.bal_fwd == 0 && amount.carr_fwd == 0 && amount.prd_cr == 0 && amount.prd_dr == 0)
                        continue;

                    double bal_fwd_dr = ((item.group == "1" || item.group == "5") && amount.bal_fwd > 0) || ((item.group == "2" || item.group == "3" || item.group == "4") && amount.bal_fwd < 0) ? (amount.bal_fwd < 0 ? amount.bal_fwd * -1 : amount.bal_fwd) : 0;
                    double bal_fwd_cr = ((item.group == "2" || item.group == "3" || item.group == "4") && amount.bal_fwd > 0) || ((item.group == "1" || item.group == "5") && amount.bal_fwd < 0) ? (amount.bal_fwd < 0 ? amount.bal_fwd * -1 : amount.bal_fwd) : 0;
                    double carr_fwd_dr = ((item.group == "1" || item.group == "5") && amount.carr_fwd > 0) || ((item.group == "2" || item.group == "3" || item.group == "4") && amount.carr_fwd < 0) ? (amount.carr_fwd < 0 ? amount.carr_fwd * -1 : amount.carr_fwd) : 0;
                    double carr_fwd_cr = ((item.group == "2" || item.group == "3" || item.group == "4") && amount.carr_fwd > 0) || ((item.group == "1" || item.group == "5") && amount.carr_fwd < 0) ? (amount.carr_fwd < 0 ? amount.carr_fwd * -1 : amount.carr_fwd) : 0;

                    table_elem.Add(new XElement(ns_ss + "Row", new XAttribute(ns_ss + "row_type", "data"),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "1"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), this.main_form.taxonomy_list.Find(t => t.taxodesc == item.taxonomy).name)
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "2"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), item.taxonomy)
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "3"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), item.accnum)
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "4"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), item.accnam.Trim())
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "5"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), bal_fwd_dr.ToString())
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "6"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), bal_fwd_cr.ToString())
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "7"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), amount.prd_dr.ToString())
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "8"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), amount.prd_cr.ToString())
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "9"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), carr_fwd_dr.ToString())
                        ),
                        new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "10"),
                            new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), carr_fwd_cr.ToString())
                        ))
                    );
                }

                xdoc.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    this.CreateXML4Excel(path);
                }
                return true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.btnOK.Focused || this.btnCancel.Focused)
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
