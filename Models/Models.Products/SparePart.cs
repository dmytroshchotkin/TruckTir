using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class SparePart
    {
        public int SparePartId { get; set; }
        public string Photo { get; set; }
        public string Manufacturer { get; set; }
        public string Articul { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MeasureUnit { get; set; }

        private Lazy<List<Availability>> _availabilityList;
        public List<Availability> AvailabilityList { get { return _availabilityList.Value; } }

        public SparePart() { }
        /// <summary>
        /// Конструктор для добавления нового объекта в БД.
        /// </summary>
        public SparePart(int sparePartId, string photo, string manufacturer, string articul,
                         string title, string description, string measureUnit)
        {
            SparePartId  = sparePartId;
            Photo        = photo;
            Manufacturer = manufacturer;
            Articul      = articul;
            Title        = title;
            Description  = description;
            MeasureUnit  = measureUnit;

            // исключить из домена
            //_availabilityList = new Lazy<List<Availability>>(() => PartsDAL.FindAvailability(this));
        }

        public SparePart(SparePart sparePart)
            : this (sparePart.SparePartId, sparePart.Photo, sparePart.Manufacturer, sparePart.Articul, sparePart.Title,
                    sparePart.Description, sparePart.MeasureUnit)
        {
           
        }

        /// <summary>
        /// Возвращает список новых объектов созданного на основании переданного списка.
        /// </summary>
        /// <param name="sparePartsList">Список объектов</param>
        /// <returns></returns>
        public static IList<SparePart> GetNewSparePartsList(IList<SparePart> sparePartsList)
        {
            IList<SparePart> newSparePartsList = new List<SparePart>(sparePartsList.Count);

            for (int i = 0; i < sparePartsList.Count; ++i)
                newSparePartsList.Add(new SparePart(sparePartsList[i]));

            return newSparePartsList;
        }

        public void TrySetAvailabilities(Lazy<List<Availability>> availabilities)
        {
            if (availabilities != null)
            {
                _availabilityList = availabilities;
            }
        }
    }

}
