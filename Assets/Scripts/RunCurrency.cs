public class RunCurrency
{
    public int Coins { get; private set; }
    public void AddCoins(int amount) { Coins += amount; }
    public void Reset() { Coins = 0; }
}