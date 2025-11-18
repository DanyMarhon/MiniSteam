namespace MiniSteam.CustomExceptions
{
    public class MiniSteamException : Exception
    {
        public string ErrorType { get; }

        public MiniSteamException(string errorType)
            : base(GetDefaultMessage(errorType))
        {
            ErrorType = errorType;
        }

        public MiniSteamException(string errorType, Exception innerException)
            : base(GetDefaultMessage(errorType), innerException)
        {
            ErrorType = errorType;
        }

        private static string GetDefaultMessage(string errorType)
        {
            return errorType switch
            {
                // --- Application / Domain ---
                "Mapping" => "An error occurred while mapping data between objects.",
                "Validation" => "The provided data did not pass validation checks.",
                "Repository" => "An error occurred while accessing the data repository.",
                "BusinessRule" => "A business rule violation occurred.",
                "Service" => "An unexpected error occurred in the service layer.",

                // --- Infrastructure ---
                "Database" => "A database error occurred while executing the operation.",
                "Network" => "A network communication error occurred.",
                "ExternalApi" => "An error occurred while communicating with an external API.",
                "FileSystem" => "An error occurred while accessing the file system.",
                "Cache" => "An error occurred while accessing the cache store.",

                // --- Security / Authentication ---
                "Unauthorized" => "User authentication failed or token is invalid.",
                "Forbidden" => "User does not have permission to perform this action.",
                "TokenExpired" => "Authentication token has expired.",
                "UserNotFound" => "User not found in the system.",

                // --- Request / HTTP ---
                "BadRequest" => "The request contains invalid or missing parameters.",
                "NotFound" => "The requested resource could not be found.",
                "Conflict" => "The operation could not be completed due to a conflict with existing data.",
                "Timeout" => "The operation took too long and timed out.",

                // --- Default ---
                _ => "An unknown error occurred in the application."
            };
        }
    }
}