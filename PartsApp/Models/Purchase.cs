using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class Purchase : IOperation
    {
        public int OperationId { get; set; }
        public Employee Employee { get; set; }
        public IContragent Contragent { get; set; }
        public string ContragentEmployee { get; set; }
        public DateTime OperationDate { get; set; }
        public string Description { get; set; }
       
        public IList<SparePart> OperationDetailsList { get; set; }

        public Purchase() { }
        public Purchase(int operationId, Employee employee, IContragent contragent, string contragentEmployee,
                        DateTime operationDate, string description ) 
        {
            OperationId        = operationId;
            Employee           = employee;
            Contragent         = contragent;
            ContragentEmployee = contragentEmployee;
            OperationDate      = operationDate;
            Description        = description;          
        }
    }//Purchase

}//namespace
