using FelixWebsite.Bdo.Models.Acquisition;
using System.Collections.Generic;

namespace FelixWebsite.Bdo.Api
{
    public class DamageDeclaration
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LicensePlate { get; set; }
        public string Insurance { get; set; }
        public byte[] Pdf { get; set; }
        public List<File> AdditionalFiles { get; set; }

        public class File
        {
            public string Filename { get; set; }
            public byte[] Contents { get; set; }
        }

        public DamageDeclaration(string name, string email, string mobile, string licensePlate, string insurance, string pdfFileLocation, List<File> additionalFiles)
        {
            Name = name;
            Email = email;
            Mobile = mobile;
            LicensePlate = licensePlate;
            Insurance = insurance;
            Pdf = System.IO.File.ReadAllBytes(pdfFileLocation);
            AdditionalFiles = additionalFiles;
        }
    }
}