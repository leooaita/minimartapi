using MiniMartApi.Models;

namespace MiniMartApi.Dtos
{
    public class ResponseProductStockStoreDTO
    {
        public Product  product { get; set; }
        public Store  store { get; set; }
        public int? cant { get; set; }
    }
}
