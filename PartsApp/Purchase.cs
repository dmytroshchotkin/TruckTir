using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    class Purchase : IOperation
    {
        public int OperationId { get; set; }
        public Employee Employee { get; set; }
        public IContragent Contragent { get; set; }
        public string ContragentEmployee { get; set; }
        public DateTime OperationDate { get; set; }
        public string Currency { get; set; }
        public double ExcRate { get; set; }
        public string Description { get; set; }
        public IList<SparePart> OperationDetails { get; set; }
          
        public Purchase() {}
        
    }//Purchase

}//namespace

/*
 для преобразования даты в целое число используется функция strftime('%s', value), а для обратного преобразования используется функция datetime(value, 'unixepoch').
*/