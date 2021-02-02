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
    public class ItemsController : Controller
    {
        private PriceLists_RedactorContext db = new PriceLists_RedactorContext();

        // GET: Items
        public async Task<ActionResult> Index()
        {
            var items = db.Items.Include(i => i.PriceList);
            return View(await items.ToListAsync());
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            SelectList priceLists = new SelectList(db.PriceLists, "Id", "Name");
            ViewBag.PriceLists = priceLists;
            return View();
        }

        public JsonResult GetColumnsOfAPriceList(int priceListId)
        {
            IEnumerable<Column> columns = db.Columns.Where(c => c.PriceListId == priceListId);

            return Json( columns, JsonRequestBehavior.AllowGet);
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemAndCellsViewModel itemAndCells)
        {
            if (ModelState.IsValid)
            {
                var item = itemAndCells.Item;
                db.Items.Add(item);
                await db.SaveChangesAsync();

                foreach (var cell in itemAndCells.Cells)
                {
                    cell.ItemId = item.Id;
                    db.Cells.Add(cell);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            SelectList priceLists = new SelectList(db.PriceLists, "Id", "Name", itemAndCells.Item.PriceListId);
            ViewBag.PriceLists = priceLists;
            return View(itemAndCells);
        }

        public JsonResult InsertItemAndCells(Item item, IEnumerable<Cell> cells)
        {
            db.Items.Add(item);
            db.SaveChanges();

            foreach (Cell cell in cells)
            {
                cell.ItemId = item.Id;
                db.Cells.Add(cell);
            }
            db.SaveChanges();

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ItemAndCellsViewModel itemAndCells = new ItemAndCellsViewModel
            {
                Item = item,
                Cells = db.Cells.Where(c => c.ItemId == item.Id)
            };
            ViewBag.PriceListId = new SelectList(db.PriceLists, "Id", "Name", item.PriceListId);
            return View(itemAndCells);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ItemAndCellsViewModel itemAndCells)
        {
            if (ModelState.IsValid)
            {
                var item = itemAndCells.Item;
                db.Entry(item).State = EntityState.Modified;
                var cells = itemAndCells.Cells;
                foreach (Cell cell in cells)
                {
                    db.Entry(cell).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PriceListId = new SelectList(db.PriceLists, "Id", "Name", itemAndCells.Item.PriceListId);
            itemAndCells.Cells = db.Cells.Where(c => c.ItemId == itemAndCells.Item.Id);
            return View(itemAndCells);
        }

        public JsonResult UpdateItemAndCells(Item item, IEnumerable<Cell> cells)
        {
            db.Entry(item).State = EntityState.Modified;

            foreach (Cell cell in cells)
            {
                db.Entry(cell).State = EntityState.Modified;
            }
            db.SaveChanges();

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
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
