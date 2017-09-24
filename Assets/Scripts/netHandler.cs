using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class netHandler : NetworkBehaviour {
    [SerializeField]
    Behaviour[] components;
    Camera sceneCam;
    string remotePlayer = "RemotePlayer";

    public GameObject PlayerUI;
    private GameObject PlayerUIInstanse;
	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            //Assigning a remote Player
            gameObject.layer = LayerMask.NameToLayer(remotePlayer);

            /* 
             * Removing compontents to avoid double distortion effects on all clients
             * Taking in the audio listener
             * the player movement and the Camera of the respective client
             * It is then turned off
             * */
            for(int i=0; i<components.Length; i++) 
            {
                components[i].enabled = false;
            }
        }
        else
        {
            sceneCam = Camera.main;
            if(sceneCam != null)
            {
                sceneCam.gameObject.SetActive(false);
            }
           
        }

        GetComponent<PlayerManager>().Setup();

        PlayerUIInstanse = Instantiate(PlayerUI);
        PlayerUIInstanse.name = PlayerUI.name;
	}

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netId = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager playerManager = GetComponent<PlayerManager>();

        NetManager.RegisterPlayer(_netId, playerManager);
    }


    private void OnDisable()
    {
        Destroy(PlayerUIInstanse);
        if (sceneCam != null)
        {
            sceneCam.gameObject.SetActive(true);
        }

        NetManager.RemoveRegisteredPlayer(transform.name);
    }


}
