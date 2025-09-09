namespace Contract_Monthly_System.Models
{
    public class User
    {

        public int Id { get; set; }
        public string UserFullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public string Role { get; set; } = "";

    }
}
