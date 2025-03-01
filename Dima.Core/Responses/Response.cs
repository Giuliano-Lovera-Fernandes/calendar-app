using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dima.Core.Responses
{
    public class Response<TData>
    {        
        private readonly  int _code;

        [JsonConstructor]
        public Response() => _code = Configuration.DEFAULTSTATUSCODE;
        //public Response()
        //{
        //    _code = DEFAULTSTATUSCODE; 
        //}

        public Response(TData? data, int code = Configuration.DEFAULTSTATUSCODE, string? message = null)
        {
            Data = data;
            Message = message;
            _code = code;           
        }
        public TData? Data { get; set; }
        public string? Message { get; set; }        

        public int Code => _code;      

        public bool IsSuccess => _code is >= 200 and <= 299; 
    }
}
