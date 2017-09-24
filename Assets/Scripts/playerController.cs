using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour{
    /*
     * Here it is all about animation of the rigid body 
     * It is linked with the animator
     * Make sure you adjust the transition and LOOP the animation
     * Also make sure network is ticked.
     * */
    public Animator anim;
    public AudioSource footStepSound;

    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetFloat("walk", 1);
            footStepSound.Play();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetFloat("walkBack", 1);
            footStepSound.Play();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetFloat("walk left", 1);
            footStepSound.Play();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetFloat("walk right", 1);
            footStepSound.Play();
        }
        else
        {
            anim.SetFloat("walk", 0);
            anim.SetFloat("walkBack", 0);
            anim.SetFloat("walk left", 0);
            anim.SetFloat("walk right", 0);
            footStepSound.Pause();
        }
    }

    public override void OnStartLocalPlayer()
    {
        Renderer ren = GetComponentInChildren<Renderer>();
        GetComponentInChildren<NetworkAnimator>().SetParameterAutoSend(0, true);
    }

    public override void PreStartClient()
    {
        GetComponentInChildren<NetworkAnimator>().SetParameterAutoSend(0, true);
    }
}
