using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vaistine.Areas.Goods.Models;
using Vaistine.Data;

namespace Vaistine.Areas.Goods.Controllers
{
    [Area("Goods")]
    public class GoodsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GoodsController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Goods/Goods
        public IActionResult Index()
        {
            return View();
        }

        // GET: Goods/Goods/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var good = await _db.Goods
                .Include(g => g.Category)
                .Include(g => g.MainComponent)
                .Include(g => g.Unit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (good == null)
            {
                return NotFound();
            }

            return View(good);
        }

        #region Kendo Grid
        [AllowAnonymous]
        public IActionResult GetItems([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_db.Goods.ToDataSourceResult(request));
        }
        public async Task<IActionResult> Create(Good item)
        {
            ModelState["Id"].ValidationState = ModelValidationState.Valid;
            if (item != null && ModelState.IsValid)
            {
                _db.Add(item); await _db.SaveChangesAsync();
            }
            return Json(new[] { item }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, Good item)
        {
            ModelState["Id"].ValidationState = ModelValidationState.Valid;
            if (item != null && ModelState.IsValid)
            {
                _db.Update(item);
                _db.SaveChanges();
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, Good item)
        {
            //if (!_db.Lots.Any(x => x.AuId == item.Id))
            //{
                _db.Remove(item);
                _db.SaveChanges();
            //}
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion

    }
}
