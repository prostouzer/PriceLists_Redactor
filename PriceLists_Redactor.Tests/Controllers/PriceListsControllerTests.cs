using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using PriceLists_Redactor.Controllers;
using PriceLists_Redactor.Data;
using PriceLists_Redactor.Models;
using PriceLists_Redactor.Tests.Fakes;
using System.Web.Mvc;
using PriceLists_Redactor.Models.ViewModels;

namespace PriceLists_Redactor.Tests.Controllers
{
    class PriceListsControllerTests
    {
        private IPriceListsRedactorContext _context;
        private PriceListsController _controller;

        [SetUp]
        public void SetUp()
        {
            _context = new FakePriceListsRedactorContext();

            _controller = new PriceListsController(_context);
        }

        [Test]
        public void UpdateItemTitle_IsOk()
        {
            //arrange
            var item1 = new Item() { Id = 1, PriceListId = 1, Title = "Item1" };
            var item2 = new Item() { Id = 2, PriceListId = 2, Title = "Item2" };
            _context.Items.Add(item1);
            _context.Items.Add(item2);

            var newTitle = "NEW Item2 TITLE";

            //act
            _controller.UpdateItemTitle(item2.Id, newTitle);

            //assert
            Assert.AreEqual(newTitle, item2.Title);
        }

        [Test]
        public void GetPriceListAndItemsViewModelFromPriceList_IsOk()
        {
            //arrange
            var priceList1 = new PriceList() { Id = 1, Name = "First priceList" };
            _context.PriceLists.Add(priceList1);

            var column1 = new Column() { Id = 1, DataType = DataType.Numeric, HeaderText = "Numeric", PriceListId = 1 };
            _context.Columns.Add(column1);

            var item1 = new Item() { Id = 1, PriceListId = 1, Title = "Item1" };
            _context.Items.Add(item1);

            var cell1 = new Cell() { Id = 1, Data = "1", Item = item1, ItemId = 1 };
            _context.Cells.Add(cell1);

            var expected = new PriceListAndItemsViewModel(new PriceList() {Id = priceList1.Id, Name = priceList1.Name}, 
                new List<Column>()
                {
                    new Column() { Id = 1, DataType = DataType.Numeric, HeaderText = "Numeric", PriceListId = 1 }
                }, 
                new List<Item>()
                {
                    new Item() { Id = 1, PriceListId = 1, Title = "Item1" }
                },
                new List<Cell>()
                {
                    new Cell() { Id = 1, Data = "1", Item = item1, ItemId = 1 }
                });

            //act
            var actual = _controller.GetPriceListAndItemsViewModelFromPriceList(priceList1);

            //assert
            Assert.AreEqual(expected.PriceList.Id, actual.PriceList.Id);
            Assert.AreEqual(expected.PriceList.Name, actual.PriceList.Name);

            Assert.AreEqual(expected.Columns.Single().Id, actual.Columns.Single().Id);
            Assert.AreEqual(expected.Columns.Single().DataType, actual.Columns.Single().DataType);
            Assert.AreEqual(expected.Columns.Single().HeaderText, actual.Columns.Single().HeaderText);
            Assert.AreEqual(expected.Columns.Single().PriceListId, actual.Columns.Single().PriceListId);

            Assert.AreEqual(expected.Items.Single().Id, actual.Items.Single().Id);
            Assert.AreEqual(expected.Items.Single().PriceListId, actual.Items.Single().PriceListId);
            Assert.AreEqual(expected.Items.Single().Title, actual.Items.Single().Title);

            Assert.AreEqual(expected.Cells.Single().Id, actual.Cells.Single().Id);
            Assert.AreEqual(expected.Cells.Single().Data, actual.Cells.Single().Data);
            Assert.AreEqual(expected.Cells.Single().ItemId, actual.Cells.Single().ItemId);
            Assert.AreEqual(expected.Cells.Single().Item.Id, actual.Cells.Single().Item.Id);
            Assert.AreEqual(expected.Cells.Single().Item.PriceListId, actual.Cells.Single().Item.PriceListId);
            Assert.AreEqual(expected.Cells.Single().Item.Title, actual.Cells.Single().Item.Title);
        }

        [Test]
        public void UpdateCell_IsOk()
        {
            //arrange
            var item1 = new Item() { Id = 1, PriceListId = 1, Title = "Item1" };
            _context.Items.Add(item1);
            var cell1 = new Cell() { Id = 1, Data = "123", Item = item1, ItemId = 1 };
            _context.Cells.Add(cell1);

            var newData = "321";

            //act
            _controller.UpdateCell(item1.Id, 0, newData);

            //assert
            Assert.AreEqual(newData, cell1.Data);
        }

        [Test]
        public void InsertPriceListAndColumns_IsOk()
        {
            //arrange
            var priceList1 = new PriceList() { Id = 1, Name = "First priceList" };

            List<Column> columns = new List<Column>()
            {
                new Column() { Id = 1, DataType = DataType.Numeric, HeaderText = "MyHeaderText1", PriceListId = priceList1.Id},
                new Column() { Id = 2, DataType = DataType.Bool, HeaderText = "MyHeaderText2", PriceListId = priceList1.Id}
            };

            //act
            _controller.InsertPriceListAndColumns(priceList1, columns);
            var actualPriceList = _context.PriceLists.Single();
            List<Column> actualColumns = _context.Columns.Where(c => c.PriceListId == priceList1.Id).ToList();

            //assert
            Assert.AreEqual(priceList1, actualPriceList);
            CollectionAssert.AreEqual(columns, actualColumns);
        }

        [Test]
        public void DeleteConfirmed()
        {
            //arrange
            var priceList1 = new PriceList() { Id = 1, Name = "First priceList" };
            _context.PriceLists.Add(priceList1);
            _controller.DeletePriceList(priceList1.Id);

            //act
            var priceList = _context.PriceLists.SingleOrDefault(p => p.Id == priceList1.Id);

            //assert
            Assert.AreEqual(null, priceList);
        }
    }

}
