using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXL.Commons
{
    public enum ErrorCode
    {
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,
    }
}
