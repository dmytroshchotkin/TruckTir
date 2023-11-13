using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Helper;

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
        }

        public static string GetDescription(float markup)
        {
            if (markup <= 0)
            {
                return "Уценка";
            }                

            int errorMargin = 5; //погрешность от значения Markup.Types.
            //Если 'Розница'.
            if (markup >= (int)Types.Retail - errorMargin && markup <= (int)Types.Retail + errorMargin)
            {
                return Types.Retail.ToDescription();
            }                

            //Если 'Малый опт'.
            if (markup >= (int)Types.SmallWholesale - errorMargin && markup <= (int)Types.SmallWholesale + errorMargin)
            {
                return Types.SmallWholesale.ToDescription();
            }                

            //Если 'Средний опт'.
            if (markup >= (int)Types.AverageWholesale - errorMargin && markup <= (int)Types.AverageWholesale + errorMargin)
            {
                return Types.AverageWholesale.ToDescription();
            }                

            //Если 'Крупный опт'.
            if (markup >= (int)Types.LargeWholesale - errorMargin && markup <= (int)Types.LargeWholesale + errorMargin)
            {
                return Types.LargeWholesale.ToDescription();
            }                

            return "Другая наценка";
        }

        public static IEnumerable<KeyValuePair<int, string>> GetValues()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            foreach (Markup.Types item in Enum.GetValues(typeof(Markup.Types)))
            {
                dict.Add((int)item, item.ToDescription());
            }

            return dict.Reverse();
        }
    }


}
