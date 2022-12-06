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

namespace AXL.Repositorys
{
    public class UserRepository : BaseRepository<UserDto>, IUserRepository
    {
        private readonly IAsyncDatabase database;
        private readonly IMapper mapper;
        private readonly IAsyncDatabase Sqldatabase;
        public UserRepository(IAsyncDatabase Database, IMapper Mapeer, [KeyFilter("SqlDb")] IAsyncDatabase sqldatabase) : base(Database, Mapeer)
        {
            database = Database;
            mapper = Mapeer;
            Sqldatabase = sqldatabase;
        }
        public void Get()
        {
          var res =   Sqldatabase.Connection.Query("select * from [192.168.1.199].[SmartSp].[dbo].nv_dept s left join users u on s.dept_uid = u.address");
        }
        
    }
}
