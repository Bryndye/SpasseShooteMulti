using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MyLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject panelConnectToMaster;
    [SerializeField] GameObject panelLobby;
    [SerializeField] Text feedbackUI;

    private byte MaxPerPlayerPerRoom = 10;

    bool isConnecting = false;

    public string GameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
    }

    public void ChangeNickName(Text _name)
    {
        PhotonNetwork.NickName = _name.text;
    }

    public void Connect()
    {
        isConnecting = true;
        feedbackUI.text = "";
        panelConnectToMaster.SetActive(false);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = GameVersion;
        feedbackUI.text = "Connecting...";

        //if (PhotonNetwork.IsConnected)
        //{
        //    PhotonNetwork.JoinRandomRoom();
        //    feedbackUI.text = "Join random room...";
        //}
        //else
        //{
        //    PhotonNetwork.ConnectUsingSettings();
        //    PhotonNetwork.GameVersion = GameVersion;
        //    feedbackUI.text = "Connecting...";
        //}
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        if (isConnecting)
        {
            panelLobby.SetActive(true);
            feedbackUI.text = "Connected to Master";
            //PhotonNetwork.JoinRandomRoom();
        }
    }



    public void JoinRandomRoom()
    {
        feedbackUI.text = "Connected to Master : Try to connect to randomRoom";
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        feedbackUI.text = "FAILED to join randomRoom : Try to create a Room";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPerPlayerPerRoom});
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        feedbackUI.text = "Create Room : Enjoy !";

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
