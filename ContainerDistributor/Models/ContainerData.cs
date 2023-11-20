namespace ContainerDistributor.Models;

public class ContainerData
{
    public string ID { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public DateTime AppealTimeUTC { get; private set; }
    public DateTime UpdateAppealTime() => AppealTimeUTC = DateTime.UtcNow;
    public ContainerData(Guid userId, string id)
    {
        UserId = userId;
        ID = id;
        UpdateAppealTime();
    }
}