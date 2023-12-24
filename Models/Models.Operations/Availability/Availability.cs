using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class Availability
    {
        public OperationDetails OperationDetails { get; set; }
        /// <summary>
        /// Адрес хранилища.
        /// </summary>
        public string StorageAddress { get; set; }
        /// <summary>
        /// Наценка.
        /// </summary>
        public float Markup { get; set; }
        /// <summary>
        /// Цена продажи.
        /// </summary>
        public float SellingPrice 
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
        }

        /// <summary>
        /// Возвращает или задает объект, содержащий данные об элементе управления.
        /// </summary>
        public object Tag { get; set; }


        public Availability(OperationDetails operationDetails, string storageAddress, float markup)
        {
            OperationDetails = operationDetails;
            StorageAddress   = storageAddress;
            Markup           = markup;
        }

        public Availability(Availability avail)
            : this(avail.OperationDetails, avail.StorageAddress, avail.Markup)
        {

        }


        /// <summary>
        /// Возвращает максимальную цену продажи из переданного списка.
        /// </summary>
        /// <param name="availabilityList">Список товаров в наличии.</param>
        /// <returns></returns>
        public static float GetMaxSellingPrice(IList<Availability> availabilityList)
        {
            return availabilityList.Max(av => av.SellingPrice);
        }

        /// <summary>
        /// Возвращает общее кол-во товара с основного и виртуального склада.
        /// </summary>
        /// <param name="availabilityList">Список товаров в наличии.</param>
        /// <returns></returns>
        public static float GetTotalCount(IList<Availability> availabilityList)
        {
            return availabilityList.Sum(av => av.OperationDetails.Count);
        }

        /// <summary>
        /// Возвращает список новых объектов созданного на основании переданного списка.
        /// </summary>
        /// <param name="availabilityList">Список объектов</param>
        /// <returns></returns>
        public static List<Availability> GetNewAvailabilityList(List<Availability> availabilityList)
        {
            List<Availability> newAvailList = new List<Availability>();
            foreach (Availability avail in availabilityList)
            {
                newAvailList.Add(new Availability(avail));
            }

            return newAvailList;
        }

    }

}
