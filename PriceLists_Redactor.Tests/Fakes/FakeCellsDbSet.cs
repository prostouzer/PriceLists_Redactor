using System.Linq;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Tests.Fakes
{
    class FakeCellsDbSet : FakeDbSet<Cell>
    {
        public override Cell Find(params object[] keyValues)
        {
            return this.SingleOrDefault(cell => cell.Id == (int)keyValues.Single());
        }
    }
}
