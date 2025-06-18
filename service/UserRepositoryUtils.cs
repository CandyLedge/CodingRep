using System.Collections.Generic;
using System.Linq;
using CodingRep.App_Code;

namespace CodingRep.service
{
    public class UserRepositoryUtils
    {
        public static KeyValuePair<int, int>  getRepCntByUID(int userID)
        {
            using (var context = new ModelDb())
            {
                int repCnt = context.repositories
                    .Count(r => r.id == userID);

                return new KeyValuePair<int, int>(userID, repCnt);
            }
        }
    }
}