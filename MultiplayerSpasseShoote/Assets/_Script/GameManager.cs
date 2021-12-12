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
    PhotonView PV;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerUIPrefab;
    [SerializeField] private Transform Canvas;

    public List<PlayerLife> PlayersInRoom;
    public List<MyPlayerUI> PlayersUIInRoom;

    private void Awake()
    {
        Instance = this;
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC(nameof(CreateLifePlayerUI), RpcTarget.AllBufferedViaServer, PhotonNetwork.MasterClient.NickName);
        }

        GameObject _player = PhotonNetwork.Instantiate("Prefab/"+playerPrefab.name, Vector3.zero, Quaternion.identity);
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    //Can't send script by RPC
        //    PV.RPC(nameof(AddPlayerPrefabToList), RpcTarget.AllBufferedViaServer, _player.GetComponent<PlayerLife>());
        //}
    }

    private void Update()
    {
        UpdateUIPlayers();
    }

    [PunRPC]
    private void AddPlayerPrefabToList(PlayerLife _playerLife)
    {
        PlayersInRoom.Add(_playerLife);
    }

    #region UI
    [PunRPC]
    private void CreateLifePlayerUI(string _nickName)
    {
        //Associer le gameObject au player qui a fait spawn l'objet
        //Ajouter a une liste pour le delete quand il part
        MyPlayerUI _pUI = Instantiate(playerUIPrefab, Canvas).GetComponent<MyPlayerUI>();
        _pUI.PlayerName.text = _nickName;
        PlayersUIInRoom.Add(_pUI);
    }

    private void UpdateUIPlayers()
    {
        if (PlayersInRoom.Count > 0)
        {
            //Update all life bar for each players into the room
            for (int i = 0; i < PlayersUIInRoom.Count; i++)
            {
                PlayersUIInRoom[i].PlayerLife.fillAmount = (float)(PlayersInRoom[i].myLife / PlayersInRoom[i].myLifeMax);
            }
        }
    }
    #endregion


    #region Photon CallBacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " is connected !");
        //When a player enter into the room, create his UI (Name, life bar)
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC(nameof(CreateLifePlayerUI), RpcTarget.AllBufferedViaServer, newPlayer.NickName);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " is disconnected !");

        //When a player left the room, delete his UI (Name, life bar)
        for (int i = 0; i < PlayersUIInRoom.Count; i++)
        {
            if (PlayersUIInRoom[i].PlayerName.text == otherPlayer.NickName)
            {
                Destroy(PlayersUIInRoom[i].gameObject);
                PlayersUIInRoom.Remove(PlayersUIInRoom[i]);
            }
        }

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
