using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace XML_Exporter.MiscClass
{
    public class DriveInfo2
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetVolumeInformation(
            string rootPathName,
            StringBuilder volumeNameBuffer,
            int volumeNameSize,
            ref uint volumeSerialNumber,
            ref uint maximumComponentLength,
            ref uint fileSystemFlags,
            StringBuilder fileSystemNameBuffer,
            int nFileSystemNameSize);

        public string VolumeName { get; private set; }

        public string FileSystemName { get; private set; }

        public uint SerialNumber { get; private set; }

        public string DriveLetter { get; private set; }

        public static DriveInfo2 GetDrive(string driveLetter)
        {
            const int VolumeNameSize = 255;
            const int FileSystemNameBufferSize = 255;
            StringBuilder volumeNameBuffer = new StringBuilder(VolumeNameSize);
            uint volumeSerialNumber = 0;
            uint maximumComponentLength = 0;
            uint fileSystemFeatures = 0;
            StringBuilder fileSystemNameBuffer = new StringBuilder(FileSystemNameBufferSize);

            if (GetVolumeInformation(
                string.Format("{0}:\\", driveLetter),
                volumeNameBuffer,
                VolumeNameSize,
                ref volumeSerialNumber,
                ref maximumComponentLength,
                ref fileSystemFeatures,
                fileSystemNameBuffer,
                FileSystemNameBufferSize))
            {
                return new DriveInfo2
                {
                    DriveLetter = driveLetter,
                    FileSystemName = fileSystemNameBuffer.ToString(),
                    VolumeName = volumeNameBuffer.ToString(),
                    SerialNumber = volumeSerialNumber
                };
            }

            // Something failed, returns null
            return null;
        }
    }
}
