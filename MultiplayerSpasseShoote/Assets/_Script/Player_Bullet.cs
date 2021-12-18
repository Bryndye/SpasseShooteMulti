using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    PhotonView PV;
    Rigidbody2D rb;
    [HideInInspector] public int ID_shooter;

    [Header("Parametre Basic")]
    public int Damage = 10;
    [SerializeField] float mySpeed = 100;
    [SerializeField] float autoDestroyTime = 20;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        if(PV.IsMine)
            Invoke(nameof(AutoDelete), autoDestroyTime);
    }

    private void AutoDelete() => PhotonNetwork.Destroy(PV);

    private void Start()
    {
        rb.AddForce(transform.up * mySpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (collision.TryGetComponent(out PlayerLife _life) && _life.TryGetComponent(out PhotonView _pv))
        {
            if (ID_shooter != _pv.ViewID)
            {
                _pv.RPC(nameof(_life.TakeDamage), RpcTarget.All, Damage, ID_shooter);
            }
        }
        PhotonNetwork.Destroy(PV);
    }
}
