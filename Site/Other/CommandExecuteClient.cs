using System.Net;
using System.Net.Sockets;
using System.Text;
using Exceptions;

namespace Site.Other;

public class CommandExecuteClient : IDisposable
{
    private Socket Socket;

    public CommandExecuteClient()
    {
        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ip = Environment.GetEnvironmentVariable("COMMAND_EXECUTOR_IPADDRESS");
        if (ip == null)
            throw new EnvironmentVariableExistingException("COMMAND_EXECUTOR_IPADDRESS");
        var port = Environment.GetEnvironmentVariable("COMMAND_EXECUTOR_PORT");
        if (port == null)
            throw new EnvironmentVariableExistingException("COMMAND_EXECUTOR_PORT");

        if (port.Any(c => !Char.IsDigit(c)))
            throw new ArgumentException("BashExecutorPort");
        Socket.Connect(IPAddress.Parse(ip), int.Parse(port));
        Socket.ReceiveBufferSize = 1024;
    }

    public string SendCommandToExecute(string command)
    {
        Socket.Send(Encoding.UTF8.GetBytes(command));
        var recv = new byte[1024];
        while (Socket.Receive(recv) == 0)
        {
            
        }

        return Encoding.UTF8.GetString(recv);
    }

    public void Dispose()
    {
        Socket.Disconnect(false);
        Socket.Dispose();
    }
}