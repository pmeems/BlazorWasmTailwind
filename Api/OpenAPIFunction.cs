using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BlazorApp.Api;

/// <summary>
/// Open API example function.
/// </summary>
public class OpenApiFunction
{
    private readonly ILogger<OpenApiFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiFunction"/> class.
    /// </summary>
    /// <param name="log">The logger.</param>
    public OpenApiFunction(ILogger<OpenApiFunction> log)
    {
        _logger = log;
    }

    [FunctionName("OpenApiFunction")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Read query string:
        string name = req.Query["name"];

        // Read body:
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        // Deserialize the request body, stringly typed is prefert:
        var data = JsonSerializer.Deserialize<dynamic>(requestBody);

        // If query string is not provided use value from body:
        name ??= data?.name;

        var responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";

        return new OkObjectResult(responseMessage);
    }
}
