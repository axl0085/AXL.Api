using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXL.Dto;
using AXL.Repositorys.Contract.Base;
namespace AXL.Repositorys.Contract
{
    public interface IUserRepository:IRepository<UserDto>
    {
        void Get();
    }
}
