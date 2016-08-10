using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class Sale : IOperation
    {
        public int OperationId { get; set; }
        public Employee Employee { get; set; }
        public IContragent Contragent { get; set; }
        public string ContragentEmployee { get; set; }
        public DateTime OperationDate { get; set; }
        public string Description { get; set; }

        private Lazy<IList<OperationDetails>> _operationDetailsList;
        public IList<OperationDetails> OperationDetailsList { get { return _operationDetailsList.Value; } }

        public Sale(){}
        public Sale(Employee employee, IContragent contragent, string contragentEmployee,
                        DateTime operationDate, string description, List<OperationDetails> operDetList)
        {
            Employee = employee;
            Contragent = contragent;
            ContragentEmployee = contragentEmployee;
            OperationDate = operationDate;
            Description = description;

            _operationDetailsList = new Lazy<IList<OperationDetails>>(() => operDetList);
        }//

        public Sale(int operationId, Employee employee, IContragent contragent, string contragentEmployee,
                    DateTime operationDate, string description ) 
        {
            OperationId           = operationId;
            Employee              = employee;
            Contragent            = contragent;
            ContragentEmployee    = contragentEmployee;
            OperationDate         = operationDate;
            Description           = description;          
        }//

    }//Sale


}//namespace
