using FantasyForge_N01543896.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FantasyForge_N01543896.Controllers
{
    public class MediaItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all mediaitems in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all mediaitems in the database.
        /// </returns>
        /// <example>
        /// GET: api/mediaitemdata/Listmediaitem
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MediaItemDto))]
        public IHttpActionResult ListMediaItems() {
            List<MediaItem> MediaItems = db.MediaItems.ToList();
            List<MediaItemDto> MediaItemDtos = new List<MediaItemDto>();

            MediaItems.ForEach(m => MediaItemDtos.Add(new MediaItemDto() {
                MediaItemID = m.MediaItemID,
                Title = m.Title,
                Type = m.Type,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                Genre = m.Genre,
            }));

            return Ok(MediaItemDtos);
        }

        /// <summary>
        /// Returns all MediaItem in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An MediaItem in the system matching up to the MediaItem ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the MediaItem</param>
        /// <example>
        /// GET: api/MediaItemData/FindMediaItem/5
        /// </example>
        [ResponseType(typeof(MediaItemDto))]
        [HttpGet]
        public IHttpActionResult FindMediaItemDtos(int id) {
            MediaItem MediaItem = db.MediaItems.Find(id);
            MediaItemDto MediaItemDto = new MediaItemDto() {
                MediaItemID = MediaItem.MediaItemID,
                Title = MediaItem.Title,
                Type = MediaItem.Type, 
                Description = MediaItem.Description,
                ReleaseDate= MediaItem.ReleaseDate,
                Genre = MediaItem.Genre,
            };
            if (MediaItem == null) {
                return NotFound();
            }

            return Ok(MediaItemDto);
        }

        /// <summary>
        /// Updates a particular mediaitem in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Species ID primary key</param>
        /// <param name="mediaitem">JSON FORM DATA of an mediaitem</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/mediaitemdata/updatemediaitem/5
        /// FORM DATA: Species JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSpecies(int id, MediaItem MediaItem) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != MediaItem.MediaItemID) {

                return BadRequest();
            }

            db.Entry(MediaItem).State = EntityState.Modified;

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                if (!MediaItemExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an MediaItem to the system
        /// </summary>
        /// <param name="MediaItem">JSON FORM DATA of an MediaItem</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: MediaItem ID, MediaItem Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/MediaItemData/AddMediaItem
        /// FORM DATA: MediaItem JSON Object
        /// </example>
        [ResponseType(typeof(MediaItem))]
        [HttpPost]
        public IHttpActionResult AddMediaItem(MediaItem MediaItem) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.MediaItems.Add(MediaItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = MediaItem.MediaItemID }, MediaItem);
        }

        /// <summary>
        /// Deletes a MediaItem from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the MediaItem</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/MediaItemData/DeleteMediaItem/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(MediaItem))]
        [HttpPost]
        public IHttpActionResult DeleteMediaItem(int id) {
            MediaItem MediaItem = db.MediaItems.Find(id);
            if (MediaItem == null) {
                return NotFound();
            }

            db.MediaItems.Remove(MediaItem);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MediaItemExists(int id) {
            return db.MediaItems.Count(m => m.MediaItemID == id) > 0;
        }
    }
}
