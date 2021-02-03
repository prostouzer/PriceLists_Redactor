using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PriceLists_Redactor.Controllers;
using PriceLists_Redactor.Data;
using PriceLists_Redactor.Models;
using PriceLists_Redactor.Models.ViewModels;
using PriceLists_Redactor.Tests.Fakes;

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
        public void GetDetails()
        {
            //arrange
            var priceList1 = new PriceList() { Id = 1, Name = "First priceList" };
            var priceList2 = new PriceList() {Id = 2, Name = "Second priceList"};

            var item1 = new Item() { Id = 1, PriceListId = 1, Title = "Item1" };
            var item2 = new Item() { Id = 2, PriceListId = 2, Title = "Item2" };

            PriceListAndItemsViewModel viewModel;

            //act


            //assert

        }
    }
}
