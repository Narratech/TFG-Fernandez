
public enum Status
{
    Continue, Success, Failure
}

public class Operator
{
    protected Status status;
    protected WorldState worldState;

    public Operator(WorldState worldState)
    {
        this.worldState = worldState;
    }

    public virtual void Run() { }
    public virtual void Stop() { }
    public virtual void Reset() { }

    public virtual Status GetStatus()
    {
        return status;
    }
}
