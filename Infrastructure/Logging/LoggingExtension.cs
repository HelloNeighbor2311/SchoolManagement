using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SchoolManagement.Infrastructure.Logging
{
    public static class LoggingExtension
    {
        public static void LogUserLogin(this ILogger logger, int userId, string userName)
        {
            logger.LogInformation("User logged in successfully. UserId: {UserId}, Username: {Username}", userId, userName);
        }
        public static void LogLoginFailed(this ILogger logger, string userName)
        {
            logger.LogWarning("Login failed for Username. Username: {Username}", userName);
        }
        public static void LogUserRegistered(this ILogger logger,int userId, string userName, string roleName)
        {
            logger.LogInformation("User register successfully. UserId : {UserId}, Username: {Username}, Role name: {RoleName}", userId, userName, roleName);
        }
        public static void LogTokenRefreshed(this ILogger logger, int userId)
        {
            logger.LogInformation(
                "Token refreshed for UserId: {UserId}",
                userId);
        }

        public static void LogTokenRevoked(this ILogger logger, int userId)
        {
            logger.LogInformation(
                "Token revoked for UserId: {UserId}.",
                userId);
        }

        //CRUD operation logging
        public static void LogEntityCreated(this ILogger logger, string entityName, int id)
        {
            logger.LogInformation("{EntityName} created successfully. Id: {Id}", entityName, id);
        }
        public static void LogEntityUpdated(this ILogger logger, string entityName, int id)
        {
            logger.LogInformation(
                "{EntityName} updated successfully. Id: {Id}",
                entityName, id);
        }

        public static void LogEntityDeleted(this ILogger logger, string entityName, int id)
        {
            logger.LogInformation(
                "{EntityName} deleted successfully. Id: {Id}",
                entityName, id);
        }

        public static void LogEntityNotFound(this ILogger logger, string entityName, int id)
        {
            logger.LogWarning(
                "{EntityName} not found with Id: {Id}",
                entityName, id);
        }

        public static IDisposable? BeginOperationScope<T>(this ILogger<T> logger, string operationName, params (string Key, object Value)[] parameters)
        {
            var props = new Dictionary<string, object>
            {
                ["Operation"] = operationName,
                ["Service"] = typeof(T).Name
            };
            foreach (var (key, value) in parameters)
            {
                props[key] = value;
            }
            return logger.BeginScope(props);
        }
        public static void LogOperationStart<T>(this ILogger<T> logger, string operationName, params object[] args)
        {
            logger.LogDebug("Starting operation: {Operation} with args: {@Args}", operationName, args);
        }
        public static void LogOperationComplete<T>(this ILogger<T> logger,string operationName,long elapsedMs)
        {
            if (elapsedMs > 1000)
            {
                logger.LogWarning("Operation {Operation} completed in {ElapsedMs}ms (SLOW)",operationName, elapsedMs);
            }
            else
            {
                logger.LogDebug("Operation {Operation} completed in {ElapsedMs}ms",operationName, elapsedMs);
            }
        }
        public static void LogOperationError<T>(this ILogger<T> logger, string operationName, Exception ex, params object[] context)
        {
            logger.LogError(ex,
                "Error in operation {Operation}. Context: {@Context}",
                operationName, context);
        }
        //Validation
        public static void LogValidationWarning<T>(this ILogger<T> logger, string field, string error)
        {
            logger.LogWarning("Validation error - Filed: {Field}, Error: {Error}", field, error);
        }
        //Performance
        public static OperationTimer<T> TimeOperation<T>(this ILogger<T> logger, string operationName)
        {
            return new OperationTimer<T>(logger, operationName);
        }

        
        
    }
}
