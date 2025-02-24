using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Models.Reports
{
    public record IncomesAndExpenses(string UserId, int Month, int Year, decimal Incomes, decimal Expenses);
   
}
