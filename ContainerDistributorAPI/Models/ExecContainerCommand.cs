namespace ContainerDistributorAPI;

[Serializable]
public class ExecContainerCommand
{
    public Guid ID { get; set; }
    public string Command { get; set; }
}