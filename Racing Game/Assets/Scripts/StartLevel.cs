using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    [SerializeField] GameObject car, startGameUI;
    [SerializeField] private string newGameLevel;

    CarController carScript;
    LevelSystem levelSystem;

    string levelToLoad;

    private void Start()
    {
        carScript = car.GetComponent<CarController>();
        levelSystem = GetComponent<LevelSystem>();
        levelToLoad = PlayerPrefs.GetString("LevelSaved");

        if (PlayerPrefs.HasKey("LevelSaved") && levelToLoad == levelSystem.ActiveScene)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            startGameUI.SetActive(false);
            carScript.startSpeed = 27;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted Every Player Pref");
        }
    }
}
