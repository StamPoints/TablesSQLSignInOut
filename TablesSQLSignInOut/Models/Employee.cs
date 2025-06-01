namespace TablesSQLSignInOut.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public int WorkID { get; set; } // Unique identifier for the employee 
        public string Name { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public int Age { get; set; }    // in years
        public string Email { get; set; } = string.Empty;
        public string UserPage { get; set; } = string.Empty;
        public int WorkTimeTotal { get; set; } // in minutes
        public string JobTitle { get; set; } = string.Empty;
        public string privileges { get; set; } = string.Empty;
    }

}
