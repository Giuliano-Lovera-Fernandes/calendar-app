using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Enums
{
    public enum EEventResponseStatus
    {
        Sim = 1,   // O usuário aceitou o convite
        Não = 2,    // O usuário recusou o convite
        Talvez = 3  // O usuário talvez participe
    }
}
