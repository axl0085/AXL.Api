using AutoMapper;
using AXL.Dto;
using AXL.Entitys;
using AXL.Repositorys.Contract;
using AXL.Repositorys.Contract.Base;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Dapper;
using DapperExtensions.Predicate;
using System.Transactions;

namespace AXL.Repositorys
{
    public class UserRepository : BaseRepository<UserDto>, IUserRepository
    {
        private readonly IAsyncDatabase database;
        private readonly IMapper mapper;
        private readonly IAsyncDatabase Sqldatabase;
        private readonly IAsyncDatabase Pgdatabase;

        public UserRepository(IAsyncDatabase Database, IMapper Mapeer, [KeyFilter("SqlDb")] IAsyncDatabase sqldatabase, [KeyFilter("PgDb")] IAsyncDatabase pgdatabase) 
        : base(new Dictionary<string, IAsyncDatabase> { { "database", Database },{ "sqldatabase", sqldatabase },{ "pgdatabase", pgdatabase } }, Mapeer)
        {
            database = Database;
            mapper = Mapeer;
            Sqldatabase = sqldatabase;
            Pgdatabase = pgdatabase;
        }
        public void Get()
        {
          var res =   Sqldatabase.Connection.Query("select * from [192.168.1.199].[SmartSp].[dbo].nv_dept s left join users u on s.dept_uid = u.address");
        }

        public async Task <List<UserDto>> PgGetUsers()
        {
            var res = Pgdatabase.Connection.Query("Select * from users");
            var  reslt =  await Pgdatabase.GetList<UserEntity>();
            return mapper.Map<List<UserDto>>(reslt);
        }
    }
}
