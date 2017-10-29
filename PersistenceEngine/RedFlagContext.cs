using System.Data.Linq;
using RedFlag.ObjectModels;

namespace RedFlag.DataAccess
{
    public class RedFlagContext :  DataContext
    {
        public Table<FlaggedClient> FlaggedClients;
        public Table<FlaggedNonClient> FlaggedNonClients;

        public RedFlagContext(string connString) : base(connString) { }
    }
}
