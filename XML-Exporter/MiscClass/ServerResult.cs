using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XML_Exporter.MiscClass
{
    public class ServerResult
    {
        public const int SERVER_CREATE_RESULT_FAILED = 0;
        public const int SERVER_CREATE_RESULT_FAILED_EXIST = 1;
        public const int SERVER_READ_RESULT_FAILED = 2;
        public const int SERVER_UPDATE_RESULT_FAILED = 3;
        public const int SERVER_UPDATE_RESULT_FAILED_EXIST = 4;
        public const int SERVER_DELETE_RESULT_FAILED = 5;
        public const int SERVER_RESULT_SUCCESS = 99;

        public bool result { get; set; }
        public string message { get; set; }
    }
}
