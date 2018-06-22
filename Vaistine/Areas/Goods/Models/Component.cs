using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Models;

namespace Vaistine.Areas.Goods.Models
{
    public class Component : DescrClass
    {
        public virtual ICollection<Good> Goods { get; set; }
    }
}
