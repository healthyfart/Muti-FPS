using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerNetwork : MonoBehaviourPunCallbacks
{
    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Instance = this;
        DontDestroyOnLoad(this);

        PlayerName = "Player#" + Random.Range(1000, 9999);
    }
}
