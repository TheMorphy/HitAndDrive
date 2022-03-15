using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{

    #region public
    [SerializeField] GameObject finishLevel, levelUI, car, cam, moneyUI, moneyUIFinal, trackManager, startGameUI, motor, steeringWheel;
    [SerializeField] Text levelNumber; 
    [SerializeField] int numberOfLevels;
    #endregion

    #region private
    bool hasFinished, isMiddle;
    int levelNumb, savedScene, money, moneyInOneRound;
    public float multiplier;
    string activeScene;
    CarController carScript;
    TrackManager tmScript;
    Text moneyNumber, moneyNumberFinal;
    private IEnumerator waitToFinish;
    Animator camAnim;
    #endregion

    #region Encapsulated
    public int LevelNumb { get => levelNumb; set => levelNumb = value; }
    public int SavedScene { get => savedScene; set => savedScene = value; }
    public string ActiveScene { get => activeScene; set => activeScene = value; }
    public int Money { get => money; set => money = value; }
    public int MoneyInOneRound { get => moneyInOneRound; set => moneyInOneRound = value; }
    public float Multiplier { get => multiplier; set => multiplier = value; }
    public bool HasFinished { get => hasFinished; set => hasFinished = value; }
    #endregion

    private void Start()
    {
        waitToFinish = WaitAndPrint(1.0f);

        carScript = FindObjectOfType<CarController>();
        moneyNumber = moneyUI.GetComponent<Text>();
        moneyNumberFinal = moneyUIFinal.GetComponent<Text>();
        tmScript = trackManager.GetComponent<TrackManager>();
        camAnim = cam.GetComponent<Animator>();

        moneyInOneRound = 0;

        money = PlayerPrefs.GetInt("Money");
        
        LevelNumb = SceneManager.GetActiveScene().buildIndex;
        levelNumber.text = "Level " + (LevelNumb + 1).ToString();

        UpdateMoney();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            startGameUI.SetActive(false);
            if(carScript.IsUsingSteeringWheel) steeringWheel.SetActive(true);
            carScript.startSpeed = 27;
        }

        if (Input.touchCount > 0)
        {
            startGameUI.SetActive(false);
            if (carScript.IsUsingSteeringWheel) steeringWheel.SetActive(true);
            carScript.startSpeed = 27;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted Every Player Pref");
        }

        if (HasFinished == true && motor.transform.position.x > -2.0f && isMiddle == false)
        {
            carScript.ForcedRotation = -0.5f;
        }
        
        if (HasFinished == true && motor.transform.position.x < -1.6f && isMiddle == false)
        {
            carScript.ForcedRotation = 0.5f;
        }

        if (motor.transform.position.x < -1.6f && motor.transform.position.x > -2.0f)
        {
            isMiddle = true;
        }
        else isMiddle = false;

        if (hasFinished == true)
        {
            if (isMiddle)
            {
                carScript.TurnSpeed = 0;
            }
        }

        if (isMiddle)
        {
            carScript.ForcedRotation = 0;
        }

        if (HasFinished == true && tmScript.currentlevel <= 10)
        {
            finishLevel.SetActive(true);
            levelUI.SetActive(false);
            StartCoroutine(waitToFinish);
        }
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        if(hasFinished == true)
        {
            hasFinished = false;
            yield return new WaitForSeconds(waitTime);
            moneyInOneRound = Mathf.RoundToInt(multiplier * moneyInOneRound);
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
            carScript.startSpeed *= 2;
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
