using EmployeeManagement.Server.Data;
using EmployeeManagement.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EmployeesController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            return await _db.Employees.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            var employee = await _db.Employees.Include(x => x.Experiences).FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Post(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee.Id;
        }
        [HttpPut]

        public async Task<ActionResult> Put(Employee employee)
        {
            _db.Entry(employee).State = EntityState.Modified;
            foreach (var experience in employee.Experiences)
            {
                if (experience.Id != 0)
                {
                    _db.Entry(experience).State = EntityState.Modified;
                }
                else
                {
                    _db.Entry(experience).State = EntityState.Added;
                }
            }
            var idOfExperiences = employee.Experiences.Select(x => x.Id).ToList();
            var experienceToDelete = await _db.Experiences.Where(x => !idOfExperiences.Contains(x.Id) && x.EmployeeId == employee.Id).ToListAsync();
            _db.RemoveRange(experienceToDelete);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _db.Employees.Include(s => s.Experiences).FirstOrDefaultAsync(s => s.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            _db.Experiences.RemoveRange(employee.Experiences);
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
