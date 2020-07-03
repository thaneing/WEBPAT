using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DetailTableERP
    {
        public int ID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
        public int FieldID { get; set; }
        public string FieldName { get; set; }
        public string Detail { get; set; }

    }
}
