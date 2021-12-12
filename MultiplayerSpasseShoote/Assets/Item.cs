using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    PhotonView PV;
    public WeaponScriptable WeaponItem;

    PlayerShoot newOwner = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerShoot _playerShoot))
        {
            newOwner = _playerShoot;
            PV.RPC(nameof(SwitchWeapon), RpcTarget.OthersBuffered);
        }
    }

    [PunRPC]
    private void SwitchWeapon()
    {
        newOwner.MyWeapon = WeaponItem;
        //PhotonNetwork.Destroy(PV); CAN'T DESTROY THIS GAMEOBJECT BUT IDK WHY
    }
}
