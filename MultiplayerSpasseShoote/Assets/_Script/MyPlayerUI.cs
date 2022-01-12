using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerUI : MonoBehaviour
{
    [Header("Player scripts")]
    public PhotonView PV;
    public PlayerScore MyPlayerScore;

    [Header("UI for His Player")]
    public Text PlayerName, AllScores;
    public Image PlayerLife, WeaponIcon, Border;

    public TeamColor ColorTeam;

    private void Start()
    {
        MyPlayerScore = PV.GetComponent<PlayerScore>();

        Border.color = ColorTeam.SetColorTeam(MyPlayerScore.MyTeam, PV.IsMine);
    }

    private void SetColorTeam()
    {
        if (MyPlayerScore != null)
        {
            switch (MyPlayerScore.MyTeam)
            {
                case Teams.NoTeam:
                    if (PV.IsMine)
                    {
                        Border.color = ColorTeam.ColorMe;
                    }
                    else
                    {
                        Border.color = ColorTeam.ColorE;
                    }
                    break;

                case Teams.Blue:
                    if (PV.IsMine)
                    {
                        Border.color = ColorTeam.ColorMe;
                    }
                    else
                    {
                        Border.color = ColorTeam.ColorA;
                    }
                    break;

                case Teams.Red:
                    Border.color = ColorTeam.ColorE;
                    break;

                default:
                    Border.color = Color.white;
                    break;
            }
        }
    }

    private void Update()
    {
        AllScores.text = "K: "+ MyPlayerScore.Kills + " D: " + MyPlayerScore.Deads + " A: " + MyPlayerScore.Assists
            + "\nScores: " + MyPlayerScore.Score;
    }
}
