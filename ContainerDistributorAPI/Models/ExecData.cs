namespace ContainerDistributorAPI.Models;

[Serializable]
public class ExecData
{
    public ExecData(Guid id, string command)
    {
        ID = id;
        Command = command;
    }

    public Guid ID { get; set; }
    public string Command { get; set; }
    public string Result { get; set; }
}