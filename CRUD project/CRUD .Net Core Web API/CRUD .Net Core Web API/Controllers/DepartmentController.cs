using CRUD_.Net_Core_Web_API.Models.DB;
using CRUD_.Net_Core_Web_API.Models;
using CRUD_.Net_Core_Web_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace CRUD_.Net_Core_Web_API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ProjectDbContext db;
        public DepartmentController(ProjectDbContext db)
        {
            this.db = db;
        }

        [HttpPost("create")]
        public ActionResult CreateDepartment(Department department)
        {
            try
            {
                var data = new DepartmentRepo(db).Create(department);
                if (data)
                {
                    return Ok("Department has been created");
                }
                else
                {
                    return BadRequest("Failed to create");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var data = new DepartmentRepo(db).Delete(id);
                if (data)
                {
                    return Ok(new { Success = "true", Message = "Department Deleted", Payload = data });
                }
                else
                {
                    return NotFound("Id not found delete failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var data = new DepartmentRepo(db).Get(id);
                if (data != null)
                {
                    return Ok(new { Success = "true", StatusCode = 200, Payload = data });
                }
                else
                {
                    return NotFound("Id not found ");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public ActionResult GetAll()
        {
            try
            {
                var data = new DepartmentRepo(db).GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public ActionResult Update(Department department)
        {
            try
            {
                var data = new DepartmentRepo(db).Update(department);
                if (data)
                {
                    return Ok("Department Update");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}
