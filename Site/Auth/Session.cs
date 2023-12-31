namespace Site;

public class Session<T>
{
    public Guid Id { get; set; }
    public T? Data { get; set; }
    
    public Session(T? data = default)
    {
        Id = Guid.NewGuid();
        Data = data;
    }

}
