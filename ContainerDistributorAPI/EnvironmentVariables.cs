using Exceptions;

namespace ContainerDistributorAPI;

public class EnvironmentVariables
{
    public string ImageName { get; private set; }
    public string ImageTag { get; private set; }
    public int BufferSize { get; private set; }


    public EnvironmentVariables()
    {
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