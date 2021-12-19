using UnityEngine.Events;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyLauncher : MonoBehaviourPunCallbacks
{
    bool isConnecting = false;
    public string GameVersion = "1";

    [Header("Menu general")]
    [SerializeField] GameObject panelConnectToMaster;
    [SerializeField] GameObject panelLobby;
    [SerializeField] Text feedbackUI;
    public UnityEvent EnterGame;
    public UnityEvent EnterLobby;

    [Header("CreateRoom")]
    private byte MaxPerPlayerPerRoom = 10;
    [SerializeField] InputField inputFieldRoomName;
    [SerializeField] Dropdown dropDownMaxPlayer;

    [Header("JoinLobby")]
    [SerializeField] GameObject prefabBtnRoom;
    [SerializeField] Transform parentListBtn;
    public List<Button> ListBtnRooms;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
    }

    #region CreateRoom
    public void CreateRoom()
    {
        string _nameRoom = inputFieldRoomName.text;
        byte _maxPlayers = (byte)int.Parse(dropDownMaxPlayer.options[dropDownMaxPlayer.value].text);
        PhotonNetwork.CreateRoom(_nameRoom, new RoomOptions { MaxPlayers = _maxPlayers });
    }
    #endregion

    #region ShowRoom/Join
    public void ListGames()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            feedbackUI.text = "Connected to lobby";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Button btn in ListBtnRooms)
        {
            Destroy(btn);
        }
        ListBtnRooms.Clear();
        foreach (RoomInfo info in roomList)
        {
            Debug.LogFormat("{0} Players : {1}/{2}",info.Name, info.PlayerCount,info.MaxPlayers );
            Button _room = Instantiate(prefabBtnRoom, parentListBtn).GetComponent<Button>();
            _room.onClick.AddListener(delegate { PhotonNetwork.JoinRoom(info.Name); });
            _room.GetComponentInChildren<Text>().text = info.Name+" Players : " + info.PlayerCount+ "/" +info.MaxPlayers;
            ListBtnRooms.Add(_room);
        }
        //Debug.Log(roomList.Count);
    }
    #endregion

    #region fct btn
    public void ChangeNickName(Text _name)
    {
        PhotonNetwork.NickName = _name.text;
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            EnterGame.Invoke();
            isConnecting = true;
            feedbackUI.text = "";

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
            feedbackUI.text = "Connecting...";
        }
        else
        {
            EnterLobby.Invoke();
        }
    }

    #endregion


    #region CallBacks PUN

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            EnterLobby.Invoke();
            feedbackUI.text = "Connected to Master";
        }
    }



    public void JoinRandomRoom()
    {
        feedbackUI.text = "Connected to Master : Try to connect to randomRoom";
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedbackUI.text = "FAILED to join randomRoom : Try to create a Room";
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions { MaxPlayers = MaxPerPlayerPerRoom});
    }

    public override void OnJoinedRoom()
    {
        feedbackUI.text = "Create Room : Enjoy !";

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    #endregion
}
