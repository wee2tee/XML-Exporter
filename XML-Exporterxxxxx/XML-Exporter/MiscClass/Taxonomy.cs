using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using MetroFramework;


namespace XML_Exporter.MiscClass
{
    public class Taxonomy
    {
        public string code { get; set; }
        public string group { get; set; }
        public string name { get; set; }
        public string taxodesc { get; set; }

        //public static List<Taxonomy> GetTaxonomyList()
        //{
        //    List<Taxonomy> taxo = new List<Taxonomy>();

        //    taxo.Add(new Taxonomy() { group = "1", taxodesc = "CSG_Assets", name = "สินทรัพย์" });
        //    taxo.Add(new Taxonomy() { group = "1", taxodesc = "CSG_CurrAssets", name = "สินทรัพย์หมุนเวียน" });
        //    taxo.Add(new Taxonomy() { group = "1", taxodesc = "CSG_Cash", name = "เงินสดและรายการเทียบเท่าเงินสด" });
        //    taxo.Add(new Taxonomy() { group = "1", taxodesc = "CSG_CashFlow_Op", name = "กระแสเงินสดจากกิจกรรมดำเนินงาน" });
        //    taxo.Add(new Taxonomy() { group = "1", taxodesc = "CSG_CashPayDiv(Op)", name = "เงินปันผลจ่าย" });

        //    taxo.Add(new Taxonomy() { group = "2", taxodesc = "CSG_LiabilitiesAndEquityAbstract", name = "หนี้สินและส่วนของผู้ถือหุ้น" });
        //    taxo.Add(new Taxonomy() { group = "2", taxodesc = "CSG_CurrentLiabilities", name = "หนิ้สินหมุนเวียน" });

        //    taxo.Add(new Taxonomy() { group = "3", taxodesc = "CSG_Equity", name = "ส่วนของผู้ถือหุ้น" });
        //    taxo.Add(new Taxonomy() { group = "3", taxodesc = "CSG_ShareCapital", name = "ทุนเรือนหุ้น" });

        //    taxo.Add(new Taxonomy() { group = "4", taxodesc = "CSG_Revenue", name = "รายได้" });
        //    taxo.Add(new Taxonomy() { group = "4", taxodesc = "CSG_RevenuesFromSalesOrRevenueFromServices", name = "รายได้จากการขายหรือให้บริการ" });

        //    taxo.Add(new Taxonomy() { group = "5", taxodesc = "CSG_Expense", name = "ค่าใช้จ่าย" });
        //    taxo.Add(new Taxonomy() { group = "5", taxodesc = "CSG_WorkPerformedByEntityAndCapitalized", name = "การเปลี่ยนแปลงของสินค้าสำเร็จรูปและงานระหว่างทำ" });

        //    return taxo;
        //}

        public static List<Taxonomy> GetTaxonomyList(MainForm main_form)
        {
            List<Taxonomy> taxo = new List<Taxonomy>();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Template/dbd_taxonomy.xml"))
            {
                XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/Template/dbd_taxonomy.xml");
                Console.WriteLine(" >>>> xdoc count : " + xdoc.Root.Descendants("Taxonomy").Count());

                foreach (XElement elem in xdoc.Root.Descendants("Taxonomy"))
                {
                    taxo.Add(new Taxonomy()
                    {
                        code = elem.Element("Code").Value,
                        group = elem.Element("Group").Value,
                        name = elem.Element("Name").Value,
                        taxodesc = elem.Element("Taxodesc").Value
                    });
                }
            }
            else
            {
                MetroMessageBox.Show(main_form, "ไม่พบไฟล์ dbd_taxonomy.xml", "พบข้อผิดพลาด : ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return taxo;
        }

        public override string ToString()
        {
            return "[" + this.taxodesc + "]" + " " + this.name;
        }
    }
}
