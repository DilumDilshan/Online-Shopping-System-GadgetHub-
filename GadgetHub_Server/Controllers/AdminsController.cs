using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GadgetHub_Server.Models;

namespace GadgetHub_Server.Controllers
{
    public class AdminsController : ApiController
    {
        private GadgetHub_dbEntities3 db = new GadgetHub_dbEntities3();

        // GET: api/Admins
        public IQueryable<Admins> GetAdmins()
        {
            return db.Admins;
        }

        // GET: api/Admins/5
        [ResponseType(typeof(Admins))]
        public IHttpActionResult GetAdmins(int id)
        {
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return NotFound();
            }

            return Ok(admins);
        }

        // PUT: api/Admins/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAdmins(int id, Admins admins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != admins.AdminID)
            {
                return BadRequest();
            }

            db.Entry(admins).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Admins
        [ResponseType(typeof(Admins))]
        public IHttpActionResult PostAdmins(Admins admins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Admins.Add(admins);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AdminsExists(admins.AdminID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = admins.AdminID }, admins);
        }

        // DELETE: api/Admins/5
        [ResponseType(typeof(Admins))]
        public IHttpActionResult DeleteAdmins(int id)
        {
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return NotFound();
            }

            db.Admins.Remove(admins);
            db.SaveChanges();

            return Ok(admins);
        }

        [HttpPost]
        [Route("api/admins/Login")]
        public IHttpActionResult Login([FromBody] Admins loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.AdminEmail) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var Admins = db.Admins.FirstOrDefault(p => p.AdminEmail == loginRequest.AdminEmail);

            if (Admins == null || Admins.Password != loginRequest.Password) // In real cases, hash & verify passwords
            {
                return Unauthorized();
            }

            return Ok(new { Message = "Login successful", AdminID = Admins.AdminID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdminsExists(int id)
        {
            return db.Admins.Count(e => e.AdminID == id) > 0;
        }
    }
}