using Lib.DataBases;

namespace WebClasses;

public abstract class Session<T> where T: Entity
{
    public Guid Id { get; protected set; }
    public T Account { get; protected set; }
    
    protected void SetAccount(T account)
    {
        Account = account;
    }

    protected void SetId(Guid id)
    {
        Id = id;
    }
}