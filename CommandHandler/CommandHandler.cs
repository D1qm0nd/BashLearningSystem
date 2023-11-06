using System.Net;
using System.Net.Sockets;
using System.Text;
using CommandExecution;
using Exceptions;
using Sockets;

namespace RemoteCommandHandler;

public class CommandHandler
{
    private UnixSocketListener _commandListener;
    private CommandExecutor _commandExecutor;

    public CommandHandler()
    {
        var path = Environment.GetEnvironmentVariable("CURRENT_FOLDER");
        if (path == null)
            throw new EnvironmentVariableExistingException("path");
        Console.WriteLine($"path: {path}");
        var bufferSizeEnv = Environment.GetEnvironmentVariable("BUFFER_SIZE");
        if (bufferSizeEnv == null)
            
        _commandExecutor = new(path);
        _commandListener = new UnixSocketListener(InitializeAction, HandleAction, 1024, 1000);
    }

    private void HandleAction(Socket clientSocket, byte[] buffer)
    {
        var command = Encoding.UTF8.GetString(buffer);
        Console.WriteLine($"Req: {command}");
        var request = _commandExecutor.ExecuteCommand(command);
        clientSocket.Send(Encoding.UTF8.GetBytes(request));
    }

    private Socket InitializeAction()
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.ReceiveBufferSize = 1024;
#if DEBUG
        {
            // var ips = Dns.GetHostAddresses(Dns.GetHostName());
            var ip = IPAddress.Parse("127.0.0.1"); //Dns.GetHostAddresses(Dns.GetHostName())[0];
            // var venvPort = Environment.GetEnvironmentVariable("venv COMMAND_EXECUTOR_PORT");
            // if (venvPort == null || venvPort == String.Empty)
            //     throw new ArgumentException("venv COMMAND_EXECUTOR_PORT was incorrect ot not exists");
            var port = 55555; //Convert.ToInt32(venvPort);
            var endPoint = new IPEndPoint(ip, port);
            Console.WriteLine(endPoint);
            socket.Bind(endPoint);
        }
#else
        {
            var endPoint = new IPEndPoint( IPAddress.Parse("127.0.0.1"), 55555);
            socket.Bind(endPoint);
        }
#endif
        return socket;
    }
}