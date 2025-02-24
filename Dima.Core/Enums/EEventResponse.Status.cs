using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Enums
{
    public enum EEventResponseStatus
    {
        Yes = 1,   // O usuário aceitou o convite
        No = 2,    // O usuário recusou o convite
        Maybe = 3  // O usuário talvez participe
    }
}
