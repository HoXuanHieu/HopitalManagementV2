namespace HospitalManagement.Client.DTOs
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}