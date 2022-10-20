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
namespace AXL.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService: BaseService<UserDto>,IUserService
    {
        private readonly IAsyncDatabase database;
        private readonly IMapper mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAsyncDatabase Oracledatabase;
        public UserService(IUserRepository userRepository, IAsyncDatabase Database, IMapper Mapeer, [KeyFilter("SqlDb")] IAsyncDatabase oracledatabase)
        {
            _userRepository = userRepository;
            database = Database;
            mapper = Mapeer;
            Oracledatabase = oracledatabase;
        }
        public async Task<List<UserDto>> GetUsers() 
        {
           var res = await _userRepository.GetList<UserEntity>();
            return  mapper.Map<List<UserDto>>(res);
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
        public void Get() {
            _userRepository.Get();
        }
    }
}
