using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class UpdateModel
    {
        public string MediaFile { get; set; }
        public string Value { get; set; }
        public bool Update { get; set; }
        public int Count { get; set; }

        public int MediaId { get; set; }

        public UpdateModel(string mediaFile, string value, int mediaId = 0, int count = 0)
        {
            MediaFile = mediaFile;
            Value = value;
            Update = true;
            Count = count;
            MediaId = mediaId;
        }
    }
}
