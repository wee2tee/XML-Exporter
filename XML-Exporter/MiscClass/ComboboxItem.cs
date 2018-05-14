using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.MiscClass
{
    public class ComboboxItem
    {
        private string text = "";
        private object tag = null;

        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        public ComboboxItem(string display_text, object object_tag)
        {
            this.text = display_text;
            this.Tag = object_tag;
        }

        public override string ToString()
        {
            //return base.ToString();
            return this.text;
        }
    }
}
