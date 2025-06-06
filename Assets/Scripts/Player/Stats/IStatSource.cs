namespace Player.Stats
{
    public interface IStatSource
    {
        float Get(StatDefinition stat);
    }
}