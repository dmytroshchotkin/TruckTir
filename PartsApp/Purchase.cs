using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    class Purchase
    {
        public int      PurchaseId                   { get; set; }
        public int?     EmployeeId                   { get; set; }
        public int      SupplierId                   { get; set; }
        public string   SupplierEmployee             { get; set; }
        public DateTime PurchaseDate                 { get; set; }
        public string   Currency                     { get; set; }
        public double   ExcRate                      { get; set; }
        public IList<PurchaseDetail> PurchaseDetails { get; set; }
          
        public Purchase() {}
        public Purchase(int purchaseId, int? employeeid, int supplierid, string supplierEmployee, DateTime purchaseDate, string currency, double excRate)         
        {      
            PurchaseId        = purchaseId;
            EmployeeId        = employeeid;
            SupplierId        = supplierid;
            SupplierEmployee  = supplierEmployee;
            PurchaseDate      = purchaseDate;
            Currency          = currency;
            ExcRate           = excRate;         
        }
        public Purchase(int? employeeid, int supplierid, string supplierEmployee, DateTime purchaseDate, string currency, double excRate)
        {
            EmployeeId = employeeid;
            SupplierId = supplierid;
            SupplierEmployee = supplierEmployee;
            PurchaseDate = purchaseDate;
            Currency = currency;
            ExcRate = excRate;
        }
    }//Purchase

    class PurchaseDetail
    { 
        public int    PurchaseId  { get; set; }
        public int    SparePartId { get; set; }
        public double Price       { get; set; }
        public double Quantity    { get; set; }

        public PurchaseDetail() { }
        public PurchaseDetail(int purchaseId, int sparePartId, double price, double quantity)
        {
            PurchaseId  =  purchaseId;
            SparePartId =  sparePartId;
            Price       =  price;
            Quantity    =  quantity;
        }
    }//PurchaseDetail

}//namespace

/*
 для преобразования даты в целое число используется функция strftime('%s', value), а для обратного преобразования используется функция datetime(value, 'unixepoch').
*/