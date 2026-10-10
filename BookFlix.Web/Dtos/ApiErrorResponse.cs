namespace BookFlix.Web.Dtos
{
    public class ApiErrorResponse(string error)
    {
        public string Error { get; init; } = error;
    }
}
