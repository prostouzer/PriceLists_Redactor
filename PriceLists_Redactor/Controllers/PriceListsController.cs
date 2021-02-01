using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PriceLists_Redactor.Data;
using PriceLists_Redactor.Models;
using PriceLists_Redactor.Models.ViewModels;

namespace PriceLists_Redactor.Controllers
{
    public class PriceListsController : Controller
    {
        private PriceLists_RedactorContext db = new PriceLists_RedactorContext();

        // GET: PriceLists
        public async Task<ActionResult> Index()
        {
            return View(await db.PriceLists.ToListAsync());
        }

        // GET: PriceLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = await db.PriceLists.FindAsync(id);
            IEnumerable<Column> columns = db.Columns.Where(c => c.PriceListId == id);
            IEnumerable<Item> items = db.Items.Where(i => i.PriceListId == id);
            IEnumerable<Cell> cells = db.Cells.Where(c => c.Item.PriceListId == id);

            PriceListAndItemsViewModel priceListWithItems = new PriceListAndItemsViewModel(priceList, columns, items, cells);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceListWithItems);
        }

        // GET: PriceLists/Create
        public ActionResult Create()
        {
            return View(new PriceListAndColumnsViewModel(new PriceList()));
        }

        public JsonResult InsertPriceListAndColumns(PriceList priceList, IEnumerable<Column> columns)
        {
            if (priceList == null)
            {
                priceList = new PriceList();
            }

            db.PriceLists.Add(priceList);
            db.SaveChanges();

            if (columns!=null)
            {
                foreach (Column column in columns)
                {
                    column.PriceListId = priceList.Id;
                    db.Columns.Add(column);
                }
                db.SaveChanges();
            }

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "PriceLists"));
        }

        // GET: PriceLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = await db.PriceLists.FindAsync(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // POST: PriceLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priceList).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(priceList);
        }

        // GET: PriceLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = await db.PriceLists.FindAsync(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // POST: PriceLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PriceList priceList = await db.PriceLists.FindAsync(id);
            db.PriceLists.Remove(priceList);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
