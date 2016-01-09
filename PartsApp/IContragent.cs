using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PartsApp
{
    interface IContragent
    {
        [Browsable(false)]
        public int ContragentId { get; set; }
        [DisplayName("Название")]
        public string ContragentName { get; set; }
        [DisplayName("ИНН/ОКПО")]
        public string Code { get; set; }
        [DisplayName("Юр./Физ. лицо")]
        public string Entity { get; set; }
        [Browsable(false)]
        public ContactInfo ContactInfoId { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
    }//IContragent


}//namespace
