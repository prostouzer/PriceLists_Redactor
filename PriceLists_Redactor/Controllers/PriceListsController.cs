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
            PriceList priceList = _db.PriceLists.Find(id);
            List<Column> columns = _db.Columns.Where(c => c.PriceListId == id).ToList();
            IEnumerable<Item> items = _db.Items.Where(i => i.PriceListId == id);
            IEnumerable<Cell> cells = _db.Cells.Where(c => c.Item.PriceListId == id);

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
            // предлагаем юзеру взять уже имеющиеся наборы колонок в новый прайс лист
            var allPriceLists = _db.PriceLists.ToList();
            ViewBag.AllPriceLists = allPriceLists;
            return View(new PriceListAndColumnsViewModel(new PriceList()));
        }

        public JsonResult UpdateItemTitle(int itemId, string newTitle)
        {
            var item = _db.Items.Single(i => i.Id == itemId);
            item.Title = newTitle;
            _db.SaveChanges();

            return Json(Url.Action("Index", "PriceLists"));
        }

        public JsonResult UpdateCell(int itemId, int cellIndex, string data)
        {
            var itemCells = _db.Cells.Where(c => c.ItemId == itemId).ToList();
            var cellToUpdate = itemCells[cellIndex];

            var cellEntity = _db.Cells.Single(c => c.Id == cellToUpdate.Id);
            cellEntity.Data = data;

            _db.SaveChanges();
            return Json(Url.Action("Index", "PriceLists"));
        }

        public JsonResult InsertPriceListAndColumns(PriceList priceList, IEnumerable<Column> columns)
        {
            if (priceList == null)
            {
                priceList = new PriceList();
            }

            _db.PriceLists.Add(priceList);
            _db.SaveChanges();

            if (columns!=null)
            {
                foreach (Column column in columns)
                {
                    column.PriceListId = priceList.Id;
                    _db.Columns.Add(column);
                }
                _db.SaveChanges();
            }

            // т.к. ajax-post запрос то нет смысла использовать RedirectToAction - не среагирует
            return Json(Url.Action("Index", "PriceLists"));
        }

        // GET: PriceLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = _db.PriceLists.Find(id);

            if (priceList == null)
            {
                return HttpNotFound();
            }

            List<Column> columns = _db.Columns.Where(c => c.PriceListId == priceList.Id).ToList();
            PriceListAndColumnsViewModel priceListAndColumns = new PriceListAndColumnsViewModel(priceList) { Columns = columns };

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
                foreach (Column column in columns)
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
            PriceList priceList = _db.PriceLists.Find(id);
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
            PriceList priceList = _db.PriceLists.Find(id);
            _db.PriceLists.Remove(priceList);
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
