using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public GameObject progressLabel;
    public GameObject controlPanel;
    bool isConnecting = false;

    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };

        PhotonNetwork.CreateRoom(RoomName.text, roomOptions, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("我是第一個進入遊戲室的玩家");
            Debug.Log("我得主動做載入場景 'Demo' 的動作");
            PhotonNetwork.LoadLevel("Demo");
        }
    }

    public override void OnConnectedToMaster()
    {

        Debug.Log("Master Conected");
    }
}
