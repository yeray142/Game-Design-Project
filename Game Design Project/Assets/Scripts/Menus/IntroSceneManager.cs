using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    public GameObject startText;
    float timer;
    bool loadingLevel;
    bool changingChar;
    bool init;

    public int activeElement;
    public GameObject menuObj;
    public GameObject charactersObj;
    public ButtonRef[] menuOptions;
    public GameObject[] characterArrows;

    // Start is called before the first frame update
    void Start()
    {
        menuObj.SetActive(false);
        charactersObj.SetActive(false);

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
                charactersObj.SetActive(true);
            }
        }
        else
        {
            if (!loadingLevel && !changingChar) // if not loading level or changing char
            {
                menuOptions[activeElement].selected = true;

                // Change the selected element
                if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement != 3 && activeElement != 4)
                        if (activeElement > 0)
                            activeElement--;
                        else
                            activeElement = menuOptions.Length - 3;
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement != 3 && activeElement != 4)
                        if (activeElement < menuOptions.Length - 3)
                            activeElement++;
                        else
                            activeElement = 0;
                }
                else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement == 4)
                        activeElement = 0;
                    else
                        activeElement = 3;
                }
                else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement == 3)
                        activeElement = 0;
                    else
                        activeElement = 4;
                }

                // If press Space or Enter then go to the next menu or level
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
                {
                    // Based on our selection
                    switch (activeElement)
                    {
                        case 0:
                            // Load the level
                            loadingLevel = true;
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
                        case 3:
                            changingChar = true;
                            characterArrows[0].SetActive(true);
                            break;
                        case 4:
                            changingChar = true;
                            characterArrows[1].SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (changingChar)
            {
                // Code for changing character
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    changingChar = false;
                    characterArrows[activeElement - 3].SetActive(false);
                }

                // TODO: Change character when pressing right or left:
            }
        }
    }

    IEnumerator LoadLevel()
    {
        // CharacterManager.GetInstance().numberOfUsers = 2;
        // CharacterManager.GetInstance().players[1].playerType = PlayerBase.PlyaerType.user;

        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync("MainGame", LoadSceneMode.Single);
    }
}
