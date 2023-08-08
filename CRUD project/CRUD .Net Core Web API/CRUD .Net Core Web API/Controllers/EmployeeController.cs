using CRUD_.Net_Core_Web_API.Models;
using CRUD_.Net_Core_Web_API.Models.DB;
using CRUD_.Net_Core_Web_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_.Net_Core_Web_API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ProjectDbContext db;
        public EmployeeController(ProjectDbContext db) 
        { 
            this.db = db;
        }

        [HttpPost("create")]
        public ActionResult CreateEmployee(Employee employee)
        {
            try
            {
                var data=new EmployeeRepo(db).Create(employee);
                if(data)
                {
                    return Ok("Employee has been created");
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
                var data = new EmployeeRepo(db).Delete(id);
                if (data)
                {
                    return Ok(new { Success = "true", Message = "Employee Deleted", Payload = data });
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
                var data = new EmployeeRepo(db).Get(id);
                if (data!=null)
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
                var data = new EmployeeRepo(db).GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public ActionResult Update(Employee employee)
        {
            try
            {
                var data = new EmployeeRepo(db).Update(employee);
                if (data)
                {
                    return Ok("Employee Update");
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
