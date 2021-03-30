using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class PdfUserInformation
    {
    
        public string Text { get; set; }

        public string Information { get; set; }

        public PdfUserInformation(string text, string information)
        {
            Text = text;
            Information = information;
        }
    }
}
