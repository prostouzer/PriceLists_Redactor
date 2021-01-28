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

            var cell1 = new Cell() { Data = "Просто стол!", ColumnId = 1 };
            var cell2 = new Cell() { Data = "1", ColumnId = 2 };
            var cellsList1 = new List<Cell>();
            cellsList1.Add(cell1);
            cellsList1.Add(cell2);

            var cell3 = new Cell() { Data = "Хороший, деревянный стул. ТЕСТ МНОГОСТРОЧНОЙ КОЛОНКИ. ТЕСТ МНОГОСТРОЧНОЙ КОЛОНКИ", ColumnId = 3 };
            var cell4 = new Cell() { Data = "10", ColumnId = 4 };
            var cellsList2 = new List<Cell>();
            cellsList2.Add(cell3);
            cellsList2.Add(cell4);

            context.Items.AddOrUpdate(new Item() { Title = "Обеденный стол", Cells = cellsList1, PriceListId = 1 });
            context.Items.AddOrUpdate(new Item() { Title = "Стул", Cells = cellsList2, PriceListId = 2 });
        }
    }
}
