using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

    public static LevelUI Instance { get; private set; }

    public TextMeshProUGUI[] playerText;
    public TextMeshProUGUI LevelTimer;
    public TextMeshProUGUI AnnouncerTextLine;
    public Slider[] playerHealthbar;

    public GameObject[] winIndicatorGrids;
    public GameObject winIndicator;

    CharacterManager charM;

    
    // Start is called before the first frame update
    void Start()
    {
        charM = CharacterManager.GetInstance();

        if (playerText == null)
        {
            playerText = new TextMeshProUGUI[charM.plrs.Count]; 
        }
        if(AnnouncerTextLine == null)
        {
            AnnouncerTextLine = GameObject.FindWithTag("AnnouncerTextLine1").GetComponent<TextMeshProUGUI>();
        }
        if (playerHealthbar == null)
        {
            playerHealthbar = new Slider[charM.plrs.Count];
        }
        if (LevelTimer == null)
        {
            LevelTimer = GameObject.FindWithTag("TimeText").GetComponent<TextMeshProUGUI>();
        }

        for (int i = 0; i < charM.plrs.Count; i++)
        {
            playerText[i] = GameObject.FindWithTag("Player" + (i+1) + "Text").GetComponent<TextMeshProUGUI>();
            playerHealthbar[i] = GameObject.FindWithTag("Player" + (i + 1) + "Healthbar").GetComponent<Slider>();
        }

    }

    public void AddWinIndicator(int player)
    {
        GameObject go = Instantiate(winIndicator, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(winIndicatorGrids[player].transform);
    }

    void Awake()
    {
        Instance = this;
    }
}
