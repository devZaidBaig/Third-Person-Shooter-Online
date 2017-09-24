using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour{
    
    /*
     * This part of the script handles
     * the respawn and take damage by the player
     * all through the network 
     * */

    //Create a bool variable to check if dead or not
    [SyncVar]
    private bool _Dead = false;
    public bool Dead
    {
        get { return _Dead; }
        protected set { _Dead = value; }
    }

    //Variable to store the max health of player
    public int maxHealth = 100;

    //SyncVar is set to say that this variable is going to change across the network
    [SyncVar]
    private int currentHealth;

    //To disable all the behaviours of the player which dies
    public Behaviour[] disable;
    bool[] wasEnable;

    public Slider healthSlider;

    public void Setup()
    {
        wasEnable = new bool[disable.Length];
        for (int i = 0; i < wasEnable.Length; i++)
        {
            wasEnable[i] = disable[i].enabled;
        }

        Dead = false;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        for (int i = 0; i < disable.Length; i++)
        {
           disable[i].enabled = wasEnable[i];
        }


        Collider collider = GetComponent<Collider>();
        if (collider!=null)
        {
            collider.enabled = true;
        }
    }

    [ClientRpc]
    public void RpcDamage(int _damage)
    {
        Debug.Log("RpcDamage has been called");
        currentHealth -= _damage;
        healthSlider.value = currentHealth;
        Debug.Log("The current health is " + currentHealth + " for " + transform.name);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    //This meathod is called when the player's heath is <0
    private void Die()
    {
        Dead = true;

        for (int i = 0; i < disable.Length; i++)
        {
            disable[i].enabled = false;
        }

        //Disable colliders of the player
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        Debug.Log(transform.name + " is Dead...");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        //GUILayout.BeginArea(new Rect(100, 100, 200, 400));
        //GUILayout.BeginVertical();
        //GUILayout.Label("You are Dead... Respawn in 5s...");

        //GUILayout.EndVertical();
        //GUILayout.EndArea();
        yield return new WaitForSeconds(5f);
        Dead = false;
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;

        for (int i = 0; i < disable.Length; i++)
        {
            disable[i].enabled = wasEnable[i];
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        Transform startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcDamage(100);
        }
    }
}
