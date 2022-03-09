using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    [SerializeField] GameObject car, startGameUI;
    [SerializeField] private string newGameLevel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            startGameUI.SetActive(false);

            if (PlayerPrefs.HasKey("LevelSaved"))
            {
                string levelToLoad = PlayerPrefs.GetString("LevelSaved");
                SceneManager.LoadScene(levelToLoad);

            } else SceneManager.LoadScene(newGameLevel);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted Every Player Pref");
        }
    }
}
