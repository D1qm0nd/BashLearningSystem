using System.Diagnostics;
using System.Text;

namespace CommandExecution;

public class CommandExecutor
{
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
        return ExecuteUnixCommand(command);
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