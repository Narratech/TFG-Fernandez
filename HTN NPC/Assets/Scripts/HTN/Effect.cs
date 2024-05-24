
public class Effect
{
    private string id;
    private object value;

    public Effect(string id, object value)
    {
        this.id = id;
        this.value = value;
    }

    public void Apply(WorldState state)
    {
        state.ChangeValue(id, value);
    }
}
