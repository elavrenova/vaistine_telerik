using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaistine.Areas.Goods.Models;
using Vaistine.Models;

namespace Vaistine.Areas.Docs.Models
{
    public class DocLine : BaseClass
    {
        public Guid GoodId { get; set; }
        public virtual Good Good { get; set; }
        public Guid DocHeadId { get; set; }
        public virtual DocHead DocHead { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
