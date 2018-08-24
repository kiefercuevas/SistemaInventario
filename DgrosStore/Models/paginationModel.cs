using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models
{
    public class PaginationModel
    {
        public int ActualPage { get; set; }
        public int TotalRecords { get; set; }
        public int PageRecordNumber { get; set; }
    }
}