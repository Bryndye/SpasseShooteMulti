using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponScriptable : ScriptableObject
{
    public GameObject Bullet;
    public int Damage = 10;
    public float FireRate = 0.5f;
}
