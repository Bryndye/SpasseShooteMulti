using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    PhotonView PV;
    public int myLifeMax = 100;
    public int myLife = 100;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        myLife = myLifeMax;
    }


    [PunRPC]
    public void TakeDamage(int _dmg)
    {
        myLife -= _dmg;

        if (myLife <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        myLife = myLifeMax;
    }
}
