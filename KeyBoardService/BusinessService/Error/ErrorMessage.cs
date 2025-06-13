namespace BusinessService.Error
{
    public static class ErrorMessage
    {
        public static string NotFound(string entity) => 
            $"Date: {DateTime.UtcNow} Warning: No {entity} found.";

        public static string NotExists(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: {entity} does not exist.";

        public static string Exists(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: {entity} already exists.";

        public static string NotEnough(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: Not enough {entity} in stock.";

        public static string FailedUpdate(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: Failed to update {entity}";
        
        public static string FailedDelete(string entity) =>
            $"Date: {DateTime.UtcNow} Warning: Failed to delete {entity}";
    }
}