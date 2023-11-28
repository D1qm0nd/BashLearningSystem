namespace ContainerDistributorAPI.Models;

/// <summary>
/// Data about container
/// </summary>
public class ContainerData
{
    public string? Name { get; private set; }
    public string ID { get; private set; }
    public DateTime AppealUTC { get; private set; }
    public DateTime UpdateAppeal()  => AppealUTC = DateTime.UtcNow;

    public ImageData Image;
    
    public ContainerData(string id, string? name, ImageData image)
    {
        ID = id;
        Name = name;
        Image = image;
        UpdateAppeal();
    }
}