using UnityEngine.Networking;
using UnityEngine;

public class ShootingNetwork : NetworkBehaviour {

    
    private const string PLAYER_TAG = "Player";

    public AudioSource gunFire;
    public GameObject hitParticlePrefab;
    public WeaponDetail weapon;
    public Camera cam;
    public LayerMask mask;
	// Use this for initialization
	void Start () {
		if(cam == null)
        {
            Debug.LogError("There is no player");
            this.enabled = false;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (weapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Fire", 0f, weapon.fireRate);
                gunFire.Play();
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Fire");
            }
        }
    }

    [Client]
    void Fire()
    {
        RaycastHit r;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out r,weapon.range,mask))
        {
            if(r.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(r.transform.name,weapon.damage);
            }
        } else
        {
            Debug.Log("No hit");
        }

        CmdShootEffect(r.point, r.normal);
    }
  
    [Command]
    void CmdShootEffect(Vector3 position,Vector3 normal)
    {
        RpcDoShootEffect(position, normal);
    }

    [ClientRpc]
    void RpcDoShootEffect(Vector3 position,Vector3 normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(hitParticlePrefab, position, Quaternion.LookRotation(normal));
        Destroy(_hitEffect, 2f);
    }

    [Command]
    void CmdPlayerShot(string _ID,int damage)
    {
        Debug.Log(_ID + " has been shot");

        PlayerManager playermanager = NetManager.GetPlayer(_ID);
        playermanager.RpcDamage(damage);
    }
}
