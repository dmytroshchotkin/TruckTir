using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PartsApp
{
    public interface IContragent
    {
        //[Browsable(false)]
        int ContragentId { get; set; }
        [DisplayName("Название")]
        string ContragentName { get; set; }
        [DisplayName("ИНН/ОКПО")]
        string Code { get; set; }
        [DisplayName("Юр./Физ. лицо")]
        string Entity { get; set; }
        [Browsable(false)]
        ContactInfo ContactInfo { get; set; }
        [DisplayName("Описание")]
        string Description { get; set; }
    }//IContragent


}//namespace
