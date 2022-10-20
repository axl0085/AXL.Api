using AutoMapper;
using AXL.Repositorys.Contract.Base;
using DapperExtensions;

namespace AXL.Services.Contract.Base
{
    public class BaseService<T> : IService<T> where T : class, new()
    {
        //private readonly IAsyncDatabase database;
        //private readonly IMapper mapper;
        //public BaseService(IAsyncDatabase database, IMapper mapper)
        //{
        //    this.database = database;
        //    this.mapper = mapper;
        //}
    }
}
