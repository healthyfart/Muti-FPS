using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager Instance;

    [SerializeField]
    private Transform spawnpoint1;
    [SerializeField]
    private Transform spawnpoint2;

    public GameObject localPlayer;
    //public GameObject remotePlayer;


    private void Start()
    {
        Instance = this;

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
            return;
        }

        if (PlayerManager.LocalPlayerInstance == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("spawn Player 1");
                GameObject player1 = PhotonNetwork.Instantiate(Path.Combine("Prefeb", localPlayer.name), spawnpoint1.position, Quaternion.identity);
                player1.name = "player" + PhotonNetwork.CountOfPlayers;
            }
            else
            {
                Debug.Log("spawn Player 2");
                GameObject player2 = PhotonNetwork.Instantiate(Path.Combine("Prefeb", localPlayer.name), spawnpoint2.position, Quaternion.identity);
                player2.name = "player" + PhotonNetwork.CountOfPlayers;
            }

        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("{0} 進入遊戲室", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("我是 Master Client 嗎? {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0} 離開遊戲室", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("我是 Master Client 嗎? {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    void OnLevelWasLoaded(int levelNumber)
    {
        // 若不在Photon的遊戲室內, 則網路有問題..
        if (!PhotonNetwork.InRoom)
            return;
        Debug.Log("我們已進入遊戲場景了,耶~");

    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("我不是 Master Client, 不做載入場景的動作");
        }

        Debug.LogFormat("載入{0}人的場景", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(1);
    }

}


