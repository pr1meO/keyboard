namespace AuthService.API.Error
{
    public static class ErrorMessage
    {
        public static string NotFound(string entity) => 
            $"Date: {DateTime.UtcNow} Warning: No {entity} found.";

        public static string NotExists(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: {entity} does not exist.";

        public static string Exists(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: {entity} already exists.";
    }
}