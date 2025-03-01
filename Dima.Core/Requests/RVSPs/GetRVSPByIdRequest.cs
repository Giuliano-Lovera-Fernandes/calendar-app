using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.RVSPs
{
    public class GetRVSPByIdRequest : Request
    {
        public long Id { get; set; }
    }
}
