using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerUI : MonoBehaviour
{
    public PhotonView PV;
    public PlayerScore MyPlayerScore;
    public Text PlayerName, AllScores;
    public Image PlayerLife;

    private void Start()
    {
        MyPlayerScore = PV.GetComponent<PlayerScore>();
    }

    private void Update()
    {
        AllScores.text = "K: "+ MyPlayerScore.Kills + " D: " + MyPlayerScore.Deads + " A: " + MyPlayerScore.Assists
            + "\nScores: " + MyPlayerScore.Score;
    }
}
