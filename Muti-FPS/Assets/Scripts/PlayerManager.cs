using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;
    private PhotonView PV;

    #region TMP
    public TMP_Text health;
    private float mineScore = 0;
    private float enemyScore = 0;
    #endregion

    #region health
    [SerializeField]
    private int maxHealth = 200;
    private int currentHealth;
    public TMP_Text EnemyScore;
    public TMP_Text MineScore;
    #endregion

    #region Dead
    [SerializeField]
    private GameObject[] disableOnDeath_Obj;
    [SerializeField]
    private MeshRenderer disableOnDeath_MR;
    [SerializeField]
    private GameObject deathtEffect;
    private bool[] wasEnabled;
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    #endregion

    #region Spawn
    [SerializeField]
    private Vector3 Min;
    [SerializeField]
    private Vector3 Max;

    private float _xAxis;
    private float _yAxis;
    private float _zAxis; //If you need this, use it
    private Vector3 _randomPosition;
    #endregion

    #region GetHit
    [SerializeField]
    private GameObject BloodScrean;
    [SerializeField]
    private GameObject BloodMuzzle;
    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        currentHealth = maxHealth;

        if (!photonView.IsMine)
        {
            EnemyScore.enabled = false;
        }
        else
        {
            MineScore.enabled = false;
        }
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    public void TakeDamage(int _amount , Vector3 hitpoint)
    {
        if (isDead)
            return;
        currentHealth -= _amount;

        health.SetText(currentHealth.ToString());

        DoGetHurtEffeft();
        photonView.RPC("DoBloodSplatter", RpcTarget.All , hitpoint);

        if (currentHealth <= 0)
        {
          photonView.RPC("Die",RpcTarget.All);
        }
    }

    [PunRPC]
    private void Die()
    {
        isDead = true;

        if (photonView.IsMine)
        {
            enemyScore += 0.5f;
            EnemyScore.SetText(enemyScore.ToString());
            Debug.Log("Into IsMine");
        }
        else
        {
            mineScore += 0.5f;
            MineScore.SetText(mineScore.ToString());
            Debug.Log(MineScore.text);
            Debug.Log("Into NotMine");
        }

        for (int i = 0; i < disableOnDeath_Obj.Length; i++)
        {
            disableOnDeath_Obj[i].SetActive(false);
        }
        disableOnDeath_MR.enabled = false;

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        //Die effect
        
        GameObject deadFX = Instantiate(deathtEffect, transform.position, Quaternion.identity);
        Destroy(deadFX, 3f);

        //Respawn
        StartCoroutine(Respawn());
    }
 
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(4);

        isDead = false;
        disableOnDeath_MR.enabled = true;
        currentHealth = maxHealth;
        health.SetText(currentHealth.ToString());

        for (int i = 0; i < disableOnDeath_Obj.Length; i++)
        {
            disableOnDeath_Obj[i].SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            SetRanges();
            transform.position = _randomPosition;
            transform.position = _randomPosition;
        }
        else
        {
            SetRanges();
            transform.position = _randomPosition;
            transform.position = _randomPosition;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }
    private void SetRanges()
    {
        _xAxis = Random.Range(Min.x, Max.x);
        _yAxis = Random.Range(Min.y, Max.y);
        _zAxis = Random.Range(Min.z, Max.z);
        _randomPosition = new Vector3(_xAxis, _yAxis, _zAxis);
    }
    private void DoGetHurtEffeft()
    {
        FindObjectOfType<AudioManager>().Play("getHit");
        StartCoroutine(DelayBloodScreen());
    }
    [PunRPC]
    private void DoBloodSplatter(Vector3 hitpoint)
    {
        GameObject hiteffect = Instantiate(BloodMuzzle , hitpoint, Quaternion.identity);
        Destroy(hiteffect, 0.3f);
    }
    private IEnumerator DelayBloodScreen()
    {
        BloodScrean.SetActive(true);
        yield return new WaitForSeconds(1);
        BloodScrean.SetActive(false);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.IsWriting)
        {
            // 為玩家本人的狀態, 將 IsFiring 的狀態更新給其他玩家
            stream.SendNext(enemyScore);
            stream.SendNext(mineScore);
        }
        else
        {
            // 非為玩家本人的狀態, 單純接收更新的資料
            enemyScore = (int)stream.ReceiveNext();
            mineScore = (int)stream.ReceiveNext();
        }
        */
    }

}
