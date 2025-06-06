namespace Player.Stats
{
    public interface IStatTarget
    {
        void AddFlatBonus(StatDefinition stat, float value);
        void AddPercentBonus(StatDefinition stat, float percent);
        void AddMultiplierBonus(StatDefinition stat, float factor);
    }
}