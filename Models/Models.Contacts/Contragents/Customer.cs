using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class Customer : IContragent
    {
        public int    ContragentId     { get; set; }
        public string ContragentName   { get; set; }
        public string Code             { get; set; }
        public string Entity           { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public string Description      { get; set; }
        public double? Balance         { get; set; }

        public Customer() { }
        public Customer(int contragentId, string contragentName, string code, string entity, ContactInfo contactInfo, string description, double? balance)
        { 
            ContragentId   = contragentId;
            ContragentName = contragentName;
            Code           = code;
            Entity         = entity;
            ContactInfo    = contactInfo;
            Description    = description;
            Balance        = balance;            
        }    
    }
}
