using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    private GameObject getO;
    private DontDestroy dd;


    private GameObject getP;
    private PlayerController play;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }
/*
    private void Start() {
        getO = GameObject.Find("MainSource");
        dd = getO.GetComponent<DontDestroy>();
        getP = GameObject.Find("player");
        play = getP.GetComponent<PlayerController>();
    }
*/
    void Paused()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
        Debug.Log("devrait s'arreter");
        GameObject.Find("player").GetComponent<PlayerController>().pause = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        GameObject.Find("player").GetComponent<PlayerController>().resumeMovement();
    }

    public void LoadMainMenu()
    {
        dd.RemoveFromDontDestroy();
        Resume();
        SceneManager.LoadScene("Menu");
    }


}
