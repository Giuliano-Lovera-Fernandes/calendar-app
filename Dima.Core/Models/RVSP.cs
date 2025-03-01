using Dima.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Models
{
    public class RVSP
    {        
        public long Id { get; set; }  
        public string UserId { get; set; }
        //Resposta (Yes, No, Maybe) - pensando em criar de forma neutra
        public EEventResponseStatus EventResponseStatus { get; set; } = EEventResponseStatus.Talvez; 
        public DateTime EventResponseDate { get; set; } 
        public long EventId { get; set; }
        public Event Event { get; set; } = null;        
    }
}
