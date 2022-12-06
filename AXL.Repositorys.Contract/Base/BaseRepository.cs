using AutoMapper;
using Dapper;
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

        public async Task<int> Count<F>(object predicate, int? commandTimeout = null) where F : class
        {
            return await database.Count<F>(predicate);
        }

        public async Task<IEnumerable<F>> GetList<F>(object? predicate = null, IList<ISort>? sort = null, int? commandTimeout = null, bool buffered = true) where F : class
        {
          return  await database.GetList<F>(predicate, sort, commandTimeout, buffered);
        }
        public async  Task<IEnumerable<F>> GetPage<F>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction ,int? commandTimeout = null, bool buffered = true) where F : class
        {
            return await database.GetPage<F>(predicate, sort, page, resultsPerPage, transaction, resultsPerPage);
        }
    }

}
