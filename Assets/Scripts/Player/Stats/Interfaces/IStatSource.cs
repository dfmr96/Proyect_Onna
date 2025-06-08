namespace Player.Stats.Interfaces
{
    public interface IStatSource
    {
        float Get(StatDefinition stat);
    }
}