using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using yzu_course_info_API.Models;

namespace yzu_course_info_API.Controllers
{
    public class MembersController : ODataController
    {
        private Entities db = new Entities();

        // GET: api/Members
        [EnableQuery]
        public IQueryable<Member> GetMembers()
        {
            return db.Member;
        }

        // GET: api/Members(5)
        [EnableQuery]
        public SingleResult<Member> GetMember([FromODataUri] string key)
        {
            return SingleResult.Create(db.Member.Where(member => member.mId == key));
        }

        // PUT: api/Members(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Member> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Member member = db.Member.Find(key);
            if (member == null)
            {
                return NotFound();
            }

            patch.Put(member);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(member);
        }

        // POST: api/Members
        public IHttpActionResult Post(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Member.Add(member);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MemberExists(member.mId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(member);
        }

        // PATCH: api/Members(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Member> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Member member = db.Member.Find(key);
            if (member == null)
            {
                return NotFound();
            }

            patch.Patch(member);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(member);
        }

        // DELETE: api/Members(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            Member member = db.Member.Find(key);
            if (member == null)
            {
                return NotFound();
            }

            db.Member.Remove(member);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/Members(5)/Comment
        [EnableQuery]
        public IQueryable<Comment> GetComment([FromODataUri] string key)
        {
            return db.Member.Where(m => m.mId == key).SelectMany(m => m.Comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(string key)
        {
            return db.Member.Count(e => e.mId == key) > 0;
        }
    }
}
