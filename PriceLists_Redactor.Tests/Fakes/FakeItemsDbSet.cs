using System.Linq;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Tests.Fakes
{
    internal class FakeItemsDbSet : FakeDbSet<Item>
    {
        public override Item Find(params object[] keyValues)
        {
            return this.SingleOrDefault(item => item.Id == (int)keyValues.Single());
        }
    }
}
