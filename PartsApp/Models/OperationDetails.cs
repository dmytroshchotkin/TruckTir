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
        //private Lazy<Purchase> _purchase;
        public Purchase Purchase { get; set; }//{ return _purchase.Value; } }
        public float Count { get; set; }
        public float Price { get; set; }
        
        public OperationDetails(SparePart sparePart, Purchase purchase, float count, float price)
        {
            SparePart = sparePart;
            Purchase  = purchase;
            Count     = count;
            Price     = price;
        }//

    }//OperationDetails

}//namespace
