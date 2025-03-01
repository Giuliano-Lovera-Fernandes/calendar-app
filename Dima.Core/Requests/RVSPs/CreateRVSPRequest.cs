using Dima.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.RVSPs
{
    public class CreateRVSPRequest : Request
    {       

        [Required(ErrorMessage = "Resposta do evento inválida")]
        public EEventResponseStatus EventResponseStatus { get; set; } = EEventResponseStatus.Talvez;

        [Required(ErrorMessage = "Data inválida")]
        public DateTime? EventResponseDate { get; set; }        

        [Required(ErrorMessage = "Evento inválido")]
        public long EventId { get; set; }

        
    }
}
