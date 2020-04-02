using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }

   public void OnClickJoinRoom(string roomName)
   {
        PhotonNetwork.JoinRoom(roomName);
    }
}
