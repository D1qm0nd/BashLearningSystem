namespace ContainerDistributorAPI.Models;

public class ImageData
{
    public string Image { get; private set; }
    public string Tag { get; private set; }

    public ImageData(string image, string tag)
    {
        Image = image;
        Tag = tag;
    }

    public override string ToString() => $"{Image}:{Tag}";
}