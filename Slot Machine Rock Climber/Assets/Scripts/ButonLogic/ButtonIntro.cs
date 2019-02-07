using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonIntro : MonoBehaviour {

    public void gameScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void awardScene()
    {
        SceneManager.LoadScene("AwardsScene"); 
    }

    public void introScene()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
