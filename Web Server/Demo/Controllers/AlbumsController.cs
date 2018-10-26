namespace Demo.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Demo.Attributes;
    using Demo.Database;
    using Demo.Models;

    using Http.Models.Responses;

    using Microsoft.EntityFrameworkCore;

    public class AlbumsController : ControllerBase
    {
        private readonly DemoDbContext _dbContext = new DemoDbContext();

        [HttpGet("/albums/all")]
        public IHttpResponse GetAll()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            Album[] albums = _dbContext.Albums.ToArray();

            if (albums.Length > 0)
            {
                ViewPropertyBag["albums"] = string.Join("\n", albums.Select(album => $"<b><a href=\"/albums/details?id={album.Id}\">{album.Name}</a></b>"));
            }
            else
            {
                ViewPropertyBag["albums"] = "No albums to display.";
            }

            return View("Views/Albums/All");
        }

        [HttpGet("/albums/create")]
        public IHttpResponse GetCreate()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            return View("Views/Albums/Create");
        }

        [HttpPost("/albums/create")]
        public IHttpResponse PostCreate()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            Album album = new Album
            {
                Cover = Request.FormData["cover"].ToString(),
                Name = Request.FormData["name"].ToString()
            };

            _dbContext.Albums.Add(album);
            _dbContext.SaveChanges();

            return Redirect($"/albums/details?albumId={album.Id}");
        }

        [HttpGet("/albums/details")]
        public IHttpResponse GetDetails()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            string albumId = Request.QueryData["id"].ToString();
            Album matchingAlbum = _dbContext.Albums.Include(album => album.Tracks).SingleOrDefault(album => album.Id == albumId);

            if (matchingAlbum == null)
            {
                return Redirect("/");
            }

            ViewPropertyBag["albumId"] = albumId;
            ViewPropertyBag["cover"] = matchingAlbum.Cover;
            ViewPropertyBag["name"] = matchingAlbum.Name;
            ViewPropertyBag["price"] = matchingAlbum.Price;

            Track[] tracks = matchingAlbum.Tracks.ToArray();

            if (tracks.Length > 0)
            {
                ViewPropertyBag["tracks"] = string.Join("\n", matchingAlbum.Tracks.Select(track => $@"<li><div><a href=/tracks/details?albumId={albumId}&trackId={track.Id}>{track.Name}</a></div></li><br/>"));
            }
            else
            {
                ViewPropertyBag["tracks"] = "There aren't any tracks";
            }

            return View("Views/Albums/Details");
        }
    }
}