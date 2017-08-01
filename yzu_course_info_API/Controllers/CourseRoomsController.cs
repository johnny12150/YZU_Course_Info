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
    public class CourseRoomsController : ODataController
    {
        private Entities db = new Entities();

        // GET: api/CourseRooms
        [EnableQuery]
        public IQueryable<CourseRoom> GetCourseRooms()
        {
            return db.CourseRoom;
        }

        // GET: api/CourseRooms(5)
        [EnableQuery]
        public SingleResult<CourseRoom> GetCourseRoom([FromODataUri] string key)
        {
            return SingleResult.Create(db.CourseRoom.Where(courseRoom => courseRoom.cId == key));
        }

        // PUT: api/CourseRooms(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<CourseRoom> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CourseRoom courseRoom = db.CourseRoom.Find(key);
            if (courseRoom == null)
            {
                return NotFound();
            }

            patch.Put(courseRoom);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseRoomExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(courseRoom);
        }

        // POST: api/CourseRooms
        public IHttpActionResult Post(CourseRoom courseRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseRoom.Add(courseRoom);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CourseRoomExists(courseRoom.cId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(courseRoom);
        }

        // PATCH: api/CourseRooms(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<CourseRoom> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CourseRoom courseRoom = db.CourseRoom.Find(key);
            if (courseRoom == null)
            {
                return NotFound();
            }

            patch.Patch(courseRoom);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseRoomExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(courseRoom);
        }

        // DELETE: api/CourseRooms(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            CourseRoom courseRoom = db.CourseRoom.Find(key);
            if (courseRoom == null)
            {
                return NotFound();
            }

            db.CourseRoom.Remove(courseRoom);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/CourseRooms(5)/Course
        [EnableQuery]
        public SingleResult<Course> GetCourse([FromODataUri] string key)
        {
            return SingleResult.Create(db.CourseRoom.Where(m => m.cId == key).Select(m => m.Course));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseRoomExists(string key)
        {
            return db.CourseRoom.Count(e => e.cId == key) > 0;
        }
    }
}
