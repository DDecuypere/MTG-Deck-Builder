using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.ViewModels
{
    public class FilterViewModel
    {
        public string? CardName { get; set; }
        public string? CardText { get; set; }
        public string? TypeName { get; set; }
        public string? ArtistName { get; set; }
        public string? SetName { get; set; }
        public string? RarityName { get; set; }
        public int? PageNumber { get; set; } = 1;
        public string? SortType { get; set; } = "asc";
        public bool IsDescending { get; set; } = false;
    }
}
