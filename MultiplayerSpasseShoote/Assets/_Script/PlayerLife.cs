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
    public void TakeDamage(int _dmg, int _ID, string _nickName)
    {
        string _nameKiller = PhotonView.Find(_ID).name;
        Debug.LogFormat( "{0} damages by {1} !", _dmg, _nickName);
        myLife -= _dmg;

        if (myLife <= 0)
        {
            Death(_nickName);
        }
    }

    private void Death(string _nameKiller)
    {
        string _message = " dead by " + _nameKiller + " !";
        string _playerName = PhotonNetwork.NickName;
        Debug.Log(_playerName + _message);
        if(PV.IsMine)
            Tchat.PV_Tchat.RPC(nameof(Tchat.AddMessageToTchat), RpcTarget.MasterClient, 
            _playerName, _message);
        myLife = myLifeMax;
    }
}
