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
    public class UserMediaItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all UserMediaItems in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all UserMediaItems in the database.
        /// </returns>
        /// <example>
        /// GET: api/UserMediaItemsData/ListUserMediaItems
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserMediaItemDto))]
        public IHttpActionResult ListUserMediaItems() {
            List<UserMediaItem> UserMediaItems = db.UserMediaItems.ToList();
            List<UserMediaItemDto> UserMediaItemDtos = new List<UserMediaItemDto>();

            UserMediaItems.ForEach(ui => UserMediaItemDtos.Add(new UserMediaItemDto() {
                UserMediaItemID = ui.UserMediaItemID,
                UserID = ui.User.UserID,
                MediaItemID = ui.MediaItem.MediaItemID,
                Rating = ui.Rating,
                Review = ui.Review,
                Status = ui.Status,
            }));

            return Ok(UserMediaItemDtos);
        }

        /// <summary>
        /// Returns all UserMediaItems in the system associated with a particular user.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all UserMediaItems in the database related to a particular user
        /// </returns>
        /// <param name="id">user Primary Key</param>
        /// <example>
        /// GET: api/UserMediaItemData/ListUserMediaItemsforuser/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserMediaItemDto))]
        public IHttpActionResult ListUserMediaItemsForUser(int id) {

            //SQL Equivalent:
            //select keepers.*, keeperanimals.* from animals inner join keeperanimals on keeperanimals.keeperid = keepers.keeperid where
            //keeperanimals.animalid = {id}

            List<UserMediaItem> UserMediaItems = db.UserMediaItems.Where(
                ui => ui.Users.Any(
                    u => u.UserID == id)
                ).ToList();
            List<UserMediaItemDto> UserMediaItemDtos = new List<UserMediaItemDto>();

            UserMediaItems.ForEach(ui => UserMediaItemDtos.Add(new UserMediaItemDto() {
                UserMediaItemID = ui.UserMediaItemID,
                UserID = ui.User.UserID,
                MediaItemID = ui.MediaItem.MediaItemID,
                Rating = ui.Rating,
                Review = ui.Review,
                Status = ui.Status,
            }));

            return Ok(UserMediaItemDtos);
        }



        /// <summary>
        /// Updates a particular UserMediaItem in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the UserMediaItem ID primary key</param>
        /// <param name="UserMediaItem">JSON FORM DATA of an UserMediaItem</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/UserMediaItemrData/UpdateUserMediaItem/5
        /// FORM DATA: UserMediaItem JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateKeeper(int id, UserMediaItem UserMediaItem) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != UserMediaItem.UserMediaItemID) {

                return BadRequest();
            }

            db.Entry(UserMediaItem).State = EntityState.Modified;

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserMediaItemExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an UserMediaItem to the system
        /// </summary>
        /// <param name="UserMediaItem">JSON FORM DATA of an UserMediaItem</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: UserMediaItem ID, UserMediaItem Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/UserMediaItemData/AddUserMediaItem
        /// FORM DATA: UserMediaItem JSON Object
        /// </example>
        [ResponseType(typeof(UserMediaItem))]
        [HttpPost]
        public IHttpActionResult AddUserMediaItem(UserMediaItem UserMediaItem) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.UserMediaItems.Add(UserMediaItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = UserMediaItem.UserMediaItemID }, UserMediaItem);
        }

        /// <summary>
        /// Deletes a UserMediaItem from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the UserMediaItem</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/UserMediaItemData/DeleteUserMediaItem/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(UserMediaItem))]
        [HttpPost]
        public IHttpActionResult DeleteUserMediaItem(int id) {
            UserMediaItem UserMediaItem = db.UserMediaItems.Find(id);
            if (UserMediaItem == null) {
                return NotFound();
            }

            db.UserMediaItems.Remove(UserMediaItem);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserMediaItemExists(int id) {
            return db.UserMediaItems.Count(e => e.UserMediaItemID == id) > 0;
        }
    }
}
