using CommandExecution;
using Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BashExecutorAPI.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("MyPolicy")]
public class BashExecutorController : ControllerBase
{
    private static CommandExecutor _commandExecutor;

    private readonly ILogger<BashExecutorController> _logger;

    public BashExecutorController(ILogger<BashExecutorController> logger)
    {
        var current_folder = Environment.GetEnvironmentVariable("CURRENT_FOLDER");
        if (current_folder == null)
            throw new EnvironmentVariableExistingException("CURRENT_FOLDER");
        _commandExecutor = new(current_folder);
        _logger = logger;
    }

    [HttpGet("Execute/{command}")]
    public string Execute(string command)
    {
        return ExecuteCommand(command);
    }

    [HttpPost("Execute")]
    public string ExecuteFromBody([FromBody] string command)
    {
        return ExecuteCommand(command);
    }

    private string ExecuteCommand(string command)
    {
        _logger.LogInformation($"Executing {command}");
        try
        {
            var res = _commandExecutor.ExecuteCommand(command);
            _logger.LogInformation($"Result: {res}");
            return res;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}