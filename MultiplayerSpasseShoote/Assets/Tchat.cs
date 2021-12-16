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
        
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PV_Tchat.RPC(nameof(AddMessageToTchat), RpcTarget.MasterClient,
        //        PhotonNetwork.NickName, " a tué rien...");
        //}
    }

    private void Start()
    {
        tchat.text = null;
    }

    [PunRPC]
    public void AddMessageToTchat(string _namePlayer, string _message)
    {
        string[] _text = tchat.text.Split('\n');
        //line 1 = ligne 2
        //line 2 = ligne 1
        //line 3 = nouveau message
        tchat.text = _namePlayer + " : " + _message + "\n";

        for (int i = 0; i < 3; i++)
        {
            if (i < _text.Length)
                tchat.text += _text[i] + "\n";
        }

        //tchat.text = _text[1] + "\n "+
        //    _text[2] + "\n "
        //    + _namePlayer + " : " + _message;    
        PV_Tchat.RPC(nameof(AddMessageToTchatForOther), RpcTarget.Others, tchat.text);
    }

    [PunRPC]
    private void AddMessageToTchatForOther(string _tchat)
    {
        tchat.text = _tchat;
    }
}
