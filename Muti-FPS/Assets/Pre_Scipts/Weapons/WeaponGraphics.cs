using UnityEngine;
using Photon.Pun;

public class WeaponGraphics : MonoBehaviourPunCallbacks, IPunObservable
{
    //private PhotonView PW;
    public ParticleSystem[] muzzleFlash;
    public GameObject hitEffectPrefab;
    public Light Firelight;
 
    /*
    private void Start()
    {
        PW = GetComponent<PhotonView>();
    }
    */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.IsWriting)
        {
            for (int i = 0; i < muzzleFlash.Length; i++)
            {
                stream.SendNext(muzzleFlash[i]);
            }
            stream.SendNext(Firelight);
            stream.SendNext(hitEffectPrefab);
        }
        else
        {
            for (int i = 0; i < muzzleFlash.Length; i++)
            {
                muzzleFlash[i] = (ParticleSystem)stream.ReceiveNext();
            }
            // 非為玩家本人的狀態, 單純接收更新的資料
            Firelight = (Light)stream.ReceiveNext();
            hitEffectPrefab = (GameObject)stream.ReceiveNext();
        }
        */
    }
}
