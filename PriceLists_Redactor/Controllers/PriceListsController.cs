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

        //POST: PriceLists/Create=
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PriceListAndColumnsViewModel priceListAndColumns)
        {
            if (ModelState.IsValid)
            {
                db.PriceLists.Add(priceListAndColumns.PriceList);
                foreach (Column column in priceListAndColumns.Columns)
                {
                    db.Columns.Add(column);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(priceListAndColumns);
        }

        public async Task<ActionResult> InsertPriceListAndColumns(PriceListAndColumnsViewModel priceListAndColumns)
        {
            if (priceListAndColumns == null)
            {
                priceListAndColumns = new PriceListAndColumnsViewModel(new PriceList());
            }

            db.PriceLists.Add(priceListAndColumns.PriceList);
            await db.SaveChangesAsync();

            foreach (Column column in priceListAndColumns.Columns)
            {
                column.PriceListId = priceListAndColumns.PriceList.Id;
                db.Columns.Add(column);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public JsonResult InsertPricelist(PriceList priceList)
        {
            if (priceList == null)
            {
                priceList = new PriceList();
            }
            db.PriceLists.Add(priceList);
            var insertedRecords = db.SaveChanges();
            return Json(insertedRecords);
        }

        public JsonResult InsertColumns(IEnumerable<Column> columns)
        {
            if (columns == null)
            {
                columns = new List<Column>();
            }

            foreach (Column col in columns)
            {
                db.Columns.Add(col);
            }
            var insertedRecords = db.SaveChanges();
            return Json(insertedRecords);
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
