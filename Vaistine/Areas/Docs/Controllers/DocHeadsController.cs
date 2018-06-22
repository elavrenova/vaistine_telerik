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
using Vaistine.Areas.Docs.Models;
using Vaistine.Data;

namespace Vaistine.Areas.Docs.Controllers
{
    [Area("Docs")]
    public class DocHeadsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DocHeadsController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: DocHeads
        public IActionResult Index()
        {
            ViewBag.StoreId = new SelectList(_db.Stores.OrderBy(x => x.Descr), "Id", "Descr");
            ViewBag.CagId = new SelectList(_db.Cags.OrderBy(x => x.Descr), "Id", "Descr");
            return View();
        }

        #region Kendo Grid
        [AllowAnonymous]
        public IActionResult GetInDocs([DataSourceRequest] DataSourceRequest request, Guid StoreId)
        {
            return Json(_db.Docs.Where(x=>x.ToStoreId == StoreId).ToDataSourceResult(request));
        }

        [AllowAnonymous]
        public IActionResult GetOutDocs([DataSourceRequest] DataSourceRequest request, Guid StoreId)
        {
            return Json(_db.Docs.Where(x => x.FromStoreId == StoreId).ToDataSourceResult(request));
        }

        public IActionResult GetItems([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_db.Docs.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Create(DocHead item)
        {
            ModelState["Id"].ValidationState = ModelValidationState.Valid;
            if (item != null && ModelState.IsValid)
            {
                _db.Add(item); await _db.SaveChangesAsync();
            }
            return Json(new[] { item }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        //public async Task<ActionResult> CreateInDoc (DocHead item, Guid InOlolo)
        //{
        //    ModelState["Id"].ValidationState = ModelValidationState.Valid;
        //    if (item != null && ModelState.IsValid)
        //    {
        //        item.ToStoreId = InOlolo;
        //        _db.Add(item); await _db.SaveChangesAsync();
        //    }
        //    return Json(new[] { item }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        //}
        //public async Task<ActionResult> CreateOutDoc(DocHead item, Guid OutOlolo)
        //{
        //    ModelState["Id"].ValidationState = ModelValidationState.Valid;
        //    if (item != null && ModelState.IsValid)
        //    {
        //        item.FromStoreId = OutOlolo;
        //        _db.Add(item); await _db.SaveChangesAsync();
        //    }
        //    return Json(new[] { item }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        //}

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, DocHead item)
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
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, DocHead item)
        {
            if (!_db.DocLines.Any(x => x.DocHeadId == item.Id))
            {
                _db.Remove(item);
                _db.SaveChanges();
            }
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion


    }
}
