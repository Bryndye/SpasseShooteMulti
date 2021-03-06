using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    //GameManager Custom
    static public GameManager Instance;
    static public PhotonView PV_GM;

    private PhotonView pv_Tchat;
    Tchat tchat;

    [Header("Prefabs/ Basics")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerUIPrefab;
    [SerializeField] private Transform Canvas;
    [SerializeField] private Transform[] spawnsPlayer;

    [Header("Lists Players items")]
    public List<PlayerLife> PlayersInRoom;
    public List<MyPlayerUI> PlayersUIInRoom;
    public List<PlayerScore> PlayersScore;


    private void Awake()
    {
        Instance = this;
        PV_GM = GetComponent<PhotonView>();
    }

    private void Start()
    {
        pv_Tchat = Tchat.PV_Tchat;
        tchat = Tchat.Instance;

        if (!PhotonNetwork.IsConnected)
            return;

        Debug.LogFormat("NameRoom : {0} MaxPlayers : {1}", PhotonNetwork.CurrentRoom.Name,PhotonNetwork.CurrentRoom.MaxPlayers);

        InstantiatePlayerPrefab();

        //TEMPORAIRE
        if(PhotonNetwork.IsMasterClient)
            PV_GM.RPC(nameof(ItemSpawn), RpcTarget.All);
    }

    private void Update()
    {
        UpdateUIPlayers();
    }


    #region PlayerPrefab Start
    private void InstantiatePlayerPrefab()
    {
        GameObject _player = PhotonNetwork.Instantiate("Prefab/" + playerPrefab.name, Vector3.zero, Quaternion.identity);
        int _ID = _player.GetComponent<PhotonView>().ViewID;
        PV_GM.RPC(nameof(AddPlayerPrefabToList), RpcTarget.AllBufferedViaServer, 
            _ID, PhotonNetwork.NickName);

        PV_GM.RPC(nameof(CreateLifePlayerUI), RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName, _ID);
    }

    [PunRPC]
    private void AddPlayerPrefabToList(int _ID, string _nickName)
    {
        //Prevoir de recup dabord le PV puis le PlayerLife
        GameObject _playerInstance = PhotonView.Find(_ID).gameObject;
        _playerInstance.name = _nickName;
        PlayersInRoom.Add(_playerInstance.GetComponent<PlayerLife>());
    }

    #endregion


    #region UI
    [PunRPC]
    private void CreateLifePlayerUI(string _nickName, int _ID)
    {
        //Associer le gameObject au player qui a fait spawn l'objet
        //Ajouter a une liste pour le delete quand il part
        MyPlayerUI _pUI = Instantiate(playerUIPrefab, Canvas).GetComponent<MyPlayerUI>();
        _pUI.PV = PhotonView.Find(_ID);
        _pUI.PlayerName.text = _nickName;
        PlayersUIInRoom.Add(_pUI);
    }

    private void UpdateUIPlayers()
    {
        if (PlayersInRoom.Count > 0)
        {
            //Update all life bar for each players into the room
            for (int i = 0; i < PlayersInRoom.Count; i++)
            {
                PlayersUIInRoom[i].PlayerLife.fillAmount = (float)PlayersInRoom[i].myLife / PlayersInRoom[i].myLifeMax;
            }
        }
    }

    #endregion


    #region Events
    [PunRPC]
    private void ItemSpawn()
    {
        //TEMPORAIRE
        PhotonNetwork.Instantiate("Prefab/Item_Weapon", spawnsPlayer[0].position, Quaternion.identity);
    }
    #endregion


    #region Photon CallBacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.Log(newPlayer.NickName + " is connected !");
        tchat.AddMessage(newPlayer.NickName, " joinned the room !");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.Log(otherPlayer.NickName + " is disconnected !");
        tchat.AddMessage(otherPlayer.NickName, " left the room !");

        //When a player left the room, delete his UI (Name, life bar)
        for (int i = 0; i < PlayersUIInRoom.Count; i++)
        {
            if (PlayersUIInRoom[i].PlayerName.text == otherPlayer.NickName)
            {
                Destroy(PlayersInRoom[i].gameObject);
                PlayersInRoom.Remove(PlayersInRoom[i]);

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
