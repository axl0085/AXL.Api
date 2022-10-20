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

namespace AXL.Repositorys
{
    public class UserRepository : BaseRepository<UserDto>, IUserRepository
    {
        private readonly IAsyncDatabase database;
        private readonly IMapper mapper;
        public UserRepository(IAsyncDatabase Database, IMapper Mapeer) : base(Database, Mapeer)
        {
            database = Database;
            mapper = Mapeer;
        }
        public void Get()
        {
            string s = "";
        }
        
    }
}
