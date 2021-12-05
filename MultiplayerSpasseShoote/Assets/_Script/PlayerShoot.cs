using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    PhotonView PV;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform spawnBullet;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //tir RPc
                PV.RPC("Fire", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void Fire()
    {
        GameObject _bullet = Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
        _bullet.GetComponent<Rigidbody2D>().AddForce(spawnBullet.up * 100);
    }

}
