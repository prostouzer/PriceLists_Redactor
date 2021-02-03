using System.Linq;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Tests.Fakes
{
    internal class FakeColumnsDbSet : FakeDbSet<Column>
    {
        public override Column Find(params object[] keyValues)
        {
            return this.SingleOrDefault(column => column.Id == (int)keyValues.Single());
        }
    }
}
