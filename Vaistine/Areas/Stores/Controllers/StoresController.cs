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
using Vaistine.Areas.Stores.Models;
using Vaistine.Data;

namespace Vaistine.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class StoresController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StoresController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Stores/Stores
        public IActionResult Index()
        {
            ViewData["ownerid"] = _db.Cags.ToList();
            return View();
        }

        public IActionResult Details(Guid id)
        {
            var store = _db.Stores
                    .Include(x=>x.Owner)
                .SingleOrDefault(x=>x.Id == id);
            if (store == null)
                return NotFound();

            return View(store);
        }

        #region Kendo Grid
        [AllowAnonymous]
        public IActionResult GetItems([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_db.Stores.ToDataSourceResult(request));
        }
        public async Task<IActionResult> Create(Store item)
        {
            ModelState["Id"].ValidationState = ModelValidationState.Valid;
            if (item != null && ModelState.IsValid)
            {
                _db.Add(item); await _db.SaveChangesAsync();
            }
            return Json(new[] { item }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, Store item)
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
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, Store item)
        {
            if (!_db.Docs.Any(x => x.FromStoreId == item.Id) && !_db.Docs.Any(x => x.ToStoreId == item.Id))
            {
                _db.Remove(item);
                _db.SaveChanges();
            }
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion

    }
}
