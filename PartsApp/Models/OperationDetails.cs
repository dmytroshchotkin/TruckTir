using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class OperationDetails
    {
        //private Lazy<SparePart> _sparePart;
        public SparePart SparePart { get; set; } //{ return _sparePart.Value; } }
        //private Lazy<Operation> _purchase;
        public IOperation Operation { get; set; }//{ return _purchase.Value; } }
        public float Count { get; set; }
        public float Price { get; set; }

        public OperationDetails() { }
        public OperationDetails(SparePart sparePart, IOperation operation, float count, float price)
        {
            SparePart = sparePart;
            Operation = operation;
            Count     = count;
            Price     = price;
        }//

    }//OperationDetailsList

}//namespace
