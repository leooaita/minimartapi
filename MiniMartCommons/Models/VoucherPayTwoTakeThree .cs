using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMart.Models
{
    public class VoucherPayTwoTakeThree : VoucherDiscountPercentPerUnit
    {
        public VoucherPayTwoTakeThree():base(0,100,3)
        {

        }
    }
}
