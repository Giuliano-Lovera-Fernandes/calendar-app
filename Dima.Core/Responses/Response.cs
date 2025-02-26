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
        //private readonly int _code;
        public int _code;

        //[JsonConstructor]
        //public Response() => _code = Configuration.DEFAULTSTATUSCODE;


        [JsonConstructor]
        public Response() => Code = Configuration.DEFAULTSTATUSCODE;
        //public Response()
        //{
        //    _code = DEFAULTSTATUSCODE; 
        //}

        public Response(TData? data, int code = Configuration.DEFAULTSTATUSCODE, string? message = null)
        {
            Data = data;
            Message = message;
            _code = code;
            //Errors = errors ?? new List<string>();
        }
        public TData? Data { get; set; }
        public string? Message { get; set; }
        //public List<string> Errors { get; set; } = new List<string>(); // Propriedade para armazenar erros

        //public int Code => _code;
        public int Code { get; set; }

        public bool IsSuccess => _code is >= 200 and <= 299; 


    }
}
