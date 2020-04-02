/*
using UnityEngine;
using System.Collections;
using Mirror;

public class PlayerManagerPRE : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead;}
        protected set { _isDead = value; }
    }


    [SerializeField]
    private int maxHealth = 200;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private GameObject[] disableOnDeath_Obj;
    [SerializeField]
    private MeshRenderer disableOnDeath_MR;
    [SerializeField]
    private GameObject deathtEffect;
    private bool[] wasEnabled;


    private bool firstSetup = true;

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
        }
        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClient();
    }
    [ClientRpc]
    private void RpcSetupPlayerOnAllClient()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
       
        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        //Disable 
        
        for (int i = 0; i < disableOnDeath.Length ; i++)
        {
            disableOnDeath[i].enabled = false;
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

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }

        //Respawn
        StartCoroutine(Respawn());
    }


    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999999999);
        }

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.MatchSettings.respawnTime);

       
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();
       // SetDefaults();
    }



    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        disableOnDeath_MR.enabled = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < disableOnDeath_Obj.Length; i++)
        {
            disableOnDeath_Obj[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
      

    }
}
*/