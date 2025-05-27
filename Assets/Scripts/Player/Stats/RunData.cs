using Player.Stats;

public static class RunData
{
    public static RuntimeStats CurrentStats { get; private set; }
    public static RunCurrency CurrentCurrency { get; private set; }

    public static void Initialize(CharacterBaseStats baseStats)
    {
        if (CurrentStats == null) CurrentStats = new RuntimeStats(baseStats);
        if (CurrentCurrency == null) CurrentCurrency = new RunCurrency();
    }
    public static void Clear() 
    { 
        CurrentStats = null;
        CurrentCurrency = null;
    }
}