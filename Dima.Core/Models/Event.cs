using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dima.Core.Models.RVSP;

namespace Dima.Core.Models
{
    public class Event
    {
        public long Id { get; set; } 
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;        

        
        public DateTime StartDate { get; set; } = DateTime.Now; 
        public DateTime EndDate { get; set; }
        
        //Evitar leitura do Entity, causa de erro
        [NotMapped]
        public TimeSpan StartTime { get; set; } = TimeSpan.Zero;

        [NotMapped]
        public TimeSpan EndTime { get; set; } = TimeSpan.Zero;

        //Relacionamento com o usuário que criou o evento
        public string UserId { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;

        public bool IsMultiDayEvent { get; set; }
        
        public List<RVSP> RSVPs { get; set; } = new List<RVSP>();
    }
}
