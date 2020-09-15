using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMartApi.Models
{
    public class CartVoucher
    {
        public int Id  {get;set;}
        public int VoucherId { get; set; }
        public int CartId { get; set; }
    }
}
