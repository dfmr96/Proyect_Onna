using Player.Stats;

public static class RunData
{
    public static RuntimeStats CurrentStats { get; private set; }
    
    public static void SetStats(RuntimeStats stats)
    {
        CurrentStats = stats;
    }

    public static void Clear() { CurrentStats = null; }
}