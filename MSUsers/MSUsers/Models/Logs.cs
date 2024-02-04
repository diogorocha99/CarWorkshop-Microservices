namespace MSUsers.Models
{
    public class Logs
    {
        public string? UserId { get; set; }

        public string Message { get; set; }

        public Logs(string userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}
