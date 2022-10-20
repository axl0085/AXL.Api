using AutoMapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AXL.Repositorys.Contract.Base
{
    public class BaseRepository<T>:IRepository<T> where T:class,new()
    {
        private readonly IAsyncDatabase database;
        private readonly IMapper mapper;
        public BaseRepository(IAsyncDatabase Database, IMapper Mapeer)
        {
            database = Database;
            mapper = Mapeer;
        }

        public async Task<int> Count<T>(object predicate, int? commandTimeout = null) where T : class
        {
            return await database.Count<T>(predicate);
        }

        public async Task<IEnumerable<T>> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
        {
          return  await database.GetList<T>(predicate, sort, commandTimeout, buffered);
        }
    }

}
