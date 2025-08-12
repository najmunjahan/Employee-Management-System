using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendance
        // Show attendance summary for a selected date (default = today)
        public async Task<IActionResult> Index(DateTime? date)
        {
            DateTime selectedDate = date?.Date ?? DateTime.Today;

            var attendanceData = await _context.Employees
                .Select(emp => new AttendanceInputModel
                {
                    EmployeeId = emp.Id,
                    EmployeeName = emp.Name,
                    IsPresent = _context.Attendances
                                .Where(a => a.EmployeeId == emp.Id && a.Date == selectedDate)
                                .Select(a => (bool?)a.IsPresent).FirstOrDefault() ?? false,
                    MealTaken = _context.Attendances
                                .Where(a => a.EmployeeId == emp.Id && a.Date == selectedDate)
                                .Select(a => (bool?)a.MealTaken).FirstOrDefault() ?? false
                })
                .ToListAsync();

            ViewBag.SelectedDate = selectedDate;

            return View(attendanceData);
        }

        // POST: Attendance/Mark
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Mark(DateTime date, List<AttendanceInputModel> attendances)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var att in attendances)
            {
                var existing = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeId == att.EmployeeId && a.Date == date);

                if (existing != null)
                {
                    existing.IsPresent = att.IsPresent;
                    existing.MealTaken = att.MealTaken;
                    _context.Update(existing);
                }
                else
                {
                    var newAttendance = new Attendance
                    {
                        EmployeeId = att.EmployeeId,
                        Date = date,
                        IsPresent = att.IsPresent,
                        MealTaken = att.MealTaken
                    };
                    _context.Add(newAttendance);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { date = date });
        }
    }
}
