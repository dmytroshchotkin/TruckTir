using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    class Sale
    {
        public int      SaleId                       { get; set; }
        public int?     EmployeeId                   { get; set; }
        public int      CustomerId                   { get; set; }
        public string   CustomerEmployee             { get; set; }
        public DateTime SaleDate                     { get; set; }
        public string   Currency                     { get; set; }
        public double   ExcRate                      { get; set; }
        public string   Description                  { get; set; }
        public IList<PurchaseDetail> PurchaseDetails { get; set; }
          
        public Sale() {}
        public Sale(int saleId, int? employeeId, int customerId, string customerEmployee, DateTime saleDate, string currency, double excRate)         
        {      
            SaleId            = saleId;
            EmployeeId        = employeeId;
            CustomerId        = customerId;
            CustomerEmployee  = customerEmployee;
            SaleDate          = saleDate;
            Currency          = currency;
            ExcRate           = excRate;         
        }
        public Sale(int? employeeId, int customerId, string customerEmployee, DateTime saleDate, string currency, double excRate)
        {
            EmployeeId       = employeeId;
            CustomerId       = customerId;
            CustomerEmployee = customerEmployee;
            SaleDate         = saleDate;
            Currency         = currency;
            ExcRate          = excRate;
        }

    }//Sale


}//namespace
