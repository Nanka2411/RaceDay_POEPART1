
namespace RaceDayMVC.Models
{
    public class ApiLoginResponse
    {
        public string Message { get; set; } = "";

        public int UserID { get; set; }

        public string Email { get; set; } = "";

        public string UserRole { get; set; } = "";
    }
}
