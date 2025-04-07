using System.Globalization;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;

namespace AzureFunctionsMcp
{
    public class MyMcpTools
    {
        [Function(nameof(GetCurrentTime))]
        public string GetCurrentTime(
        [McpToolTrigger("getcurrenttime", "Gets the current time. If no timezone is specified, the tool will return the time in UTC.")] ToolInvocationContext context,
        [McpToolProperty("timezone", "string", "The name of the timezone.")] string timezone = "UTC"
        )
        {
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

                var response = new
                {
                    timezone = timeZoneInfo.StandardName,
                    time = currentTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    displayName = timeZoneInfo.DisplayName
                };

                return JsonSerializer.Serialize(response);
            }
            catch (TimeZoneNotFoundException)
            {
                return $"The timezone '{timezone}' was not found.";
            }
            catch (InvalidTimeZoneException)
            {
                return $"The timezone '{timezone}' is invalid.";
            }
            catch
            {
                return "Could not get the current time.";
            }
        }
    }
}
