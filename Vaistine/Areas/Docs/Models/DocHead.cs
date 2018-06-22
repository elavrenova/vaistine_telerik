using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Areas.Cags.Models;
using Vaistine.Areas.Stores.Models;
using Vaistine.Models;

namespace Vaistine.Areas.Docs.Models
{
    public class DocHead : DescrClass
    {
        [Display(Name = "Откуда")]
        public Guid FromStoreId { get; set; }

        [Display(Name = "Куда")]
        public Guid ToStoreId { get; set; }

        public virtual Store FromStore { get; set; }
        public virtual Store ToStore { get; set; }

        [Display(Name = "Дата отгрузки")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Дата приема")]
        public DateTime ToDate { get; set; }

        [Display(Name="От кого")]
        public Guid FromCagId { get; set; }

        [Display(Name = "Кому")]
        public Guid ToCagId { get; set; }

        public virtual Cag FromCag { get; set; }
        public virtual Cag ToCag { get; set; }

        public ICollection<DocLine> DocLines { get; set; }
    }
}
