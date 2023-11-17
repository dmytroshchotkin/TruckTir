using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public interface IOperation
    {
        int         OperationId         { get; set; }
        Employee    Employee            { get; set; }
        IContragent Contragent          { get; set; }
        string      ContragentEmployee  { get; set; }
        DateTime    OperationDate       { get; set; }
        string      Description         { get; set; }

        List<OperationDetails> OperationDetailsList { get;}

        void TrySetOperationDetails(Lazy<List<OperationDetails>> operationDetails);

    }

}
