using System.Diagnostics;
using System.Text;
using CommandExecution.Enums;

namespace CommandExecution;

public class CommandExecutor
{
    public ExecutorSystem System { get; set; } = ExecutorSystem.UNIX;

    public string? _currentPath { get; init; }
    
    public CommandExecutor(string? currentPath=null)
    {
        if (currentPath == null)
            _currentPath = "/home";
        else
            _currentPath = currentPath;
    }

    public string ExecuteCommand(string command)
    {
        switch (System)
        {
            case ExecutorSystem.UNIX:
                return ExecuteUnixCommand(command);
            case ExecutorSystem.WINDOWS:
                return ExecuteWindowsCommand();
            case ExecutorSystem.MAC:
                return ExecuteMacCommand();
            default:
                throw new InvalidOperationException();
        }
    }

    private string ExecuteMacCommand()
    {
        throw new NotImplementedException();
    }

    private string ExecuteWindowsCommand()
    {
        throw new NotImplementedException();
    }

    public string ExecuteUnixCommand(string command)
    {
        Process proc = new Process();
        proc.StartInfo.WorkingDirectory = _currentPath;
        proc.StartInfo.FileName = "bash";
        proc.StartInfo.Arguments = $"-c \"{command}\"";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();
        proc.WaitForExit();
        StringBuilder sb = new();
        while (!proc.StandardOutput.EndOfStream)
        {
            sb.AppendLine(proc.StandardOutput.ReadLine());
        }
        while (!proc.StandardError.EndOfStream)
        {
            sb.AppendLine(proc.StandardError.ReadLine());
        }

        return sb.ToString();
    }
}