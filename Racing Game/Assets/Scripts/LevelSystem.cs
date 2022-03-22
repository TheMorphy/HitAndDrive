using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    #region public
    [SerializeField] GameObject finishLevel, levelUI, car, cam, moneyUI, moneyUIFinal, trackManager, startGameUI, motor, steeringWheel, touchMovement,leftPosition, rightPosition;
    [SerializeField] TextMeshProUGUI levelNumber; 
    [SerializeField] int numberOfLevels;
    #endregion

    #region private
    bool hasFinished, isMiddle;
    int levelNumb, savedScene, money, moneyInOneRound;
    public float mp;
    string activeScene;
    CarController carScript;
    TrackManager tmScript;
    DriverFly driverFly;
    TextMeshProUGUI moneyNumber;
    Text moneyNumberFinal;
    private IEnumerator waitToFinish;
    Animator camAnim;
    #endregion

    #region Encapsulated
    public int LevelNumb { get => levelNumb; set => levelNumb = value; }
    public int SavedScene { get => savedScene; set => savedScene = value; }
    public string ActiveScene { get => activeScene; set => activeScene = value; }
    public int Money { get => money; set => money = value; }
    public int MoneyInOneRound { get => moneyInOneRound; set => moneyInOneRound = value; }
    public float MultiplierL { get => mp; set => mp = value; }
    public bool HasFinished { get => hasFinished; set => hasFinished = value; }
    #endregion

    private void Start()
    {
        waitToFinish = WaitAndPrint(1.0f);

        carScript = FindObjectOfType<CarController>();
        moneyNumber = moneyUI.GetComponent<TextMeshProUGUI>();
        moneyNumberFinal = moneyUIFinal.GetComponent<Text>();
        tmScript = trackManager.GetComponent<TrackManager>();
        camAnim = cam.GetComponent<Animator>();
        driverFly = FindObjectOfType<DriverFly>();

        moneyInOneRound = 0;

        money = PlayerPrefs.GetInt("Money");
        if (levelNumber != null)
        {
            LevelNumb = SceneManager.GetActiveScene().buildIndex;

            levelNumber.text = "Level " + (LevelNumb + 1).ToString();

            UpdateMoney();
        }


        if (carScript.IsUsingSteeringWheel) steeringWheel.SetActive(true);
        if (carScript.IsUsingTouchControl) touchMovement.SetActive(true);
    }

    void Update()
    {
        #region Start Game and Delete Prefs
        if (Input.GetKeyDown(KeyCode.X))
        {
            startGameUI.SetActive(false);
            if (carScript.IsUsingSteeringWheel) steeringWheel.SetActive(true);
            if (carScript.IsUsingTouchControl) touchMovement.SetActive(true);
            carScript.startSpeed = 27;
            levelNumber.enabled = true;
        }

        if (Input.touchCount > 0)
        {
            startGameUI.SetActive(false);
            carScript.startSpeed = 27;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted Every Player Pref");
        }
        #endregion

        #region Car Goes To The Middle
        if (hasFinished == true)
        {
            if (isMiddle)
            {
                carScript.TurnSpeed = 0;
            }
        }
        #endregion

        if (HasFinished == true && tmScript.currentlevel < 5 && driverFly.IsBlocked == true)
        {
            StartCoroutine(waitToFinish);
        }
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        if(hasFinished == true)
        {
            hasFinished = false;
            levelUI.SetActive(false);
            yield return new WaitForSeconds(waitTime);
            finishLevel.SetActive(true);
            moneyInOneRound = Mathf.RoundToInt(mp * moneyInOneRound);
            moneyNumberFinal.text = "+ " + moneyInOneRound.ToString();
            SaveLevel();
        }
    }

    #region collisions
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HasFinished = true;
                if(tmScript.currentlevel < 6)
                {
                    tmScript.currentlevel = 6;
                }
                carScript.startSpeed = ((0 + tmScript.currentlevel) * 2) + 30;
                steeringWheel.SetActive(false);
                camAnim.SetBool("final", true);
                camAnim.Play("CamFinalStageAnim");
            }
        }
    #endregion

    #region save and load level
    public void SaveLevel()
    {
        SavedScene = SceneManager.GetActiveScene().buildIndex + 1;
        money = money + moneyInOneRound;
        PlayerPrefs.SetInt("Money", money);
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

    public void UpdateMoney()
    {
        moneyNumber.text = (money + moneyInOneRound).ToString();
    }
}
