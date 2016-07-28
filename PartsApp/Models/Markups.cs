using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public enum Markups
    {
        [System.ComponentModel.Description("Розница")]
        Retail = 100,
        [System.ComponentModel.Description("Мелкий опт")]
        SmallWholesale = 75,
        [System.ComponentModel.Description("Средний опт")]
        AverageWholesale = 50,
        [System.ComponentModel.Description("Крупный опт")]
        LargeWholesale = 30,
    }//Markups

}//namespace
