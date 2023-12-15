

using Entities;

namespace Biky_Backend.Services.DTO
{
    public class SaleFilter
    {
        public int? min { get; set; }

        public int? max { get; set; }

        public PostType? type { get; set; }

        public int? categoryid { get; set; }

        public String? contains { get; set; }
    }
}
