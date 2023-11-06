using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using Site.Other;

namespace Site.Controllers;

public class BashExecutor
{
    // GET
    // [HttpGet]
    // public string SendCommandExecuteRequest([Bind("terminalConsole")] string command)
    // {
    //     throw new NotSupportedException();
    //     var a = new CommandExecuteClient();
    //     var res = a.SendCommandToExecute(command);
    //     return res;
    // }
    
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null;

    [JSInvokable]
    private string SendCommandExecuteRequest([Bind("terminalConsole")] string command)
    {
        var a = new CommandExecuteClient();
        var res = a.SendCommandToExecute(command);
        return res;
    }

    private async Task<string> SendCommandExecuteRequestAsync(string command)
    {
        return await JsRuntime.InvokeAsync<string>("SendCommandExecuteRequest", command);
    }
}