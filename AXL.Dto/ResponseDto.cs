using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXL.Dto
{
    public class ResponseDto
    {
        public int  Code { get; set; }
        public object Data { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
        public ResponseDto(int code, object data, int count,string message = "")
        {
            Code = code;
            Data = data;
            Count = count;
            Message = message;
        }
    }
}
