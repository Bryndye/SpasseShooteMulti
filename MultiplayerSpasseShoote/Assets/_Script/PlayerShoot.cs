using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    PhotonView PV;

    public WeaponScriptable MyWeapon;
    [SerializeField] Transform spawnBullet;

    float timeShooting;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            timeShooting += Time.deltaTime;
            if (Input.GetAxis("Fire1") > 0 && timeShooting > MyWeapon.FireRate)
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        GameObject _bullet = PhotonNetwork.Instantiate("Prefab/" + MyWeapon.Bullet.name, spawnBullet.position, spawnBullet.rotation);

        //GameObject _bullet = Instantiate(MyWeapon.Bullet, spawnBullet.position, spawnBullet.rotation);
        _bullet.GetComponent<Player_Bullet>().ID_shooter = PV.ViewID;

        timeShooting = 0;
    }

}
