namespace psuCollege.Models
{
    public class CourseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; } 
        public ProfessorModel Professor { get; set; }
        public DaysModel Days { get; set; }
        public string ListDays { get; set; }
    }
}
