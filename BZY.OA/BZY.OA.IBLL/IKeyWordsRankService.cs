using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.IBLL
{
    public partial interface IKeyWordsRankService : IBaseService<KeyWordsRank>
    {
        bool DeleteAllKeyWordsRank();
        bool InsertKeyWordsRank();
        List<string> GetSearchMsg(string term);
    }
}
