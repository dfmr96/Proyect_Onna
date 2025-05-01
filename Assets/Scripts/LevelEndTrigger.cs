using UnityEngine;

public class LevelEndTrigger : LevelTrigger
{
    protected override void OnTrigger(Collider other)
    {
        SavePlayerData(other);
        LoadNextLevel();
    }
}