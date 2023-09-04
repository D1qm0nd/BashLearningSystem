namespace DataModels.Interfaces;

public interface IBashCommand
{
    public string GetFullDescription();

    public string GetAttributesDescription();

    public string GetDescription();
}