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
        public long Id { get; set; }  // Identificador do evento
        public string UserId { get; set; } // Identificador do usuário convidado
        public EEventResponseStatus EventResponseStatus { get; set; } = EEventResponseStatus.Maybe; // Resposta (Yes, No, Maybe)
        public DateTime EventResponseDate { get; set; } // Data da resposta
        public long EventId { get; set; }
        public Event Event { get; set; } = null;
        
    }
}
