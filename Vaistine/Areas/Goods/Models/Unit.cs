using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Models;

namespace Vaistine.Areas.Goods.Models
{
    public class Unit : DescrClass
    {
        public Guid? BaseUnitId { get; set; }
        public double Scale { get; set; }
        public virtual Unit BaseUnit { get; set; }
        public virtual ICollection<Unit> ChildrenUnits { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
        public string ShortDescr { get; set; }
    }
}
