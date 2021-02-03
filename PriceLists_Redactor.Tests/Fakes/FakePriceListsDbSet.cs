using System.Linq;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Tests.Fakes
{
    internal class FakePriceListsDbSet : FakeDbSet<PriceList>
    {
        public override PriceList Find(params object[] keyValues)
        {
            return this.SingleOrDefault(priceList => priceList.Id == (int)keyValues.Single());
        }
    }
}
