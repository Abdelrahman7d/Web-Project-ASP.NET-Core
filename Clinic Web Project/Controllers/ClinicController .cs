using Entity;
using Entity.Core;
using Entity.Criteria;
using Microsoft.AspNetCore.Mvc;
using Business.Custom;

namespace Clinic_Web_Project.Controllers;

[ApiController]
[Route("api/clinics")]
public class ClinicController : ControllerBase
{
    private readonly ClinicBusiness _clinicService;

    public ClinicController(ClinicBusiness clinicService)
    {
        _clinicService = clinicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClinics(ClinicCriteria? criteria = null, string? includes = null)
    {

        var clinics = await _clinicService.GetAllDtoAsync(criteria, Helper.ToStringArray(includes));
        return Ok(clinics);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClinicById(int id, string? includes = null)
    {
        var clinic = await _clinicService.FindEntityIdDtoAsync(id, Helper.ToStringArray(includes));
        if (clinic == null)
        {
            return NotFound();
        }

        return Ok(clinic);
    }

    [HttpPost]
    public async Task<IActionResult> AddClinic([FromBody] Clinic clinic)
    {
        if (clinic == null)
        {
            BadRequest();
        }
        await _clinicService.AddEntityAsync(clinic!);
        return CreatedAtAction(nameof(GetClinicById), new { id = clinic!.Id }, clinic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClinic(int id, [FromBody] Clinic clinic)
    {
        var existingClinic = await _clinicService.FindEntityByIdAsync(id);
        if (existingClinic == null)
        {
            return NotFound();
        }

        clinic.Id = id; // Ensure the ID is set
        await _clinicService.UpdateEntityAsync(clinic);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClinic(int id)
    {

        await _clinicService.DeleteEntityByIdAsync(id);

        return NoContent();
    }
}

