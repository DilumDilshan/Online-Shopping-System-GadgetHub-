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
    public class DistributorsController : ApiController
    {
        private GadgetHub_dbEntities3 db = new GadgetHub_dbEntities3();

        // GET: api/Distributors
        public IQueryable<Distributors> GetDistributors()
        {
            return db.Distributors;
        }

        // GET: api/Distributors/5
        [ResponseType(typeof(Distributors))]
        public IHttpActionResult GetDistributors(int id)
        {
            Distributors distributors = db.Distributors.Find(id);
            if (distributors == null)
            {
                return NotFound();
            }

            return Ok(distributors);
        }

        // PUT: api/Distributors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDistributors(int id, Distributors distributors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != distributors.DistributorID)
            {
                return BadRequest();
            }

            db.Entry(distributors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistributorsExists(id))
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

        // POST: api/Distributors
        [ResponseType(typeof(Distributors))]
        public IHttpActionResult PostDistributors(Distributors distributors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Distributors.Add(distributors);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DistributorsExists(distributors.DistributorID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = distributors.DistributorID }, distributors);
        }

        // DELETE: api/Distributors/5
        [ResponseType(typeof(Distributors))]
        public IHttpActionResult DeleteDistributors(int id)
        {
            Distributors distributors = db.Distributors.Find(id);
            if (distributors == null)
            {
                return NotFound();
            }

            db.Distributors.Remove(distributors);
            db.SaveChanges();

            return Ok(distributors);
        }

        [HttpPost]
        [Route("api/distributors/Login")]
        public IHttpActionResult Login([FromBody] Distributors loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.DistributorEmail) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var Distributors = db.Distributors.FirstOrDefault(p => p.DistributorEmail == loginRequest.DistributorEmail);

            if (Distributors == null || Distributors.Password != loginRequest.Password) // In real cases, hash & verify passwords
            {
                return Unauthorized();
            }

            return Ok(new { Message = "Login successful", DistributorID = Distributors.DistributorID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DistributorsExists(int id)
        {
            return db.Distributors.Count(e => e.DistributorID == id) > 0;
        }
    }
}