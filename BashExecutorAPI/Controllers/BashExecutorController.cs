using CommandExecution;
using Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BashExecutorAPI.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet("{command}")]
    public string Execute(string command)
    {
        return _commandExecutor.ExecuteCommand(command);
    }
}