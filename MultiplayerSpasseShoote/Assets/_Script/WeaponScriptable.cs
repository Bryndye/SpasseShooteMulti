using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Weapon")]
public class WeaponScriptable : ScriptableObject
{
    public GameObject Bullet;
    public int Damage = 10;
    public float FireRate = 0.5f;

    [Header("UI")]
    public Sprite IconWeapon;
}
