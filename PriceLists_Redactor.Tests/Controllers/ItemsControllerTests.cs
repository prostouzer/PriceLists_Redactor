using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PriceLists_Redactor.Controllers;
using PriceLists_Redactor.Data;
using PriceLists_Redactor.Models;
using PriceLists_Redactor.Models.ViewModels;
using PriceLists_Redactor.Tests.Fakes;

namespace PriceLists_Redactor.Tests.Controllers
{
    internal class ItemsControllerTests
    {
        private IPriceListsRedactorContext _context;
        private ItemsController _controller;

        [SetUp]
        public void SetUp()
        {
            _context = new FakePriceListsRedactorContext();

            _controller = new ItemsController(_context);
        }

        [Test]
        public void AddItemAndCells_IsOk()
        {
            //arrange
            var expected = new ItemAndCellsViewModel()
            {
                Item = new Item() { Id = 1, PriceListId = 1, Title = "MyItem" },
                Cells = new List<Cell>()
                {
                    new Cell(){Id=1, Data="MyData", ItemId = 1}
                }
            };

            //act
            _controller.AddItemAndCells(expected.Item, expected.Cells);
            var actualItem = new Item() { Id = _context.Items.Single().Id, PriceListId = _context.Items.Single().PriceListId, Title = _context.Items.Single().Title };
            var actualCell = new Cell() { Id = _context.Cells.Single().Id, Data = _context.Cells.Single().Data, ItemId = _context.Cells.Single().ItemId };

            //assert
            Assert.AreEqual(expected.Item.Id, actualItem.Id);
            Assert.AreEqual(expected.Item.PriceListId, actualItem.PriceListId);
            Assert.AreEqual(expected.Item.Title, actualItem.Title);

            Assert.AreEqual(expected.Cells.Single().Id, actualCell.Id);
            Assert.AreEqual(expected.Cells.Single().Data, actualCell.Data);
            Assert.AreEqual(expected.Cells.Single().ItemId, actualCell.ItemId);
        }

        [Test]
        public void UpdateItemAndCells_IsOk()
        {
            //arrange
            var oldItem = new Item() { Id = 1, PriceListId = 1, Title = "OldTitle" };
            var oldCells = new List<Cell>()
            {
                new Cell(){ Id=1, Data="OldData", ItemId=1 }
            };
            _context.Items.Add(oldItem);
            foreach (var cell in oldCells)
            {
                _context.Cells.Add(cell);
            }

            var newItem = new Item() { Id = 1, PriceListId = 2, Title = "MyNewTitle" };
            var newCells = new List<Cell>()
            {
                new Cell(){ Id=1, Data="MyNewData", ItemId=1 }
            };
            
            //act
            _controller.UpdateItemAndCells(newItem, newCells);
            var actualItem = _context.Items.Single();
            var actualCells = new List<Cell>() { _context.Cells.Single() };

            //assert
            Assert.AreEqual(newItem.Id, actualItem.Id);
            Assert.AreEqual(newItem.PriceListId, actualItem.PriceListId);
            Assert.AreEqual(newItem.Title, actualItem.Title);

            Assert.AreEqual(newCells.Single().Id, actualCells.Single().Id);
            Assert.AreEqual(newCells.Single().Data, actualCells.Single().Data);
            Assert.AreEqual(newCells.Single().ItemId, actualCells.Single().ItemId);
        }

        [Test]
        public void UpdateItemInsertCells_IsOk()
        {
            //arrange
            var oldItem = new Item() { Id = 1, PriceListId = 1, Title = "OldTitle" };
            var oldCells = new List<Cell>()
            {
                new Cell(){ Id=1, Data="OldData", ItemId=1 }
            };
            _context.Items.Add(oldItem);
            foreach (var cell in oldCells)
            {
                _context.Cells.Add(cell);
            }

            var newItem = new Item() { Id = 1, PriceListId = 2, Title = "MyNewTitle" };
            var newCells = new List<Cell>()
            {
                new Cell(){ Data="MyNewData", ItemId=1 }
            };

            //act
            _controller.UpdateItemInsertCells(newItem, newCells);
            var actualItem = _context.Items.Single();
            var actualCells = new List<Cell>() { _context.Cells.Single() };

            //assert
            Assert.AreEqual(newItem.Id, actualItem.Id);
            Assert.AreEqual(newItem.PriceListId, actualItem.PriceListId);
            Assert.AreEqual(newItem.Title, actualItem.Title);

            Assert.AreEqual(newCells.Single().Data, actualCells.Single().Data);
            Assert.AreEqual(newCells.Single().ItemId, actualCells.Single().ItemId);
        }

        [Test]
        public void DeleteItem_IsOk()
        {
            //arrange
            var item = new Item() { Id=1, PriceListId = 999, Title = "ItemToRemove"};
            _context.Items.Add(item);

            //act
            _controller.DeleteItem(item.Id);

            //assert
            Assert.AreEqual(false, _context.Items.Any());
        }
    }
}
