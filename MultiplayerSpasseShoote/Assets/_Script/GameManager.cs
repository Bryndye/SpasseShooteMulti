using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    //GameManager Custom
    static public GameManager Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private Transform Canvas;

    public List<PlayerLife> PlayersInRoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //jouer co ?
        //Verifier playerprefab != null
        if (PhotonNetwork.IsMasterClient)
        {
            CreateLifePlayerUI();
        }
        else
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                CreateLifePlayerUI();
            }
        }
        PhotonNetwork.Instantiate("Prefab/"+playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        
    }

    #region UI
    [PunRPC]
    private void CreateLifePlayerUI()
    {
        //Associer le gameObject au player qui a fait spawn l'objet
        GameObject _pUI = Instantiate(playerUI, Canvas);
        //PhotonNetwork.CurrentRoom.Players[0].NickName;
    }
    #endregion

    #region Photon CallBacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " is connected !");
        if (PhotonNetwork.IsMasterClient)
        {
            CreateLifePlayerUI();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " is disconnected !");
        //delete gameObject UI
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}
