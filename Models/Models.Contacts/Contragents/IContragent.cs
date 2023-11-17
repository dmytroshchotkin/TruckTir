using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public interface IContragent
    {
        int    ContragentId     { get; set; }
        string ContragentName   { get; set; }
        string Code             { get; set; }
        string Entity           { get; set; }
        ContactInfo ContactInfo { get; set; }
        string Description      { get; set; }
        double Balance         { get; set; }
    }

}

