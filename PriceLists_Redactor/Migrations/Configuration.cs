using System.Collections.Generic;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PriceLists_Redactor.Data.PriceLists_RedactorContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "PriceLists_Redactor.Data.PriceLists_RedactorContext";
        }

        protected override void Seed(PriceLists_Redactor.Data.PriceLists_RedactorContext context)
        {
            var priceList1 = new PriceList() { Name = "Прайс-лист мебели" };
            var priceList2 = new PriceList() { Name = "Прайс-лист одежды" };
            context.PriceLists.AddOrUpdate(priceList1);
            context.PriceLists.AddOrUpdate(priceList2);
            context.SaveChanges();

            var column1 = new Column() {DataType = DataType.SingleLine, HeaderText = "Моя однострочная колонка", PriceListId = 1 };
            var column2 = new Column() { DataType = DataType.Bool, HeaderText = "В наличии", PriceListId = 1 };
            var column3 = new Column() { DataType = DataType.MultiLine, HeaderText = "Моя МНОГОСТРОЧНАЯ колонка", PriceListId = 2 };
            var column4 = new Column() { DataType = DataType.Bool, HeaderText = "Кол-во на складе", PriceListId = 2 };
            context.Columns.AddOrUpdate(column1);
            context.Columns.AddOrUpdate(column2);
            context.Columns.AddOrUpdate(column3);
            context.Columns.AddOrUpdate(column4);
            context.SaveChanges();

            var item1 = new Item() { Title = "Обеденный стол", PriceListId = 1 };
            var item2 = new Item() { Title = "Стул", PriceListId = 2 };
            var item3 = new Item() { Title = "Деревянный шкаф", PriceListId = 1 };
            context.Items.AddOrUpdate(item1);
            context.Items.AddOrUpdate(item2);
            context.Items.AddOrUpdate(item3);
            context.SaveChanges();

            var cell1 = new Cell() { Data = "Просто стол!", ItemId = 1 };
            var cell2 = new Cell() { Data = "1", ItemId = 1 };
            var cell3 = new Cell() { Data = "Хороший, деревянный стул. ТЕСТ МНОГОСТРОЧНОЙ КОЛОНКИ. ТЕСТ МНОГОСТРОЧНОЙ КОЛОНКИ", ItemId = 2 };
            var cell4 = new Cell() { Data = "10", ItemId = 2 };
            var cell5 = new Cell() { Data = "Вторая однострочная колонка", ItemId = 3 };
            var cell6 = new Cell() { Data = "0", ItemId = 3 };
            context.Cells.AddOrUpdate(cell1);
            context.Cells.AddOrUpdate(cell2);
            context.Cells.AddOrUpdate(cell3);
            context.Cells.AddOrUpdate(cell4);
            context.Cells.AddOrUpdate(cell5);
            context.Cells.AddOrUpdate(cell6);
        }
    }
}
