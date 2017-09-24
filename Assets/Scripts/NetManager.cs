using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour {

    public static NetManager Instances;
    public MatchFixing match; 
    private const string PlayerId = "Player ";

    private static Dictionary<string, PlayerManager> dictionary = new Dictionary<string, PlayerManager>();

    private void Awake()
    {
        if(Instances != null)
        {
            Debug.LogError("More than one player");
        }
        else
        {
            Instances = this;
        }
    }

    public static void RegisterPlayer(string playerId, PlayerManager playerManager)
    {
        string _playerId = PlayerId + playerId;
        dictionary.Add(_playerId, playerManager);
        playerManager.transform.name = _playerId;
    }

    public static void RemoveRegisteredPlayer(string playerId)
    {
        dictionary.Remove(playerId);
    }

    public static PlayerManager GetPlayer(string _playerId)
    {
        return dictionary[_playerId];
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(100, 100, 200, 400));
    //    GUILayout.BeginVertical();

    //    foreach(string _playerId in dictionary.Keys)
    //    {
    //        GUILayout.Label(_playerId + "  =  " + dictionary[_playerId].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

}
