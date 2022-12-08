using AXL.Dto;
using AXL.Services.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXL.Services.Contract
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService : IService<UserDto>
    {
        Task<ResponseDto> GetUsers();
        Task<ResponseDto> PgGetUsers();
        Task<int> Login(string userName, string passWord);
    }
}
