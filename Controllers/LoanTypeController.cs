using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/loan-types")]
    public class LoanTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoanTypeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoanType type)
        {
            _context.LoanTypes.Add(type);
            await _context.SaveChangesAsync();
            return Ok(type);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.LoanTypes.ToListAsync());
        }
    }
}
