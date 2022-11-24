using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

    public static LevelUI Instance { get; private set; }

    public Text[] playerText;
    public Text LevelTimer;
    public Text AnnouncerTextLine;
    public Slider[] playerHealthbar;

    CharacterManager charM;

    
    // Start is called before the first frame update
    void Start()
    {
        if(playerText == null)
        {
            playerText = new Text[charM.plrs.Count]; 
        }
        if(AnnouncerTextLine == null)
        {
            AnnouncerTextLine = GameObject.FindWithTag("AnnouncerTextLine1").GetComponent<Text>();
        }
        if (playerHealthbar == null)
        {
            playerHealthbar = new Slider[charM.plrs.Count];
        }
        if (LevelTimer == null)
        {
            LevelTimer = GameObject.FindWithTag("TimeText").GetComponent<Text>();
        }

        for (int i = 0; i < charM.plrs.Count; i++)
        {
            playerText[i] = GameObject.FindWithTag("Player" + (i+1) + "Text").GetComponent<Text>();
            playerHealthbar[i] = GameObject.FindWithTag("Player" + (i + 1) + "Healthbar").GetComponent<Slider>();
        }

    }

    public void AddWinIndicator(int player)
    {

    }

    void Awake()
    {
        Instance = this;
    }
}
