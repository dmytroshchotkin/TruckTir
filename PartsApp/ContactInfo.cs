using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    public class ContactInfo
    {
        public int ContactInfoId { get; set; }
        public string Country    { get; set; }
        public string Region     { get; set; }
        public string City       { get; set; }
        public string Street     { get; set; }
        public string House      { get; set; }
        public string Room       { get; set; }  
        public string Phone      { get; set; }
        public string ExtPhone1  { get; set; }
        public string Website    { get; set; }
        public string Email      { get; set; }

          
        public ContactInfo() { }
        public ContactInfo(string country, string region, string city, string street, string house,
                           string room, string phone, string extphone1, string website, string email)
        {
            Country = country;
            Region = region;
            City = city;
            Street = street;
            House = house;
            Room = room;
            Phone = phone;
            ExtPhone1 = extphone1;
            Website = website;
            Email = email;
        }//
        public ContactInfo(int contactinfoid, string country, string region, string city, string street, string house,      
                           string room, string phone, string extphone1, string website, string email)
        {             
           ContactInfoId = contactinfoid;
           Country       = country;
           Region        = region;
           City          = city;
           Street        = street;
           House         = house;
           Room          = room; 
           Phone         = phone;
           ExtPhone1     = extphone1;
           Website       = website;
           Email         = email;
        }//
           
              
          
    }//ContactInfo


}//namespace
