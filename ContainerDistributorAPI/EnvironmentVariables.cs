using Exceptions;

namespace ContainerDistributorAPI;

public class EnvironmentVariables
{
    public string PrivateKey { get; private set; }
    public int DelayMinutes { get; private set; }

    public string ImageName { get; private set; }
    public string ImageTag { get; private set; }
    public int BufferSize { get; private set; }


    public EnvironmentVariables()
    {
        var delay = Environment.GetEnvironmentVariable("TRACKER_MINUTE_DELAY");
        DelayMinutes = int.TryParse(delay, out _) ? int.Parse(delay) : 1;
        PrivateKey = GetEnvVariable("DISTRIBUTOR_PRIVATE_KEY");
        ImageName = GetEnvVariable("IMAGE");
        ImageTag = GetEnvVariable("IMAGE_TAG");
        var bufferSize = GetEnvVariable("REQUEST_BUFFER_SIZE");
        BufferSize = int.TryParse(bufferSize, out _) ? int.Parse(bufferSize) : 1024;
    }

    private string GetEnvVariable(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);
        if (variable == null)
            throw new EnvironmentVariableExistingException(name);
        return variable;
    }
}