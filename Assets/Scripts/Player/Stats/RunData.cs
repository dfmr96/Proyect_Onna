using Player.Stats;

public static class RunData
{
    public static RuntimeStats CurrentStats { get; private set; }

    public static void Initialize(CharacterBaseStats baseStats)
    {
        if (CurrentStats == null) CurrentStats = new RuntimeStats(baseStats);
    }
    public static void Clear() { CurrentStats = null; }
    
    
}