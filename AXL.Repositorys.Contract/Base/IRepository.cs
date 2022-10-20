using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Predicate;
using System.Data;
using System.Linq.Expressions;

namespace AXL.Repositorys.Contract.Base
{
    public interface IRepository<T> where T:class,new()
    {
        //Task<T> Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        //Task<T> Get<T>(dynamic id, int? commandTimeout = null) where T : class;

        //void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        //void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : class;

        //Task<dynamic> Insert<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        //Task<dynamic> Insert<T>(T entity, int? commandTimeout = null) where T : class;

        //Task<bool> Update<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        //Task<bool> Update<T>(T entity, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        //Task<bool> Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        //Task<bool> Delete<T>(T entity, int? commandTimeout = null) where T : class;

        //Task<bool> Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        //Task<bool> Delete<T>(object predicate, int? commandTimeout = null) where T : class;

        //Task<IEnumerable<T>> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;

        Task<IEnumerable<T>> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;

        //Task<IEnumerable<T>> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;

        //Task<IEnumerable<T>> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout = null, bool buffered = true) where T : class;

        //Task<IEnumerable<T>> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;

        //Task<IEnumerable<T>> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : class;

        //Task<int> Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        Task<int> Count<T>(object predicate, int? commandTimeout = null) where T : class;

        //Task<IMultipleResultReader> GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);

        //Task<IMultipleResultReader> GetMultiple(GetMultiplePredicate predicate, int? commandTimeout = null);

        //Task<Guid> GetNextGuid();

        //Task<IClassMapper> GetMap<T>() where T : class;

        //void ClearCache();
    }
}