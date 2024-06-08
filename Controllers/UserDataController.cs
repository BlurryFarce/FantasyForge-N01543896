using FantasyForge_N01543896.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FantasyForge_N01543896.Controllers
{
    public class UserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
  
        /// <summary>
        /// Returns all Users in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all users in the database.
        /// </returns>
        /// <example>
        /// GET: api/UserData/ListUsers
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult ListUsers() {

            List<User> Users = db.Users.ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            Users.ForEach(u => UserDtos.Add(new UserDto() {
                UserID = u.UserID,
                UserName =u.UserName,
                Bio = u.Bio,
                FavoriteGenre = u.FavoriteGenre,
                JoinDate = u.JoinDate,
                Location = u.Location,
            }));

            return Ok(UserDtos);
        }

        /// <summary>
        /// Returns a user by id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: the specified user details with thier list
        /// </returns>
        /// <example>
        /// GET: api/UserData/FindUser/2
        /// </example>
        [ResponseType(typeof(User))]
        [HttpGet]
        public IHttpActionResult FindUser(int id) {
            User User = db.Users.Find(id);
            UserDto UserDto = new UserDto() {
                UserID = User.UserID,
                UserName = User.UserName,
                Bio = User.Bio,
                FavoriteGenre = User.FavoriteGenre,
                JoinDate = User.JoinDate,
                Location = User.Location,

            };
            if (User == null) {
                return NotFound();
            }

            return Ok(UserDto);
        }

        /// <summary>
        /// Updates a particular user in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the user ID primary key</param>
        /// <param name="user">JSON FORM DATA of a user</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/UserData/UpdateUser/5
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, User user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != user.UserID) {

                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a user to the system
        /// </summary>
        /// <param name="user">JSON FORM DATA of a user</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: User ID, User Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/UserData/AddUser
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult AddUser(User user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes a user from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the user</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/UserData/DeleteUser/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult DeleteUser(int id) {
            User user = db.Users.Find(id);
            if (user == null) {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id) {
            return db.Users.Count(u => u.UserID == id) > 0;
        }
    }
}
