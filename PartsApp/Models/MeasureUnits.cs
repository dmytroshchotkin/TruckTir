using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using PartsApp.SupportClasses;

namespace PartsApp.Models
{
    public static class MeasureUnit
    {
        public enum Types
        {
            [System.ComponentModel.Description("шт.")]
            Piece,

            [System.ComponentModel.Description("кг.")]
            Kgs,

            [System.ComponentModel.Description("л.")]
            Liter,

            [System.ComponentModel.Description("ком.")]
            Set,
            
            [System.ComponentModel.Description("м.")]
            Meter //= 0.5f
        }//Types

        public static float GetMinUnitSale(string measureUnit)
        {
            if (measureUnit == Types.Piece.ToDescription())
                return 1;//(int)Types.Meter;

            if (measureUnit == Types.Meter.ToDescription())
                return 0.5f;

            if (measureUnit == Types.Kgs.ToDescription())
                return 1;//(int)Types.Kgs;

            if (measureUnit == Types.Liter.ToDescription())
                return 1;//(int)Types.Liter;

            if (measureUnit == Types.Set.ToDescription())
                return 1;//(int)Types.Set;

            
            
            throw new IndexOutOfRangeException("Нет такой единицы измерения.");
        }//GetDescription

        public static List<string> GetDescriptions()
        {
            List<string> unitsDescrptnsList = new List<string>();

            foreach (MeasureUnit.Types item in Enum.GetValues(typeof(MeasureUnit.Types)))
                unitsDescrptnsList.Add(item.ToDescription());
            
            return unitsDescrptnsList;
        }//GetDescriptions
    }//MeasureUnit

}//namespace
