namespace Howest.MagicCards.MinimalAPI.Wrappers
{
    public class Deckresponse<T>
    {
        public T? Deck { get; set; }
        public bool Succeeded { get; set; } = true;
        public string[]? Errors { get; set; }
        public string Message { get; set; } = string.Empty;

        public Deckresponse() : this(default(T))
        {

        }

        public Deckresponse(T? deck)
        {
            Deck = deck;
        }
    }
}
