using E_CommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Domain.Entities
{
    public class File : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }

        public string Storage { get; set; } // dosyanın depolandığı servis, yer neresi ? (Azure , Local, AWS ?)

        // File tablosunda UpdatedDate alanını eklememek için böyle yaptık
        // çünkü bir dosya ya eklenir yada silinir. Güncelleme olmaz
        [NotMapped]
        public override DateTime? UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }
    }
}
