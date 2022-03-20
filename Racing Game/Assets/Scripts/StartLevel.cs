using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    [SerializeField] private string newGameLevel;

    string levelToLoad;

    private void Start()
    {
        levelToLoad = PlayerPrefs.GetString("LevelSaved");

        if (PlayerPrefs.HasKey("LevelSaved"))
        {
            if(levelToLoad != "Level1")
            {
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
