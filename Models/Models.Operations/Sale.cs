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
        public bool PaidCash { get; set; }

        private Lazy<List<OperationDetails>> _operationDetailsList;
        public List<OperationDetails> OperationDetailsList { get { return _operationDetailsList.Value; } }

        public Sale(){}
        public Sale(Employee employee, IContragent contragent, string contragentEmployee,
                        DateTime operationDate, string description, List<OperationDetails> operDetList, bool paidCash = true)
        {
            Employee = employee;
            Contragent = contragent;
            ContragentEmployee = contragentEmployee;
            OperationDate = operationDate;
            Description = description;
            PaidCash = paidCash;

            _operationDetailsList = new Lazy<List<OperationDetails>>(() => operDetList);
        }

        public Sale(int operationId, Employee employee, IContragent contragent, string contragentEmployee,
                    DateTime operationDate, string description, bool paidCash = true) 
        {
            OperationId           = operationId;
            Employee              = employee;
            Contragent            = contragent;
            ContragentEmployee    = contragentEmployee;
            OperationDate         = operationDate;
            Description           = description;
            PaidCash = paidCash;

            //_operationDetailsList = new Lazy<IList<OperationDetails>>(() => PartsDAL.FindSaleDetails(this));
        }

        public void TrySetOperationDetails(Lazy<List<OperationDetails>> operationDetails)
        {
            if (operationDetails != null)
            {
                _operationDetailsList = operationDetails;
            }
        }
    }


}
