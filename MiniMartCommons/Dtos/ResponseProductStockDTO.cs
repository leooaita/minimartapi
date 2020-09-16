using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMartApi.Dtos
{
    public class ResponseProductStockDTO
    {
            public Product product { get; set; }
            public int Total { get; set; }
    }
}
