using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class Availability
    {
        OperationDetails OperationDetails { get; set; }
        /// <summary>
        /// Адрес хранилища.
        /// </summary>
        string StorageAddress { get; set; }
        /// <summary>
        /// Наценка.
        /// </summary>
        float Markup { get; set; }
        /// <summary>
        /// Цена продажи.
        /// </summary>
        float SellingPrice 
        { 
            get 
            {
                return OperationDetails.Price + (OperationDetails.Price * Markup / 100);   
            }
            set 
            { 
                //Меняем наценку.
                Markup = (value * 100 / OperationDetails.Price) - 100; 
            }
        }//

        public Availability(OperationDetails operationDetails, string storageAddress, float markup)
        {
            OperationDetails = operationDetails;
            StorageAddress   = storageAddress;
            Markup           = markup;
        }//

    }//Availability

}//namespace
