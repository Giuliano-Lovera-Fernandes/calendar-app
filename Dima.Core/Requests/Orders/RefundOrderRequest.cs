﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Orders
{
    public class RefundOrderRequest : Request
    {
        public long Id { get; set; }
    }
}
