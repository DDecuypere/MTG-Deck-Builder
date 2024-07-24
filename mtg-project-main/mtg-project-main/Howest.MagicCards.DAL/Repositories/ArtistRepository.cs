using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MtgV1Context _db;
        public ArtistRepository(MtgV1Context mtgContext)
        {
            _db = mtgContext;
        }
        public IQueryable<Artist> GetAllArtists()
        {
            return _db.Artists
                        .Select(artist => artist);
        }

        public Artist? GetArtist(long id)
        {
            Artist? artist = _db.Artists
                                .FirstOrDefault(artist => artist.Id == id);
            return artist;
        }
    }
}
