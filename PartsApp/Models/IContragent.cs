using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public interface IContragent
    {
        //[Browsable(false)]
        int ContragentId { get; set; }
        [System.ComponentModel.DisplayName("Название")]
        string ContragentName { get; set; }
        [System.ComponentModel.DisplayName("ИНН/ОКПО")]
        string Code { get; set; }
        [System.ComponentModel.DisplayName("Юр./Физ. лицо")]
        string Entity { get; set; }
        [System.ComponentModel.Browsable(false)]
        ContactInfo ContactInfo { get; set; }
        [System.ComponentModel.DisplayName("Описание")]
        string Description { get; set; }
    }//IContragent


}//namespace

