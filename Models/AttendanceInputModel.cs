namespace EmployeeManagementSystem.Models
{
    public class AttendanceInputModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool IsPresent { get; set; }
        public bool MealTaken { get; set; }
    }
}
