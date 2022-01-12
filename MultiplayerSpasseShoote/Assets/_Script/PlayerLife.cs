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

        myOriginalColor = mySprite.color;
    }


    [PunRPC]
    public void TakeDamage(int _dmg, int _ID)
    {
        if (!isAnimDmg)
            StartCoroutine(HitEffect());

        if (!PV.IsMine)
        {
            return;
        }

        //Debug.LogFormat( "{0} damages by {1} !", _dmg, _nickName);
        myLife -= _dmg;

        if (myLife <= 0)
        {
            Death(_ID);
        }
        PV.RPC(nameof(SetLifeForOther), RpcTarget.OthersBuffered, myLife);
    }

    public SpriteRenderer mySprite;
    Color myOriginalColor;
    bool isAnimDmg;
    private IEnumerator HitEffect()
    {
        isAnimDmg = true;
        mySprite.color = Color.white;

        yield return new WaitForSeconds(0.1f);
        mySprite.color = myOriginalColor;
        isAnimDmg = false;
    }


    [PunRPC]
    private void SetLifeForOther(int _life)
    {
        myLife = _life;
    }



    private void Death(int _IdKiller)
    {
        //Debug.Log(_playerName + _message);

        PV.RPC(nameof(PlayerScore.AddScore), RpcTarget.AllBufferedViaServer,
            ScoreStat.Dead, 1);
        PhotonView.Find(_IdKiller).RPC(nameof(PlayerScore.AddScore), RpcTarget.AllBufferedViaServer,
            ScoreStat.Kill, 1);

        Tchat.PV_Tchat.RPC(nameof(Tchat.AddMessageKill), RpcTarget.All,
           _IdKiller ,"killed " ,PV.ViewID);

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        //Set pos in spawnPoints via GM
        myLife = myLifeMax;
        PV.RPC(nameof(SetLifeForOther), RpcTarget.OthersBuffered, myLife);
    }
}
