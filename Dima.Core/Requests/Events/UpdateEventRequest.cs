using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Events
{
    public class UpdateEventRequest : Request
    {
        public long Id { get; set; }         

        [Required(ErrorMessage = "Título inválido")]
        [MaxLength(100, ErrorMessage = "O título deve conter até 100 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição inválida")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Data e Hora de início é obrigatória")]
        public DateTime? StartDate { get; set; }

        public TimeSpan? StartTime { get; set; } = TimeSpan.Zero;

        [Required(ErrorMessage = "Data e Hora de término é obrigatória")]
        public DateTime? EndDate { get; set; }

        public TimeSpan? EndTime { get; set; } = TimeSpan.Zero;

        public bool IsActive { get; set; } = true;
    }
}
