using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var priceLists = new SelectList(_db.PriceLists, "Id", "Name");
            ViewBag.PriceLists = priceLists;
            return View();
        }

        public JsonResult GetColumnsOfAPriceList(int priceListId)
        {
            IEnumerable<Column> columns = _db.Columns.Where(c => c.PriceListId == priceListId);

            return Json( columns, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertItemAndCells(Item item, IEnumerable<Cell> cells)
        {
            AddItemAndCells(item, cells);

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        public void AddItemAndCells(Item item, IEnumerable<Cell> cells)
        {
            _db.Items.Add(item);
            _db.SaveChanges();

            foreach (var cell in cells)
            {
                cell.ItemId = item.Id;
                _db.Cells.Add(cell);
            }

            _db.SaveChanges();
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var itemAndCells = new ItemAndCellsViewModel
            {
                Item = item,
                Cells = _db.Cells.Where(c => c.ItemId == item.Id)
            };
            ViewBag.PriceListId = new SelectList(_db.PriceLists, "Id", "Name", item.PriceListId);
            return View(itemAndCells);
        }

        public JsonResult UpdateItemAndCellsJson(Item item, IEnumerable<Cell> cells)
        {
            UpdateItemAndCells(item, cells);

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        public void UpdateItemAndCells(Item item, IEnumerable<Cell> cells) // поменяли название И НЕ изменяли прайс-лист - старые колонки
        {
            _db.MarkAsModified(item);

            foreach (var cell in cells)
            {
                _db.MarkAsModified(cell);
            }

            _db.SaveChanges();
        }

        public JsonResult UpdateItemInsertCellsJson(Item item, IEnumerable<Cell> newCells) // поменяли название И изменили прайс-лист - новые колонки, следовательно ячейки надо не обновлять, а удалять и добавлять новые
        {
            UpdateItemInsertCells(item, newCells);

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "Items"));
        }

        public void UpdateItemInsertCells(Item item, IEnumerable<Cell> newCells)
        {
            _db.MarkAsModified(item);

            var oldCells = _db.Cells.Where(c => c.ItemId == item.Id).ToList();
            foreach (var cell in oldCells)
            {
                _db.Cells.Remove(cell);
            }

            foreach (var cell in newCells)
            {
                _db.Cells.Add(cell);
            }

            _db.SaveChanges();
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _db.Items.Find(id);
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
            DeleteItem(id);
            return RedirectToAction("Index");
        }

        public void DeleteItem(int id)
        {
            var item = _db.Items.Find(id);
            _db.Items.Remove(item);
            _db.SaveChanges();
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
