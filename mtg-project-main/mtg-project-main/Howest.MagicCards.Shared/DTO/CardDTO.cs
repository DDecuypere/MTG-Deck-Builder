using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public class CardDTO
    {
        public long Id { get; init; }
        public string? Name { get; init; }
        public string? Text { get; init; }
        public string? Type { get; init; }
        public string? OriginalImageUrl { get; init; }
        public string? Rarity { get; init; }
        public string? Artist { get; init; }
        public string? Set { get; init; }
    }
}
