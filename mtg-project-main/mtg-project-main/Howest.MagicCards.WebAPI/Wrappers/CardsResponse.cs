using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Wrappers
{
    public class CardsResponse<T>
    {
        public T? Data { get; set; }
        public bool Succeeded { get; set; } = true;
        public string[]? Errors { get; set; }
        public string Message { get; set; } = string.Empty;

        public CardsResponse() : this(default(T))
        {

        }

        public CardsResponse(T? data)
        {
            Data = data;
        }
    }
}
