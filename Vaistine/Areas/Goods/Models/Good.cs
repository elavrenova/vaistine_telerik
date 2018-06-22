using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Areas.Docs.Models;
using Vaistine.Models;

namespace Vaistine.Areas.Goods.Models
{
    public class Good : DescrClass
    {
        public Guid? MainComponentId { get; set; }
        public virtual Component MainComponent { get; set; }
        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid CategoryId { get; set; }
        public virtual GoodCategory Category { get; set; }
        public virtual ICollection<DocLine> DocLines { get; set; }
    }
}
