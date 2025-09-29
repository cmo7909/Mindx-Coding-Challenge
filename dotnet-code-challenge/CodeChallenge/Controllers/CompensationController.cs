using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using CodeChallenge.services;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ICompensationService compService;
        private readonly IEmployeeService employeeService;

        public CompensationController(ICompensationService newCompService, IEmployeeService newEmployeeService)
        {
            compService = newCompService;
            employeeService = newEmployeeService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Compensation payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.EmployeeId) || payload.EffectiveDate == default)
            {
                return BadRequest("employeeId, salary, and effectiveDate are required.");
            }
            var emp = employeeService.GetById(payload.EmployeeId);
            if (emp == null)
            {
                return NotFound($"Payload employee '{payload.EmployeeId}' not found");
            }
            var created = compService.Create(payload);
            return CreatedAtRoute("getCompensationByEmployeeId", new { employeeId = created.EmployeeId }, created);
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetEmployeeById(string employeeId)
        {
            var comp = compService.GetByEmployeeId(employeeId);
            if (comp == null)
            {
                return NotFound();
            }
            return Ok(comp);
        }
    }
}