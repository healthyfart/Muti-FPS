using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class shoot : MonoBehaviourPun
{

    public Camera MainCam;
    public Camera FPScam;
    public MouseLook mouseLook;
    public GameObject InGameUI;
    public GameObject PlayerUI;
    public GameObject crosshair;
    public GameObject Hitcrosshair;
    public MouseLook Mousecontr;
    private Animator animator;
    private PlayerMovement playerMovement;
    private WeaponManager weaponManager;
    private PhotonView PV;

    //   [SerializeField]
    //   private GameObject weaponGFX;

    [SerializeField]
    private string weaponLayerName = "Weapon";
    private float startTime;
    private float holdTime;
    private bool isAimed = false;
    private bool singleFire = true;
    private bool canshoot = true;
    private bool inSettingUI = true;

    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>();
        if (!photonView.IsMine)
        {
            PlayerUI.SetActive(false);
        }
        //   weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    void Update()
    {


        if (!PV.IsMine)
        {
            return;
        }
        #region UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSettingUI)
            {
                inSettingUI = false;
                PlayerUI.SetActive(false);
                //StopPanel.SetActive(true);
            }
            else
            {
                inSettingUI = true;
                PlayerUI.SetActive(true);
                //StopPanel.SetActive(false);
            }

        }


        #endregion
        if (Input.GetButtonDown("Fire1"))
        {

            startTime = Time.time;

            if (singleFire)
            {
                if (isAimed && weaponManager.currentAmmo >= 1)
                {
                    animator.Play("Aim_shooting", -1, 0f);
                }
                else
                {
                    animator.Play("shooting", -1, 0f);
                }
                if(weaponManager.currentAmmo >= 1 && PlayerUI)
                    photonView.RPC("Shoot", RpcTarget.All);
            }
            singleFire = false;

        }
        if (Input.GetButton("Fire1"))
        {

            if (Time.time - startTime > weaponManager.limit && canshoot)
            {
                if (isAimed && weaponManager.currentAmmo >= 1)
                {
                    animator.Play("Aim_shooting", -1, 0f);
                }
                else
                {
                    animator.Play("shooting", -1, 0f);
                }
                canshoot = false;
                StartCoroutine(ShootDelay());

            }

        }
        if (Input.GetButtonUp("Fire1"))
        {

            //currentWeapon.recoil = 0;
            singleFire = true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            isAimed = !isAimed;
            animator.SetBool("isAiming", isAimed);
            FindObjectOfType<AudioManager>().Play("aim");

            if (isAimed)
            {
                MainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("SelfParticles"));
                StartCoroutine(Aimslow());
                crosshair.SetActive(false);

            }
            else
            {
                MainCam.cullingMask |= 1 << LayerMask.NameToLayer("SelfParticles");
                MainCam.fieldOfView = 60;
                crosshair.SetActive(true);
                playerMovement.speed = 12f;
                Mousecontr.mouseSensitivity += 15;
            }

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(weaponManager.TotalAmmo >= 1 && weaponManager.TotalAmmo + weaponManager.currentAmmo >= 30)
            {
                FindObjectOfType<AudioManager>().Play("reload");
                animator.SetBool("isReloading", true);
                StartCoroutine(DelayReload1());
            }
            else if(weaponManager.TotalAmmo >= 1 && weaponManager.TotalAmmo + weaponManager.currentAmmo < 30)
            {
                FindObjectOfType<AudioManager>().Play("reload");
                animator.SetBool("isReloading", true);
                StartCoroutine(DelayReload2());
            }
        }
    }

    [PunRPC]
    void Shoot()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        weaponManager.currentAmmo--; 

        FindObjectOfType<AudioManager>().Play("shoot");
        photonView.RPC("RpcDoShootMuzzleFlash", RpcTarget.All);   
        RaycastHit hit;

        if (Physics.Raycast(FPScam.transform.position, FPScam.transform.forward, out hit, weaponManager.range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                StartCoroutine(ShowHcrosshair());
                CmdPlayerShot(hit.collider.name, weaponManager.damage , hit.point);
            }
            photonView.RPC("CmdOnHit", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RpcDoShootMuzzleFlash()
    {
        FindObjectOfType<AudioManager>().Play("shoot");

        for (int i = 0; i < weaponManager.muzzleFlash.Length; i++)
        {
            StartCoroutine(DelayMuzzleFlash(i));
        }
        StartCoroutine(FlashLight());
    }

    [PunRPC]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        GameObject hiteffect = Instantiate(weaponManager.hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hiteffect, 0.3f);
    }

    void CmdPlayerShot(string _playerID, int damage ,Vector3 hitpoint)
    {

        GameObject player = GameObject.Find(_playerID);
        player.GetComponent<PlayerManager>().photonView.RPC("TakeDamage", RpcTarget.All, damage , hitpoint);

    }

    private IEnumerator FlashLight()
    {
        weaponManager.Firelight.enabled = true;
        yield return new WaitForSeconds(weaponManager.lightDuration);
        weaponManager.Firelight.enabled = false;
    }
    private IEnumerator Aimslow()
    {
        yield return new WaitForSeconds(.15f);
        playerMovement.speed = 6f;
        MainCam.fieldOfView = 30;
        Mousecontr.mouseSensitivity -= 15;

    }
    private IEnumerator Aimshooting()
    {
        animator.SetBool("isAimShooting", true);
        yield return new WaitForSeconds(.001f);
        animator.SetBool("isAimShooting", false);
    }
    private IEnumerator shooting()
    {
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(.001f);
        animator.SetBool("isShooting", false);
    }
    private IEnumerator ShowHcrosshair()
    {
        Hitcrosshair.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Hitcrosshair.SetActive(false);
    }
    private IEnumerator ShootDelay()
    {
        if (weaponManager.currentAmmo >= 1 && PlayerUI)
            photonView.RPC("Shoot", RpcTarget.All);
        yield return new WaitForSeconds(0.115f);
        canshoot = true;
    }
    private IEnumerator DelayMuzzleFlash(int i)
    {
        Debug.Log("DelayMuzzleFlash");
        weaponManager.muzzleFlash[i].gameObject.SetActive(true);
        yield return new WaitForSeconds(.15f);
        weaponManager.muzzleFlash[i].gameObject.SetActive(false);
    }
    private IEnumerator DelayReload1()
    {
        
        yield return new WaitForSeconds(1.49f);
        weaponManager.TotalAmmo -= (weaponManager.MaxAmmo - weaponManager.currentAmmo);
        weaponManager.currentAmmo += (weaponManager.MaxAmmo - weaponManager.currentAmmo);
        animator.SetBool("isReloading", false);
    }
    private IEnumerator DelayReload2()
    {

        yield return new WaitForSeconds(1.49f);
        weaponManager.currentAmmo += weaponManager.TotalAmmo;
        weaponManager.TotalAmmo = 0;
        animator.SetBool("isReloading", false);
    }
}
