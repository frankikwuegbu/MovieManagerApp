namespace MovieManager.Models;

public class ApiResponse
{
    public ApiResponse(bool status, string message, object? entity = null)
    {
        Status = status;
        Message = message;
        Entity = entity;
    }
    public bool Status { get; set; }
    public string? Message { get; set; }
    public object? Entity { get; set; }
}