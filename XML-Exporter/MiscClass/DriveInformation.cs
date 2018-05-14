using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.MiscClass
{
    public class DriveInformation
    {
        public bool isLocalDrive { get; set; }
        public bool isNetworkDrive { get; set; }
        public bool isRemovableDrive { get; set; }
        public bool isCdRomDrive { get; set; }
        public bool isRamDrive { get; set; }
        public bool isNoRootDirectory { get; set; }
        public bool isUnknowDrive { get; set; }
        public string compName { get; set; }
        public string hddSerialNumber { get; set; }
        public bool correctMachine { get; set; }
    }
}
