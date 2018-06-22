using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vaistine.Data;

namespace Vaistine.Areas.Stores.ViewComponents
{
    public class StoreInDocsVC : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public StoreInDocsVC(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// возвращает входные документы по id склада
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            ViewBag.StoreId = id;
            ViewData["FromStoreId"] = _db.Stores.Where(x=>x.Id != id).ToList();
            ViewData["FromCagId"] = _db.Cags.ToList();
            ViewData["ToCagId"] = _db.Cags.ToList();
            var docs = _db.Docs.Where(x => x.ToStoreId == id);
            return View(await docs.ToListAsync());
        }
    }
}
