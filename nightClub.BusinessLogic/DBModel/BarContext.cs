using System.Data.Entity;
using nightClub.Domain.Entities.Bar;

namespace nightClub.BusinessLogic.DBModel
{
    public class BarContext:DbContext
    {
        public BarContext() :
            base("name=nightClub")
        {

        }
        public virtual DbSet<BarDbTable> Bars { get; set; }
    }
}
