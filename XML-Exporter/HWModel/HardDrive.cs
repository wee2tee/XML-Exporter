using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.HWModel
{
    class HardDrive
    {
        private string model = null;
        private string type = null;
        private string serialNo = null;
        private string deviceID = null;
        
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }
    }
}
