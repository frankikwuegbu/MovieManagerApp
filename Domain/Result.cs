namespace Domain;

public class Result(bool status, string message, object? entity = null)
{
    public bool Status { get; set; } = status;
    public string? Message { get; set; } = message;
    public object? Entity { get; set; } = entity;

    public static Result Success(string message, object? entity = null) =>
        new(true, message, entity);

    public static Result Failure(string message) => new(false, message);
}