using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheck
{
    bool isAggroed { get; set; }
    bool isWhitinCombatRadius { get; set; }

    void SetAggroStatus(bool IsAggroed);
    void SetCombatRadiusBool(bool IsWhitinCombatRadius);


}
