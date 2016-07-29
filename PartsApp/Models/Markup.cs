using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartsApp.SupportClasses;

namespace PartsApp.Models
{
    public static class Markup
    {
        public enum Types
        {
            [System.ComponentModel.Description("Мелкий опт")]
            SmallWholesale = 75,

            [System.ComponentModel.Description("Средний опт")]
            AverageWholesale = 50,

            [System.ComponentModel.Description("Крупный опт")]
            LargeWholesale = 30,

            [System.ComponentModel.Description("Розница")]
            Retail = 100
        }//Types

        public static string GetDescriptions(int markup)
        {
            if (markup <= 0)
                return "Уценка";

            switch (markup)
            {
                case (int)Types.Retail:
                    return Types.Retail.ToDescription();
                case (int)Types.SmallWholesale:
                    return Types.SmallWholesale.ToDescription();
                case (int)Types.AverageWholesale:
                    return Types.AverageWholesale.ToDescription();
                case (int)Types.LargeWholesale:
                    return Types.LargeWholesale.ToDescription();
                default:
                    return "Другая наценка";
            }//switch
        }//GetDescriptions

        public static Dictionary<int, string> GetValues()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            foreach (Markup.Types item in Enum.GetValues(typeof(Markup.Types)))
            {
                dict.Add((int)item, item.ToDescription());
            }//foreach

            return dict;
        }//GetValues
    }//Markup


}//namespace
