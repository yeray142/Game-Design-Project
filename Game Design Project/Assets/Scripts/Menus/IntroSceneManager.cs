using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;

public class IntroSceneManager : MonoBehaviour
{
    public GameObject startText;
    float timer;
    bool loadingLevel;
    bool init;

    public int activeElement;
    public GameObject menuObj;
    public ButtonRef[] menuOptions;

    // Start is called before the first frame update
    void Start()
    {
        menuObj.SetActive(false);
        startText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            // it flickers the "Press Start" text
            timer += Time.deltaTime;
            if (timer > 0.6f)
            {
                timer = 0;
                startText.SetActive(!startText.activeInHierarchy);
            }

            // If press Space or Enter then go to the main menu
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
            {
                init = true;
                startText.SetActive(false);
                menuObj.SetActive(true);
            }
        }
        else
        {
            if (!loadingLevel) // if not loading level
            {
                menuOptions[activeElement].selected = true;

                // Change the selected element
                if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement > 0)
                        activeElement--;
                    else
                        activeElement = menuOptions.Length - 1;
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement < menuOptions.Length - 1)
                        activeElement++;
                    else
                        activeElement = 0;
                }

                // If press Space or Enter then go to the next menu or level
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
                {
                    loadingLevel = true;

                    // Based on our selection
                    switch (activeElement)
                    {
                        case 0:
                            // Load the level
                            Debug.Log("load");
                            StartCoroutine("LoadLevel");
                            menuOptions[activeElement].transform.localScale *= 1.2f;
                            break;
                        case 1:
                            // Open tutorials menu
                            break;
                        case 2:
                            // Open settings menu
                            break;
                    }
                }
            }
        }
    }

    IEnumerator LoadLevel()
    {
        // CharacterManager.GetInstance().numberOfUsers = 2;
        // CharacterManager.GetInstance().players[1].playerType = PlayerBase.PlyaerType.user;

        yield return new WaitForSeconds(0.6f);
        startText.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("MainGame", LoadSceneMode.Single);
    }
}
