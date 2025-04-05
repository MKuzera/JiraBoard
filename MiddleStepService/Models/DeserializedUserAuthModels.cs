namespace MiddleStepService.Models
{
    public class DeserializedRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class DeserializedRegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }

    public class DeserializedLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class DeserializedLoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }

    public class LoginResponseWithtoken
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
    }

}
