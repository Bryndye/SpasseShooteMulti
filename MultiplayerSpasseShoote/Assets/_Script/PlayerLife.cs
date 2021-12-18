using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MessagesPreset;

public class PlayerLife : MonoBehaviour
{
    
    PhotonView PV;
    PlayerScore myPlayerScore;
    public int myLifeMax = 100;
    public int myLife = 100;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        myPlayerScore = GetComponent<PlayerScore>();
        myLife = myLifeMax;
    }


    [PunRPC]
    public void TakeDamage(int _dmg, int _ID)
    {
        if (!PV.IsMine)
        {
            return;
        }
        string _nameKiller = PhotonView.Find(_ID).name;
        //Debug.LogFormat( "{0} damages by {1} !", _dmg, _nickName);
        myLife -= _dmg;

        if (myLife <= 0)
        {
            Death(_nameKiller, _ID);
        }
        PV.RPC(nameof(SetLifeForOther), RpcTarget.OthersBuffered, myLife);
    }

    [PunRPC]
    private void SetLifeForOther(int _life)
    {
        myLife = _life;
    }
    private void Death(string _nameKiller, int _ID)
    {
        //Debug.Log(_playerName + _message);

        PV.RPC(nameof(PlayerScore.AddScore), RpcTarget.AllBufferedViaServer,
            ScoreStat.Dead, 1);
        PhotonView.Find(_ID).RPC(nameof(PlayerScore.AddScore), RpcTarget.AllBufferedViaServer,
            ScoreStat.Kill, 1);

        Tchat.PV_Tchat.RPC(nameof(Tchat.AddMessageToTchat), RpcTarget.All,
            PhotonNetwork.NickName, "killed ", _nameKiller);

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        //Set pos in spawnPoints via GM
        myLife = myLifeMax;
        PV.RPC(nameof(SetLifeForOther), RpcTarget.OthersBuffered, myLife);
    }
}
