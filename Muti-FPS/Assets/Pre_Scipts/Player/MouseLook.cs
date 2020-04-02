using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MouseLook : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private AudioListener AL;

    public Transform playerBody;

    public float mouseSensitivity = 80f;
    public float xRotation = 0f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!photonView.IsMine)
        {
            cam.enabled = false;
            AL.enabled = false;
        }
    } 

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 2.5f;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 2.5f;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.transform.localRotation = Quaternion.Euler (xRotation, 0f , 0f);
            playerBody.Rotate(Vector3.up * mouseX);

        }
        
    }

}
