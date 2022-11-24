using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int nUsers;
    public List<PlayerBase> plrs = new List<PlayerBase>();

    // Information about characters (id, prefab)
    public List<CharacterBase> characterList = new List<CharacterBase>();

    // Find character from Id (string)
    public CharacterBase returnCharacterFromID(string id)
    {
        CharacterBase retVal = null;

        for (int i = 0; i < characterList.Count; i++)
            if (string.Equals(characterList[i].charId, id))
            {
                retVal = characterList[i];
                break;
            }

        return retVal;
    }

    // Find player from his character (StateManager)
    public PlayerBase returnPlayerFromStates(StateManager states)
    {
        PlayerBase retVal = null;

        for (int i = 0; i < plrs.Count; i++)
            if (plrs[i].playerStates == states)
            {
                retVal = plrs[i];
                break;
            }

        return retVal;
    }

    public static CharacterManager instance;
    public static CharacterManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}


[System.Serializable]
public class CharacterBase
{
    public string charId;
    public GameObject prefab;
}

[System.Serializable]
public class PlayerBase
{
    public string playerId;
    public int inputId;
    // public PlayerType playerType; // Not needed
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;

    /*
    public enum PlayerType
    {
        user,
        ai,
        simulation
    }
    */
}