using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovableEnemy
{
    void MoveEnemy(Vector3 direction);
    void LookEnemy(Vector3 direction);

}
