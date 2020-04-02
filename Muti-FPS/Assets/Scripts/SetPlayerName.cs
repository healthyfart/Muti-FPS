using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;



public class SetPlayerName : MonoBehaviour
{
    /*
    const string playerNamePrefKey = "PlayerName";

    void Start()
    {
        string defaultName = string.Empty;
        TMP_InputField _inputField = GetComponent<TMP_InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        // 設定遊戲玩家的名稱
        PhotonNetwork.NickName = defaultName;
    }
    */
    public void SetName()
    {
        TMP_InputField _inputField = GetComponent<TMP_InputField>();

        if (string.IsNullOrEmpty(_inputField.text))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = _inputField.text;
        //PlayerPrefs.SetString(playerNamePrefKey, _inputField.text);
    }
}
