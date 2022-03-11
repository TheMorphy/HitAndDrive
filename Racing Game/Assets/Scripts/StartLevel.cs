using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    [SerializeField] GameObject car;
    [SerializeField] private string newGameLevel;

    CarController carScript;
    LevelSystem levelSystem;

    string levelToLoad;

    private void Start()
    {
        carScript = car.GetComponent<CarController>();
        levelSystem = GetComponent<LevelSystem>();
        levelToLoad = PlayerPrefs.GetString("LevelSaved");

        if (PlayerPrefs.HasKey("LevelSaved"))
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
