using Business.Custom;
using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Web_Project.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentBusiness _departmentService;
        private readonly ClinicBusiness _clinicService;

        public DepartmentController(DepartmentBusiness departmentService, ClinicBusiness clinicService)
        {
            _departmentService = departmentService;
            _clinicService = clinicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDtoAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.FindEntityByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            await _departmentService.AddEntityAsync(department);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            var existingDepartment = await _departmentService.FindEntityByIdAsync(id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            department.Id = id;
            await _departmentService.UpdateEntityAsync(department);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {

            await _departmentService.DeleteEntityByIdAsync(id);
            return Ok("Department was deleted!");
        }
    }
}