using System;
using System.Collections.Generic;
using System.Linq;
using CodingRep.App_Code;

namespace CodingRep.service.query
{
    public class UserRepositoryService: IDisposable
    {
        private readonly ModelDb _context;

        private UserRepositoryService(ModelDb context)
        {
            _context = context;
        }

        // 工厂方法，调用时不用写 new ModelDb()
        public static UserRepositoryService Create()
        {
            return new UserRepositoryService(new ModelDb());
        }

        public KeyValuePair<int, int> GetRepCntByUID(int userID)
        {
            int repCnt = _context.repositories.Count(r => r.userId == userID);
            return new KeyValuePair<int, int>(userID, repCnt);
        }

        public List<repositories> GetAllReposByUID(int userID)
        {
            return _context.repositories.Where(r => r.userId == userID).ToList();
        }

        public bool HasPrivateRepo(int userID)
        {
            return _context.repositories.Any(r => r.userId == userID && r.isPrivate);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}