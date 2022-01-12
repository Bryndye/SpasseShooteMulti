using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Tchat : MonoBehaviour
{
    public static Tchat Instance;
    public static PhotonView PV_Tchat;

    [Header("UI tchat")]
    [SerializeField] Transform logParent;
    int indexLog = 0;
    [SerializeField] GameObject prefabLog;
    [SerializeField] TeamColor teamColor;
    public List<Log> ListLogsInstance;

    private void Awake()
    {
        Instance = this;
        PV_Tchat = GetComponent<PhotonView>();      
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Log _log = Instantiate(prefabLog, logParent).GetComponent<Log>();
            ListLogsInstance.Add(_log);
            _log.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhotonNetwork.IsMasterClient) //Test call logs message
            {
                PV_Tchat.RPC(nameof(AddMessage), RpcTarget.All,
                    PhotonNetwork.NickName, "a tué rien...");
            }
        }*/
    }

    [PunRPC]
    public void AddMessageKill(int _IdKiller, string _message, int _IdPlayer)
    {
        //recuperer les ID des PV dead & killer
        //recupere les noms
        //Savoir si le local fait partie du msg
        //Set la couleur dans le msg
        //Assembler le tout

        PhotonView _PvKiller = PhotonView.Find(_IdKiller);
        PhotonView _PvPlayer = PhotonView.Find(_IdPlayer);

        PlayerScore _ScoreKiller = _PvKiller.GetComponent<PlayerScore>();
        PlayerScore _ScorePlayer = _PvPlayer.GetComponent<PlayerScore>();

        bool _isLocalKiller = PhotonNetwork.NickName == _PvKiller.name;
        bool _isLocalPlayer = PhotonNetwork.NickName == _PvPlayer.name;

        //Set color with Team Color
        Color _cLeft = teamColor.SetColorTeam(_ScoreKiller.MyTeam, _isLocalKiller);
        Color _cRight = teamColor.SetColorTeam(_ScorePlayer.MyTeam, _isLocalPlayer);

        ListLogsInstance[indexLog].KillerName.color = _cLeft;
        ListLogsInstance[indexLog].PlayerName.color = _cRight;


        SetMessage(_PvPlayer.name, _message, _PvKiller.name, _isLocalPlayer || _isLocalKiller);
    }

    [PunRPC]
    public void AddMessage(string _namePlayer, string _message)
    {
        ListLogsInstance[indexLog].KillerName.color = Color.white;
        ListLogsInstance[indexLog].PlayerName.text = null;
        SetMessage(null, _message, _namePlayer);
    }

    private void SetMessage(string _namePlayer, string _msg ,string _nameKiller, bool _isLocal = false)
    {
        //Reset Animations & active log
        ListLogsInstance[indexLog].gameObject.SetActive(true);

        //Set info log
        ListLogsInstance[indexLog].Fond.enabled = _isLocal;

        ListLogsInstance[indexLog].KillerName.text = _nameKiller;
        ListLogsInstance[indexLog].Message.text = _msg;
        ListLogsInstance[indexLog].PlayerName.text = _namePlayer;

        //ListLogsInstance[indexLog].Icon.enabled = Random.Range(0,2) == 1 ? true : false;

        indexLog++;
        if (indexLog >= 10)
            indexLog = 0;
    }
}
