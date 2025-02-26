using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dima.Core.Models.RVSP;

namespace Dima.Core.Models
{
    public class Event
    {
        public long Id { get; set; }  // Identificador único do evento
        public string Title { get; set; } = string.Empty; // Título do evento
        public string Description { get; set; } = string.Empty; // Descrição do evento       

        // Atributos para eventos com duração superior a um dia
        public DateTime StartDate { get; set; } = DateTime.Now; // Data de início do evento
        public DateTime EndDate { get; set; }   // Data de término do evento

        // Relacionamento com o usuário que criou o evento
        public string UserId { get; set; } = string.Empty; // ID do usuário criador

        // Status de ativação do evento (pode ser utilizado para desativar um evento sem excluí-lo)
        public bool IsActive { get; set; } = true;

        public bool IsMultiDayEvent { get; set; }

        // Lista de convidados (RSVP)
        public List<RVSP> RSVPs { get; set; } = new List<RVSP>();
    }
}
