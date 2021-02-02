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
            List<Column> columns = db.Columns.Where(c => c.PriceListId == id).ToList();
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
            // предлагаем юзеру добавить уже имеющиеся наборы колонок в новый прайс лист
            //SelectList allPriceLists = new SelectList(db.PriceLists.ToList(), "Id", "Name"); ViewBag.AllPriceLists = allPriceLists;
            var allPriceLists = db.PriceLists.ToList();
            ViewBag.AllPriceLists = allPriceLists;
            return View(new PriceListAndColumnsViewModel(new PriceList()));
        }

        public JsonResult UpdateItemTitle(int itemId, string newTitle)
        {
            var item = db.Items.Single(i => i.Id == itemId);
            item.Title = newTitle;
            db.SaveChanges();

            return Json(Url.Action("Index", "PriceLists"));
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

            List<Column> columns = db.Columns.Where(c => c.PriceListId == priceList.Id).ToList();
            PriceListAndColumnsViewModel priceListAndColumns = new PriceListAndColumnsViewModel(priceList) { Columns = columns };

            return View(priceListAndColumns);
        }

        // POST: PriceLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PriceListAndColumnsViewModel priceListAndColumns)
        {
            if (ModelState.IsValid)
            {
                var priceList = priceListAndColumns.PriceList;
                db.Entry(priceList).State = EntityState.Modified;
                var columns = priceListAndColumns.Columns;
                foreach (Column column in columns)
                {
                    db.Entry(column).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            priceListAndColumns.Columns = db.Columns.Where(c => c.PriceListId == priceListAndColumns.PriceList.Id).ToList();
            return View(priceListAndColumns);
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
