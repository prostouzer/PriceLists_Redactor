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
        private readonly IPriceListsRedactorContext _db = new PriceListsRedactorContext();

        public ItemsController() { }

        public ItemsController(IPriceListsRedactorContext context)
        {
            _db = context;
        }

        // GET: Items
        public ActionResult Index()
        {
            var items = _db.Items.Include(i => i.PriceList);
            return View(items.ToList());
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            SelectList priceLists = new SelectList(_db.PriceLists, "Id", "Name");
            ViewBag.PriceLists = priceLists;
            return View();
        }

        public JsonResult GetColumnsOfAPriceList(int priceListId)
        {
            IEnumerable<Column> columns = _db.Columns.Where(c => c.PriceListId == priceListId);

            return Json( columns, JsonRequestBehavior.AllowGet);
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemAndCellsViewModel itemAndCells)
        {
            if (ModelState.IsValid)
            {
                var item = itemAndCells.Item;
                _db.Items.Add(item);
                _db.SaveChanges();

                foreach (var cell in itemAndCells.Cells)
                {
                    cell.ItemId = item.Id;
                    _db.Cells.Add(cell);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList priceLists = new SelectList(_db.PriceLists, "Id", "Name", itemAndCells.Item.PriceListId);
            ViewBag.PriceLists = priceLists;
            return View(itemAndCells);
        }

        public JsonResult InsertItemAndCells(Item item, IEnumerable<Cell> cells)
        {
            _db.Items.Add(item);
            _db.SaveChanges();

            foreach (Cell cell in cells)
            {
                cell.ItemId = item.Id;
                _db.Cells.Add(cell);
            }
            _db.SaveChanges();

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ItemAndCellsViewModel itemAndCells = new ItemAndCellsViewModel
            {
                Item = item,
                Cells = _db.Cells.Where(c => c.ItemId == item.Id)
            };
            ViewBag.PriceListId = new SelectList(_db.PriceLists, "Id", "Name", item.PriceListId);
            return View(itemAndCells);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemAndCellsViewModel itemAndCells)
        {
            if (ModelState.IsValid)
            {
                var item = itemAndCells.Item;
                _db.MarkAsModified(item);
                var cells = itemAndCells.Cells;
                foreach (Cell cell in cells)
                {
                    _db.MarkAsModified(cell);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PriceListId = new SelectList(_db.PriceLists, "Id", "Name", itemAndCells.Item.PriceListId);
            itemAndCells.Cells = _db.Cells.Where(c => c.ItemId == itemAndCells.Item.Id);
            return View(itemAndCells);
        }

        public JsonResult UpdateItemAndCells(Item item, IEnumerable<Cell> cells)
        {
            _db.MarkAsModified(item);

            foreach (Cell cell in cells)
            {
                _db.MarkAsModified(cell);
            }
            _db.SaveChanges();

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        public JsonResult UpdateItemInsertCells(Item item, IEnumerable<Cell> newCells)
        {
            _db.MarkAsModified(item);

            var oldCells = _db.Cells.Where(c => c.ItemId == item.Id);
            foreach (Cell cell in oldCells)
            {
                _db.Cells.Remove(cell);
            }

            foreach (Cell cell in newCells)
            {
                _db.Cells.Add(cell);
            }
            _db.SaveChanges();

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = _db.Items.Find(id);
            _db.Items.Remove(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
