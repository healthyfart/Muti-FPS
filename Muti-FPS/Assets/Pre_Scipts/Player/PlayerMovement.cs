using UnityEngine;
using Photon.Pun;


public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    private CharacterController controller;

    public float gravity = -9.81f;
    public float speed = 12f;
    public float jumpHeight = 12f;


    [Header("Audio Clips")]
    [Tooltip("The audio clip that is played while walking."), SerializeField]
    private AudioClip walkingSound;
    private AudioSource audioSource;

    public Transform groundCheck;
    public Transform m_CeilingCheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool jumping = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        audioSource.clip = walkingSound;
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (z != 0 && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else if (z == 0)
            {
                audioSource.Pause();
            }

            velocity.y += gravity * Time.deltaTime;

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            Vector3 move = transform.right * x + transform.forward * z;


            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumping = true;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            if (jumping)
            {
                controller.Move(move * speed * Time.deltaTime);
                jumping = false;
            }
            else
            {
                controller.Move(move * speed * Time.deltaTime);
            }

            controller.Move(velocity * Time.deltaTime);
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();
    }
}
