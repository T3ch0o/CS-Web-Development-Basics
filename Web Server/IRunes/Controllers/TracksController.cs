namespace Demo.Controllers
{
    using System.Linq;

    using Demo.Attributes;
    using Demo.Database;
    using Demo.Models;

    using Http.Models.Responses;

    public class TracksController : ControllerBase
    {
        private readonly DemoDbContext _dbContext = new DemoDbContext();

        [HttpGet("/tracks/create")]
        public IHttpResponse GetCreate()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            ViewPropertyBag["albumId"] = Request.QueryData["albumId"].ToString();

            return View("Views/Tracks/Create");
        }

        [HttpPost("/tracks/create")]
        public IHttpResponse PostCreate()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            string albumId = Request.QueryData["albumId"].ToString();
            Album matchingAlbum = _dbContext.Albums.SingleOrDefault(album => album.Id == albumId);

            if (matchingAlbum == null)
            {
                return Redirect("/");
            }

            Track track = new Track
            {
                Name = Request.FormData["name"].ToString(),
                Link = Request.FormData["link"].ToString(),
                AlbumId = albumId,
                Price = decimal.Parse(Request.FormData["price"].ToString())
            };

            _dbContext.Tracks.Add(track);
            _dbContext.SaveChanges();

            return Redirect($"details?trackId={track.Id}");
        }

        [HttpGet("/tracks/details")]
        public IHttpResponse GetDetails()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            string trackId = Request.QueryData["trackId"].ToString();
            Track matchingTrack = _dbContext.Tracks.SingleOrDefault(track => track.Id == trackId);

            if (matchingTrack == null)
            {
                return Redirect("/");
            }

            ViewPropertyBag["albumId"] = matchingTrack.AlbumId;
            ViewPropertyBag["link"] = matchingTrack.Link;
            ViewPropertyBag["name"] = matchingTrack.Name;
            ViewPropertyBag["price"] = matchingTrack.Price;

            return View("Views/Tracks/Details");
        }
    }
}