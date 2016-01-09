using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    interface IOperation
    {
        public int OperationId { get; set; }
        public Employee Employee { get; set; }
        public IContragent ContragentId { get; set; }
        public string ContragentEmployee { get; set; }
        public DateTime OperationDate { get; set; }
        public string Currency { get; set; }
        public double ExcRate { get; set; }
        public string Description { get; set; }
        //public IList<PurchaseDetail> PurchaseDetails { get; set; }

    }//IOperation


}//namespace
