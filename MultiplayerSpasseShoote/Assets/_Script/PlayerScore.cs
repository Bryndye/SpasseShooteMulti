using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreStat { Kill, Dead, Assist, Score}
public class PlayerScore : MonoBehaviour
{
    public Teams MyTeam = Teams.NoTeam;
    public int Kills, Deads, Assists, Score;

    [SerializeField] SpriteRenderer outlineSpriteTeam;
    [SerializeField] TeamColor teamColor;

    private void Start()
    {
        //Set color Team TEMP !!!!!!!
        if (PhotonNetwork.NickName == null)
        {
            PhotonNetwork.NickName = "Blayer" + Random.Range(0,1000);
        }
        outlineSpriteTeam.color = teamColor.SetColorTeam(MyTeam, GetComponent<PhotonView>().IsMine);
    }

    [PunRPC]
    public void AddScore(ScoreStat _chooseStat, int _stat)
    {
        switch (_chooseStat)
        {
            case ScoreStat.Kill:
                Kills += _stat;
                break;
            case ScoreStat.Dead:
                Deads += _stat;
                break;
            case ScoreStat.Assist:
                Assists += _stat;
                break;
            case ScoreStat.Score:
                Score += _stat;
                break;
            default:
                break;
        }
    }
}
