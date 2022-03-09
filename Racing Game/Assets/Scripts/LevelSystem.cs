using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] Transform Player, EndLine;
    [SerializeField] GameObject finishLevel;
    [SerializeField] int numberOfLevels;
    bool hasFinished;

    private void Start()
    {
        
    }

    void Update()
    {
        if (hasFinished == true)
        {
            finishLevel.SetActive(true);
            SaveLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasFinished = true;
        }
    }

    float getDistance()
    {
        return Vector3.Distance(Player.position, EndLine.position);
    }

    public void SaveLevel()
    {
        int savedScene = SceneManager.GetActiveScene().buildIndex + 1;
        string activeScene = "Level" + savedScene.ToString();
        //string activeScene = SceneManager.GetActiveScene().name;
        if (savedScene <= numberOfLevels)
        {
            PlayerPrefs.SetString("LevelSaved", activeScene);
            Debug.Log(activeScene);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
