using UnityEngine;

[System.Serializable]
public class PlayerWeapon 
{
    public string name = "AK47";
    public float recoil;
    public float firerate = 0.3f;
    public float lightDuration = 0.02f;
    public float range = 100f;
    public float limit = 0.5f;
    public int damage = 20;
    public GameObject graphics;
}
