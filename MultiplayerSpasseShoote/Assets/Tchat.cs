using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tchat : MonoBehaviour
{
    public static PhotonView PV_Tchat;

    [Header("UI tchat")]
    [SerializeField] Text tchat;

    private void Awake()
    {
        PV_Tchat = GetComponent<PhotonView>();      
    }

    private void Start()
    {
        tchat.text = null;

        if (PhotonNetwork.IsMasterClient)
        {
            PV_Tchat.RPC(nameof(AddMessageToTchat), RpcTarget.MasterClient,
                PhotonNetwork.NickName, "a tué rien...", null);
        }
    }

    [PunRPC]
    public void AddMessageToTchat(string _namePlayer, string _message, string _nameKiller)
    {
        string[] _text = tchat.text.Split('\n');

        //When kill : PLAYER 1 killed PLAYER 2
        //Join/ left room : PLAYER 1 left the room

        string _color = _namePlayer == PhotonNetwork.NickName ? "<color=cyan>" : "<color=red>";
        string _colorKiller = _nameKiller == PhotonNetwork.NickName ? "<color=cyan>" : "<color=red>";
        
        tchat.text = string.IsNullOrEmpty(_nameKiller) ? 
            _color + _namePlayer + "</color>" + " " + _message + "\n" :
            _colorKiller + _nameKiller + "</color>" +" "+ _message + _color + _namePlayer + "</color>" + "\n";

        for (int i = 0; i < 3; i++)
        {
            if (i < _text.Length)
                tchat.text += _text[i] + "\n";
        }

        //PV_Tchat.RPC(nameof(AddMessageToTchatForOther), RpcTarget.Others, tchat.text);
    }

    [PunRPC]
    private void AddMessageToTchatForOther(string _tchat)
    {
    }
}
