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
        private readonly IPriceListsRedactorContext _db = new PriceListsRedactorContext();

        public PriceListsController() { }

        public PriceListsController(IPriceListsRedactorContext context)
        {
            _db = context;
        }

        // GET: PriceLists
        public ActionResult Index()
        {
            return View(_db.PriceLists.ToList());
        }

        // GET: PriceLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var priceList = _db.PriceLists.Find(id);
            var priceListWithItems = GetPriceListAndItemsViewModelFromPriceList(priceList);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceListWithItems);
        }

        public PriceListAndItemsViewModel GetPriceListAndItemsViewModelFromPriceList(PriceList priceList)
        {
            var columns = _db.Columns.Where(c => c.PriceListId == priceList.Id).ToList();
            IEnumerable<Item> items = _db.Items.Where(i => i.PriceListId == priceList.Id);
            IEnumerable<Cell> cells = _db.Cells.Where(c => c.Item.PriceListId == priceList.Id);

            var priceListWithItems = new PriceListAndItemsViewModel(priceList, columns, items, cells);
            return priceListWithItems;
        }

        // GET: PriceLists/Create
        public ActionResult Create()
        {
            // предлагаем юзеру взять уже имеющиеся наборы колонок в новый прайс лист
            var allPriceLists = _db.PriceLists.ToList();
            ViewBag.AllPriceLists = allPriceLists;
            return View(new PriceListAndColumnsViewModel(new PriceList()));
        }

        public JsonResult UpdateItemTitleJson(int itemId, string newTitle)
        {
            UpdateItemTitle(itemId, newTitle);

            return Json(Url.Action("Index", "PriceLists"));
        }

        public void UpdateItemTitle(int itemId, string newTitle)
        {
            var item = _db.Items.Single(i => i.Id == itemId);
            item.Title = newTitle;

            _db.SaveChanges();
        }

        public JsonResult UpdateCellJson(int itemId, int cellIndex, string data)
        {
            UpdateCell(itemId, cellIndex, data);
            return Json(Url.Action("Index", "PriceLists"));
        }

        public void UpdateCell(int itemId, int cellIndex, string data)
        {
            var itemCells = _db.Cells.Where(c => c.ItemId == itemId).ToList();
            var cellToUpdate = itemCells[cellIndex];

            var cellEntity = _db.Cells.Single(c => c.Id == cellToUpdate.Id);
            cellEntity.Data = data;

            _db.SaveChanges();
        }

        public JsonResult InsertPriceListAndColumnsJson(PriceList priceList, IEnumerable<Column> columns)
        {
            InsertPriceListAndColumns(priceList, columns);

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "PriceLists"));
        }

        public void InsertPriceListAndColumns(PriceList priceList, IEnumerable<Column> columns)
        {
            if (priceList == null)
            {
                priceList = new PriceList();
            }

            _db.PriceLists.Add(priceList);
            _db.SaveChanges();

            if (columns == null) return;
            foreach (var column in columns)
            {
                column.PriceListId = priceList.Id;
                _db.Columns.Add(column);
            }

            _db.SaveChanges();
        }

        // GET: PriceLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var priceList = _db.PriceLists.Find(id);

            if (priceList == null)
            {
                return HttpNotFound();
            }

            var columns = _db.Columns.Where(c => c.PriceListId == priceList.Id).ToList();
            var priceListAndColumns = new PriceListAndColumnsViewModel(priceList) { Columns = columns };

            return View(priceListAndColumns);
        }

        // POST: PriceLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PriceListAndColumnsViewModel priceListAndColumns)
        {
            if (ModelState.IsValid)
            {
                var priceList = priceListAndColumns.PriceList;
                _db.MarkAsModified(priceList);
                var columns = priceListAndColumns.Columns;
                foreach (var column in columns)
                {
                    _db.MarkAsModified(column);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            priceListAndColumns.Columns = _db.Columns.Where(c => c.PriceListId == priceListAndColumns.PriceList.Id).ToList();
            return View(priceListAndColumns);
        }

        // GET: PriceLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var priceList = _db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // POST: PriceLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeletePriceList(id);
            return RedirectToAction("Index");
        }

        public void DeletePriceList(int id)
        {
            var priceList = _db.PriceLists.Find(id);
            _db.PriceLists.Remove(priceList);
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
