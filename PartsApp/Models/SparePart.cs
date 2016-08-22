using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    //public class SparePart
    //{
    //    #region Св-ва класса.
    //    public int SparePartId { get; set; }
    //    [System.ComponentModel.DisplayName("Фото")]
    //    public string Photo { get; set; }
    //    public int PurchaseId { get; set; }
    //    [System.ComponentModel.DisplayName("Поставщик")]
    //    public string SupplierName { get; set; }
    //    [System.ComponentModel.DisplayName("Производитель")]
    //    public string Manufacturer { get; set; }
    //    public int? ManufacturerId { get; set; }
    //    [System.ComponentModel.DisplayName("Артикул")]
    //    public string Articul { get; set; }
    //    [System.ComponentModel.DisplayName("Название")]
    //    public string Title { get; set; }
    //    [System.ComponentModel.DisplayName("Описание")]
    //    public string Description { get; set; }

    //    [System.ComponentModel.DisplayName("Адрес склада")]
    //    public string StorageAdress { get; set; }
    //    [System.ComponentModel.DisplayName("Ед. изм.")]
    //    public string MeasureUnit { get; set; }
    //    public double Count { get; set; }
    //    public double VirtCount { get; set; }
    //    [System.ComponentModel.Browsable(false)]
    //    private string _avaliability;
    //    [System.ComponentModel.DisplayName("Наличие")]
    //    public string Avaliability
    //    {
    //        get { return (VirtCount == 0) ? Count.ToString() : (Count == 0) ? String.Format("({0})", VirtCount) : String.Format("{0} ({1})", Count, VirtCount); }
    //        set { _avaliability = value; }
    //    }

    //    [System.ComponentModel.DisplayName("Цена")]
    //    public double? Price { get; set; }
    //    [System.ComponentModel.Browsable(false)]
    //    public double? Markup { get; set; }
    //    [System.ComponentModel.DisplayName("Тип наценки")]
    //    public string MarkupType { get; set; }
    //    [System.ComponentModel.Browsable(false)]
    //    private double _excRate = 1;
    //    [System.ComponentModel.Browsable(false)]
    //    public double ExcRate
    //    {
    //        get { return _excRate; }
    //        set { _excRate = value; }
    //    }
    //    //[Browsable(false)]
    //    [System.ComponentModel.DisplayName("Цена продажи")]
    //    public double? SellingPrice
    //    {
    //        get
    //        {
    //            //return (Price == null || Markup == null) ? (double?)null : Math.Round(((double)(Price + (Price * Markup / 100)) * ExcRate), 2, MidpointRounding.AwayFromZero);
    //            if (Price == null || Markup == null)//|| Markup == 0)
    //                return null;
    //            else
    //            {
    //                double sellPrice = (double)(Price + (Price * Markup / 100)) / ExcRate;
    //                return Math.Round(sellPrice, 2, MidpointRounding.AwayFromZero);
    //            }//else
    //        }//get
    //        set { Markup = (value * 100 / Price) - 100; }
    //    }
    //    #endregion

    //    public SparePart() { }
    //    public SparePart(SparePart operDet)
    //    {
    //        this.SparePartId = operDet.SparePartId;
    //        this.Photo = operDet.Photo;
    //        this.Articul = operDet.Articul;
    //        this.Title = operDet.Title;
    //        this.Description = operDet.Description;
    //        this.ManufacturerId = operDet.ManufacturerId;
    //        this.Manufacturer = (ManufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(ManufacturerId);/*!!!*/
    //        this.MeasureUnit = operDet.MeasureUnit;
    //        this.Count = operDet.Count;
    //        this.VirtCount = operDet.VirtCount;
    //        this.StorageAdress = operDet.StorageAdress;
    //        this.Price = operDet.Price;
    //        this.Markup = operDet.Markup;
    //        this.MarkupType = operDet.MarkupType;
    //        this.ExcRate = operDet.ExcRate;
    //        this.PurchaseId = operDet.PurchaseId;
    //    }//

    //    public SparePart(int sparePartId, string photo, string articul, string title, string description,
    //                     int? manufacturerId, string measureUnit)
    //    {
    //        this.SparePartId = sparePartId;
    //        this.Photo = photo;
    //        this.Articul = articul;
    //        this.Title = title;
    //        this.Description = description;
    //        this.ManufacturerId = manufacturerId;
    //        this.Manufacturer = (ManufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(ManufacturerId);
    //        this.MeasureUnit = measureUnit;
    //    }//

    //    public SparePart(int sparePartId, string photo, string articul, string title, string description,
    //                     int? manufacturerId, int purchaseId, string measureUnit, string storageAdress, double sellCount,
    //                     double price, double? markup)
    //    {
    //        this.SparePartId = sparePartId;
    //        this.Photo = photo;
    //        this.Articul = articul;
    //        this.Title = title;
    //        this.Description = description;
    //        this.ManufacturerId = manufacturerId;
    //        this.Manufacturer = (manufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(manufacturerId); /*!!!*/
    //        this.MeasureUnit = measureUnit;
    //        this.PurchaseId = purchaseId;
    //        this.SupplierName = PartsDAL.FindSupplierByPurchaseId(purchaseId).ContragentName; /*!!!*/
    //        if (storageAdress == null) this.Count = sellCount;
    //        else this.VirtCount = sellCount;
    //        this.StorageAdress = storageAdress;
    //        this.Price = price;
    //        this.Markup = markup;
    //        this.MarkupType = (markup == null) ? null : Models.Markup.GetDescription((float)markup);
    //    }//

    //}//SparePart

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
        public SparePart(int sparePartId, string photo, string manufacturer, string articul,
                         string title, string description, string measureUnit)
        {
            SparePartId = sparePartId;
            Photo = photo;
            Manufacturer = manufacturer;
            Articul = articul;
            Title = title;
            Description = description;
            MeasureUnit = measureUnit;

            _availabilityList = new Lazy<List<Availability>>(() => PartsDAL.FindAvailability(this));
        }//

        public SparePart(string photo, string manufacturer, string articul,
                         string title, string description, string measureUnit, List<Availability> availabilityList)
        {
            Photo = photo;
            Manufacturer = manufacturer;
            Articul = articul;
            Title = title;
            Description = description;
            MeasureUnit = measureUnit;

            _availabilityList = new Lazy<List<Availability>>(() => availabilityList);
        }//

        public SparePart(SparePart sparePart)
            : this (sparePart.SparePartId, sparePart.Photo, sparePart.Manufacturer, sparePart.Articul, sparePart.Title,
                    sparePart.Description, sparePart.MeasureUnit)
        {
           this._availabilityList = new Lazy<List<Availability>>(() => Availability.GetNewAvailabilityList(sparePart.AvailabilityList));
        }//

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
        }//GetNewSparePartsList
    }//SparePart

}//namespace
