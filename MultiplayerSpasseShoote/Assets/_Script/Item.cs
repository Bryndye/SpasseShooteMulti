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
        if (collision.TryGetComponent(out PhotonView _pv) && collision.TryGetComponent(out PlayerShoot _playerShoot))
        {
            newOwner = _playerShoot;
            PV.RPC(nameof(SwitchWeapon), RpcTarget.All, _pv.ViewID);
        }
    }

    [PunRPC]
    private void SwitchWeapon(int _ID)
    {
        PlayerShoot _ps = PhotonView.Find(_ID).GetComponent<PlayerShoot>();
        _ps.MyWeapon = WeaponItem;
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        //PhotonNetwork.Destroy(PV); CAN'T DESTROY THIS GAMEOBJECT BUT IDK WHY
    }
}
