using Player;
using UnityEngine;

public class LevelEndTrigger : LevelTrigger
{
    protected override void OnTrigger(Collider other)
    {
        //SavePlayerData(other); //TODO Que hace esto?
        PlayerHelper.DisableInput();
        LoadNextLevel();
    }
}