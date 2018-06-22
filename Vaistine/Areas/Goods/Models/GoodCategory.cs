using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Models;

namespace Vaistine.Areas.Goods.Models
{
    public class GoodCategory : DescrClass
    {
        public Guid? ParentId { get; set; }
        public virtual GoodCategory Parent { get; set; }
        public virtual ICollection<GoodCategory> Children { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
    }
}
