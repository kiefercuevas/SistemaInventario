using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class DataTable
    {
        public int Start { get; set; }
        public string Search { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordFiltered { get; set; }
        public int Draw { get; set; }
        public string Order { get; set; }
        public string OrderDir { get; set; }
        public int Length { get; set; }

    }
}