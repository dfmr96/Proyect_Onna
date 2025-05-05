using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBEndTrigger : LevelTrigger
{
    protected override void OnTrigger(Collider other) { LoadNextLevel(); }
}
