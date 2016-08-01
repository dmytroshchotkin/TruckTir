using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public interface IOperation
    {
        int OperationId { get; set; }
        Employee Employee { get; set; }
        IContragent Contragent { get; set; }
        string ContragentEmployee { get; set; }
        DateTime OperationDate { get; set; }
        string Currency { get; set; }
        double ExcRate { get; set; }
        string Description { get; set; }
        IList<SparePart> OperationDetails { get; set; }

    }//IOperation

}//namespace
