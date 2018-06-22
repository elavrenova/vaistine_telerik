using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Areas.Cags.Models;
using Vaistine.Areas.Docs.Models;
using Vaistine.Models;

namespace Vaistine.Areas.Stores.Models
{
    public class Store : DescrClass
    {
        [Display(Name = "Учетный")]
        public bool IsAccount { get; set; }
        [Display(Name = "Владелец")]
        public Guid OwnerId { get; set; }
        [Display(Name = "Владелец")]
        public virtual Cag Owner { get; set; }
        public virtual ICollection<DocHead> InDocs { get; set; }
        public virtual ICollection<DocHead> OutDocs { get; set; }
    }
}
