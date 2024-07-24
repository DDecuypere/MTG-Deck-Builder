using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public class CardInDeckReadDTO
    {
        public long? CardId { get; set; }
        public int? Quantity { get; set; }
        public string? CardName { get; set; }
    }
}
