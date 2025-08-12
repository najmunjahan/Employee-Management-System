using Employee_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        // store only date part (we'll normalize in code with .Date)
        public DateTime Date { get; set; }

        // true = present, false = absent
        public bool IsPresent { get; set; }

        // true = took meal
        public bool MealTaken { get; set; }

        public Employee Employee { get; set; }
    }
}
