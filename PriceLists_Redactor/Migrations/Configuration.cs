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
            var column1 = new Column() {DataType = DataType.SingleLine, HeaderText = "Моя однострочная колонка"};
            var column2 = new Column() { DataType = DataType.Bool, HeaderText = "В наличии" };

            var columnsList1 = new List<Column>();
            columnsList1.Add(column1);
            columnsList1.Add(column2);

            context.PriceLists.AddOrUpdate(new PriceList()
            {
                Columns = columnsList1,
                Name = "Прайс-лист мебели"
            });

            var column3 = new Column() { DataType = DataType.MultiLine, HeaderText = "Моя МНОГОСТРОЧНАЯ колонка" };
            var column4 = new Column() { DataType = DataType.Bool, HeaderText = "Кол-во на складе" };
            
            var columnsList2 = new List<Column>();
            columnsList1.Add(column3);
            columnsList1.Add(column4);

            context.PriceLists.AddOrUpdate(new PriceList()
            {
                Columns = columnsList2,
                Name = "Прайс-лист одежды"
            });


            var cell1 = new Cell() { Data = "Просто стол!" };
            var cell2 = new Cell() { Data = "1" };

            var cellsList1 = new List<Cell>();
            cellsList1.Add(cell1);
            cellsList1.Add(cell2);

            var item1 = new Item()
            {
                Title = "Обеденный стол",
                PriceListId = 1,
                Cells = cellsList1
            };

            var cell3 = new Cell() { Data = "Хороший, деревянный стул. ТЕСТ МНОГОСТРОЧНОЙ КОЛОНКИ. ТЕСТ МНОГОСТРОЧНОЙ КОЛОНКИ" };
            var cell4 = new Cell() { Data = "10" };

            var cellsList2 = new List<Cell>();
            cellsList2.Add(cell3);
            cellsList2.Add(cell4);

            var item2 = new Item()
            {
                Title = "Стул",
                PriceListId = 1,
                Cells = cellsList2
            };
        }
    }
}
