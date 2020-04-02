using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UImanager : MonoBehaviourPunCallbacks
{
    public GameObject MainPanel;
    public GameObject SettingPanel;

    public Slider sensitiveSlider;
    public MouseLook MouseLook;

    private bool inSettingUI = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSettingUI)
            {
                Cursor.visible = true;
                inSettingUI = false;
                Cursor.lockState = CursorLockMode.None;
                MainPanel.SetActive(true);
            }
            else
            {
                Cursor.visible = false;
                inSettingUI = true;
                Cursor.lockState = CursorLockMode.Locked;
                MainPanel.SetActive(false);
            }

        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetMouseSensitivity()
    {
        MouseLook.mouseSensitivity = sensitiveSlider.value * 200f;
    }

}
