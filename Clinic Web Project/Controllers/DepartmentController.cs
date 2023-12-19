using Data.Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Service.Service;

namespace Clinic_Web_Project.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;
        private readonly ClinicService _clinicService;

        public DepartmentController(DepartmentService departmentService, ClinicService clinicService)
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
            var department = await _departmentService.GetEntityByIdAsync(id);
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
            var existingDepartment = await _departmentService.GetEntityByIdAsync(id);
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
            try
            {
                await _departmentService.DeleteEntityAsync(id);
                return NoContent();

            }catch (Exception ex)
            {
                return BadRequest(ex); 
            }  
        }
    }
}