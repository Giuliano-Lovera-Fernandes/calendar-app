using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Responses
{
    public class InvalidEventException(string userMessage, int statusCode = 404) : Exception(userMessage)
    {
        public int StatusCode { get; } = statusCode;
        public string UserMessage { get; } = userMessage;
    }
}
