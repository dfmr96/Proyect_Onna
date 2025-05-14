using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class FinalLevelTrigger : LevelTrigger
{
    [SerializeField] private string sceneName;
    protected override void OnTrigger(Collider other) { PlayerHelper.DisableInput(); LoadLevelByName(sceneName); }
}