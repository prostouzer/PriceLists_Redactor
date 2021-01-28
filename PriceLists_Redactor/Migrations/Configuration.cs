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

        }
    }
}
