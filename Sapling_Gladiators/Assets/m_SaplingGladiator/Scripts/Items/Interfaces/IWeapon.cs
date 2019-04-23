using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon : IItem
{
    int AttackPower { get; set; }
}
