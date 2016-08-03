using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class ContactInfo
    {
        public int    ContactInfoId { get; set; }
        public string Country       { get; set; }
        public string Region        { get; set; }
        public string City          { get; set; }
        public string Street        { get; set; }
        public string House         { get; set; }
        public string Room          { get; set; }
        public string Phone         { get; set; }
        public string ExtPhone      { get; set; }
        public string Website       { get; set; }
        public string Email         { get; set; }


        public ContactInfo() { }
        public ContactInfo(int contactInfoId, string country, string region, string city, string street, string house,
                           string room, string phone, string extPhone, string website, string email)
        {
            ContactInfoId   = contactInfoId;
            Country         = country;
            Region          = region;
            City            = city;
            Street          = street;
            House           = house;
            Room            = room;
            Phone           = phone;
            ExtPhone        = extPhone;
            Website         = website;
            Email           = email;
        }//

    }//ContactInfo

}//namespace
