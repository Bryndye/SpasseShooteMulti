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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //jouer co ?
        //Verifier playerprefab != null
        
        PhotonNetwork.Instantiate("Prefab/"+playerPrefab.name, Vector3.zero, Quaternion.identity);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " is connected !");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " is disconnected !");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
