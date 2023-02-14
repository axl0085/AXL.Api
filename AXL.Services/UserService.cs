using AXL.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXL.Dto;
using AXL.Services.Contract.Base;
using AXL.Repositorys.Contract;
using AXL.Entitys;
using DapperExtensions;
using AutoMapper;
using DapperExtensions.Predicate;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Caching.Memory;

namespace AXL.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : BaseService<UserDto>, IUserService
    {
        private readonly IAsyncDatabase database;
        private readonly IMapper mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAsyncDatabase Sqldatabase;
        private readonly IAsyncDatabase Pgdatabase;
        private readonly IMemoryCache _memoryCache;

        public UserService(IUserRepository userRepository, IAsyncDatabase Database, IMapper Mapeer, [KeyFilter("SqlDb")] IAsyncDatabase sqldatabase, [KeyFilter("PgDb")] IAsyncDatabase pgdatabase, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            database = Database;
            mapper = Mapeer;
            Sqldatabase = sqldatabase;
            Pgdatabase = pgdatabase;
            _memoryCache = memoryCache;
        }

        public async Task<ResponseDto> GetUsers()
        {
            try
            {
                IEnumerable<UserEntity>? datacache;
                datacache = _memoryCache.Get<IEnumerable<UserEntity>>("UserInfo");
                if (datacache is null)
                {
                    List<ISort> sorts = new()
                    {
                    new Sort { PropertyName = "ID", Ascending = true }
                     };
                    //IList<IPredicate> preList = new List<IPredicate>
                    //    {
                    //        Predicates.Field<UserEntity>(f => f.ID, Operator.Lt, 2)
                    //    };
                    var res = await _userRepository.GetPage<UserEntity>(null, sorts, 1, 15, null);
                    var count = await _userRepository.Count<UserEntity>(null);
                    ResponseDto responseDto = new(200, mapper.Map<List<UserDto>>(res), count);
                    _memoryCache.Set("UserInfo", res, TimeSpan.FromMinutes(1));
                    _memoryCache.Set("UserInfoCount", count, TimeSpan.FromMinutes(1));
                    return responseDto;
                }
                else
                {
                    var count = _memoryCache.Get<int>("UserInfoCount");
                    ResponseDto responseDto = new(200, mapper.Map<List<UserDto>>(datacache), count);
                    return responseDto;
                }
            }
            catch (Exception)
            {
                ResponseDto responseDto = new(400, null, 0, "查询出错");
                return responseDto;
            }
        }

        public async Task<ResponseDto> PgGetUsers()
        {
            try
            {
                var res = await _userRepository.PgGetUsers();
                ResponseDto responseDto = new(200, mapper.Map<List<UserDto>>(res), 1);
                return responseDto;
            }
            catch (Exception)
            {
                ResponseDto responseDto = new(400, null, 0, "查询出错");
                return responseDto;
            }
        }

        public async Task<int> Login(string userName, string passWord)
        {
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<UserEntity>(f => f.UserNo, Operator.Eq, userName));
            pg.Predicates.Add(Predicates.Field<UserEntity>(f => f.UserTel, Operator.Eq, passWord));
            return await _userRepository.Count<UserEntity>(pg);
        }

        /// <summary>
        /// 自定义方法
        /// </summary>
        public void Get()
        {
            _userRepository.Get();
        }
    }
}