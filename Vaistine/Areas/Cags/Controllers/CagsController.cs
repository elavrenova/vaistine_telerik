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
using Vaistine.Areas.Cags.Models;
using Vaistine.Data;

namespace Vaistine.Areas.Cags.Controllers
{
    [Area("Cags")]
    public class CagsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CagsController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Cags/Cags
        public async Task<IActionResult> Index()
        {

            ViewBag.ParentId = new SelectList(_db.Cags.OrderBy(x => x.Descr), "Id", "Descr");
            return View();
        }

        // GET: Cags/Cags/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cag = await _db.Cags
                .Include(c => c.Parent)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cag == null)
            {
                return NotFound();
            }

            return View(cag);
        }

        #region Kendo Grid
        [AllowAnonymous]
        public IActionResult GetItems([DataSourceRequest] DataSourceRequest request)
        {

            return Json(_db.Cags.ToDataSourceResult(request,  e => new CagViewModel
            {
                Id = e.Id,
                Descr = e.Descr
            }));
        }
        public async Task<IActionResult> Create(Cag item)
        {
            ModelState["Id"].ValidationState = ModelValidationState.Valid;
            if (item != null && ModelState.IsValid)
            {
                _db.Add(item); await _db.SaveChangesAsync();
            }
            return Json(new[] { item }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, Cag item)
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
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, Cag item)
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
