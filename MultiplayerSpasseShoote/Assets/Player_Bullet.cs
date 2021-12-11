using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector] public int ID_shooter;

    [Header("Parametre Basic")]
    public int Damage = 10;
    [SerializeField] float mySpeed = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.AddForce(transform.up * mySpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerLife _life))
        {
            if (_life.TryGetComponent(out PhotonView _pv))
            {
                if (ID_shooter != _pv.ViewID)
                {
                    _life.TakeDamage(Damage);
                }
            }
        }
    }
}
