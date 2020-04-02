using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using TMPro;

public class WeaponManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView PW;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    public TMP_Text CurrentAmmo_Text;
    public TMP_Text TotalAmmo_Text;

    public float recoil;
    public float firerate = 0.3f;
    public float lightDuration = 0.02f;
    public float range = 100f;
    public float limit = 0.17f;
    public int damage = 20;
    public int TotalAmmo = 60;
    public int currentAmmo = 30;
    public int MaxAmmo = 30;

    public class Weapons
    {
        public class AK47
        {
            public float recoil;
            public float firerate = 0.3f;
            public float lightDuration = 0.02f;
            public float range = 100f;
            public float limit = 0.5f;
            public int damage = 20;
            public GameObject graphics;
        }
        public class Sniper
        {
            public float recoil;
            public float firerate = 0.3f;
            public float lightDuration = 0.02f;
            public float range = 100f;
            public float limit = 0.5f;
            public int damage = 20;
            public GameObject graphics;
        }
    }

    [SerializeField]
    private Transform weaponHolder;

    ///weapon Graphics
    private WeaponGraphics currentGraphics;
    public ParticleSystem[] muzzleFlash;
    public GameObject hitEffectPrefab;
    public Light Firelight;

    void Start()
    {
        PW = GetComponent<PhotonView>();
        TotalAmmo_Text.SetText(TotalAmmo.ToString());
        CurrentAmmo_Text.SetText(currentAmmo.ToString());
        // EquipWeapon(AK47);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(DelayReload());   
        }   
        CurrentAmmo_Text.SetText(currentAmmo.ToString());
    }


    /*
    public Weapons GetCurrentWeapon()
    {
        // return currentWeapon;
    }
    */
    void EquipWeapon(PlayerWeapon _weapon)
    {

        // currentWeapon = _weapon;
        /*
        GameObject SpawnWeapons = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        SpawnWeapons.transform.SetParent(weaponHolder);
        
        currentGraphics = SpawnWeapons.GetComponent<WeaponGraphics>();
        if(currentGraphics == null)
        {
            Debug.LogError("No Particle Graphics Attach : " + SpawnWeapons.name);
        }


        if (photonView.IsMine)
            Util.SetLayerRecursively(SpawnWeapons, LayerMask.NameToLayer(weaponLayerName));
         */
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ((IPunObservable)currentGraphics).OnPhotonSerializeView(stream, info);
    }

    private IEnumerator DelayReload()
    {
        yield return new WaitForSeconds(1.5f);
        TotalAmmo_Text.SetText(TotalAmmo.ToString());
    }
}
