using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{
    //[SerializeField] Transform Player, target;
    [SerializeField] GameObject finishLevel, levelUI, car, cam;
    [SerializeField] Text levelNumber;
    [SerializeField] int numberOfLevels;
    //[SerializeField] float speed = 1.0f;

    #region private
    bool hasFinished;
    int levelNumb, savedScene;
    string activeScene;
    CarController carScript;
    Animator camAnim;
    #endregion

    #region Encapsulated
    public int LevelNumb { get => levelNumb; set => levelNumb = value; }
    public int SavedScene { get => savedScene; set => savedScene = value; }
    public string ActiveScene { get => activeScene; set => activeScene = value; }
    #endregion

    private void Start()
    {
        carScript = car.GetComponent<CarController>();
        camAnim = cam.GetComponent<Animator>();

        LevelNumb = SceneManager.GetActiveScene().buildIndex;
        levelNumber.text = "Level " + LevelNumb.ToString();
    }

    void Update()
    {
        if (hasFinished == true)
        {
            finishLevel.SetActive(true);
            levelUI.SetActive(false);
            SaveLevel();
        }
    }

    #region collisions
    private void OnTriggerEnter(Collider other)
    {
        //Vector3 newCarPosition = car.transform.position;

        if (other.CompareTag("Player"))
        {
            carScript.startSpeed *= 5;
            //transform.position = Vector3.MoveTowards(Player.position, target.position, speed * Time.deltaTime);
            carScript.sphereRB.constraints = RigidbodyConstraints.FreezePositionX;
        }
    }
    #endregion

    #region save and load level
    public void SaveLevel()
    {
        SavedScene = SceneManager.GetActiveScene().buildIndex + 1;
        ActiveScene = "Level" + SavedScene.ToString();
        if (SavedScene <= numberOfLevels)
        {
            PlayerPrefs.SetString("LevelSaved", ActiveScene);
            Debug.Log(ActiveScene);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion
}
