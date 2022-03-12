using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    public Transform[] trackPoints;

    public float reloadPathDistance;

    public static TrackManager instance;

    public List<PositionInRace> racers;

    [SerializeField] Image killBar;
    [SerializeField] float levelLongness = 5;
    [SerializeField] float decreaseMultiplicator;
    public float killValue;
    public bool fever;
    float feverBurnTime;
    public float carCrashPower = 5;
    [Header("Level stuff")]
    [SerializeField] int startLevel = 0;
    [SerializeField] public int currentlevel;
    [SerializeField] float showLevelTime;
    float showaddedLevelCountdown;
    [SerializeField] GameObject carLevelText;
    [SerializeField] string prefix = "Lvl. ";
    [SerializeField] AnimationCurve textPopupCurve;
    [SerializeField] float curveMultiplier;
    Coroutine showupLevel;
    [SerializeField] Transform camera;
    Quaternion lvltxtwrldrot;
    int currentPlus;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] float textRefreshSpeed;
    Coroutine updateTxtCoroutine;
    [SerializeField] float carSizePlus = 0.1f;
    [SerializeField] float currentCarSize;
    [SerializeField] public Transform car;
    [SerializeField] GameObject standardCarModel;
    public List<CarChange> carChanges;
    List<GameObject> usedCarModels = new List<GameObject>();
    [SerializeField]
    Animator carCollider;
    Animator carAnim;
    string currentLevelupAnim = "LevelUp";
    [Header("Fever Mode")]
    [SerializeField]
    GameObject feverCar;
    [SerializeField]
    string transformationAnimation;
    bool feverTriggered;
    bool endFeverTriggered;
    [SerializeField]
    Color feverColor, nonFeverColor;
    Material nonFeverMat;

    [Header("Wall break")]
    [SerializeField]
    public float wallDestructionForce = 4;
    public float wallExplodeDelay = 0.3f;

    [Header("Losing")]
    public bool lost;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject gameOverScreen;



    List<TextMeshPro> tmps = new List<TextMeshPro>();
    Color c = new Color(1, 1, 1, 1);
    
    // Start is called before the first frame update
    void Awake()
    {
        endFeverTriggered = true;
        currentCarSize = car.localScale.y;
        lvltxtwrldrot = carLevelText.transform.rotation;
        currentlevel = startLevel;
        if(instance != null)
        {
            Debug.LogWarning("Too many Trackmanagers in Scene!");
            return;
        }
        instance = this;
        carAnim = car.GetComponent<Animator>();
        currentLevelupAnim = "LevelUp";
    }
    private void Start()
    {
        usedCarModels.Add(standardCarModel);

        lvlText.text = "Lv." + currentlevel.ToString();
    }
    public void changeLevel(int lvl, string prefix = "+", bool isRed = false)
    {
        //currentCarSize += carSizePlus;
        //car.localScale = Vector3.one * currentCarSize; 
        currentlevel += lvl;
        currentlevel = Mathf.Clamp(currentlevel, 0, int.MaxValue);
        if (!fever)
        {

            if (updateTxtCoroutine == null)
            {
                updateTxtCoroutine = StartCoroutine(updateLevelTxt());
            }
            else
            {
                StopCoroutine(updateTxtCoroutine);
                updateTxtCoroutine = StartCoroutine(updateLevelTxt());
            }

            currentPlus += lvl;
            showaddedLevelCountdown = showLevelTime;
            if (!fever)
            {
                TextMeshPro tmp = Instantiate(carLevelText, car).GetComponent<TextMeshPro>();
                tmp.text = prefix + lvl.ToString();
                tmp.sortingOrder = currentlevel;
                if(isRed)
                {
                    tmp.color = Color.red;

                }
                StartCoroutine(holdRotation(tmp.transform));
                Destroy(tmp, 5);
            }


            if (showupLevel == null)
            {

                showupLevel = StartCoroutine(ShowUpText());
            }
            bool carChanged = false;
            foreach (CarChange c in carChanges)
            {
                if (c.levelToReach <= currentlevel && c.changed == false)
                {
                    //changeCarModel(c.newCarModel);
                    c.changed = true;
                    carChanged = true;
                    carAnim.Play(c.animationName);
                    currentLevelupAnim = c.levelupAnim;
                    if (c.CoroutineName != "")
                    {
                        StartCoroutine(c.CoroutineName);
                    }
                    carAnim.SetFloat("Car", c.index);
                }
            }

            if (!carChanged)
            {
                carAnim.Play(currentLevelupAnim);

            }
        }
        else
            carAnim.Play("bigtrucklvlupanim");
        
        

    }

    public void RemoveLevel(int lvl)
    {
        currentlevel -= lvl;
        if (updateTxtCoroutine == null)
        {
            updateTxtCoroutine = StartCoroutine(updateLevelTxt());
        }
        else
        {
            StopCoroutine(updateTxtCoroutine);
            updateTxtCoroutine = StartCoroutine(updateLevelTxt());
        }
    }

    IEnumerator SmoothCamOutForTruck()
    {
        
        while(false == false)
        {
            yield return null;
            camera.GetComponent<CameraFollow>().offset.z = Mathf.LerpAngle(camera.GetComponent<CameraFollow>().offset.z, -0.75f, 0.05f);
            camera.GetComponent<CameraFollow>().offset.y = Mathf.LerpAngle(camera.GetComponent<CameraFollow>().offset.y, 0.45f, 0.05f);
            if (camera.GetComponent<CameraFollow>().offset.z == -0.75f)
            {
                yield break;
            }

        }

        
    }

    void changeCarModel(GameObject model)
    {
        foreach(GameObject g in usedCarModels)
        {
            g.SetActive(false);
        }
        model.SetActive(true);
        usedCarModels.Add(model);

    }
    IEnumerator updateLevelTxt()
    {
        Transform t = lvlText.transform;
        while(t.localScale.y > 0)
        {
            t.localScale -= Vector3.up * Time.deltaTime * textRefreshSpeed;
            yield return new WaitForEndOfFrame();
        }
        lvlText.text = "Lv." + currentlevel.ToString();

        while (t.localScale.y < 1)
        {
            t.localScale += Vector3.up * Time.deltaTime * textRefreshSpeed;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    IEnumerator ShowUpText()
    {
        bool stop = false;
        float time = 0;
        while(stop == false)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime * curveMultiplier;
            carLevelText.transform.localScale = Vector3.one * textPopupCurve.Evaluate(time);
            if(time >= 1)
            {
                carLevelText.transform.localScale = Vector3.one;
                stop = true;
                showaddedLevelCountdown = showLevelTime;
            }
        }
        while(showaddedLevelCountdown > 0)
        {
            showaddedLevelCountdown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        time = 1;
        while(stop)
        {
            time -= Time.deltaTime * curveMultiplier;
            carLevelText.transform.localScale = Vector3.one * time;
            yield return new WaitForEndOfFrame();

            if (time <= 0f)
            {
                carLevelText.transform.localScale = Vector3.zero;
                stop = false;
            }
        }
        currentPlus = 0;
        showupLevel = null;
        yield break;
    }
    IEnumerator holdRotation(Transform t)
    {
        while(t !=null)
        {
            t.rotation = lvltxtwrldrot;
            yield return new WaitForEndOfFrame();
        }
        yield break;
        
        
    }

    private void LateUpdate()
    {
        
    }

    CarChange getCurrentCarLevel()
    {
        CarChange current = null;
        int curlvl = 0;
        foreach(CarChange c in carChanges)
        {
            if(c.levelToReach < currentlevel && c.levelToReach > curlvl)
            {
                current = c;
            }
        }
        return current;
    }

    public void PlayerDie(Vector3 explosionPosition)
    {

        gameOverScreen.SetActive(true);
        AudioManager.instance.Play("Fail");
        Instantiate(explosionParticle, explosionPosition + new Vector3(0, 0.3f, 0), car.transform.rotation);
        Destroy(car.parent.gameObject);
        lost = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        if(lost)
        {
            return;
        }
        //print(fever);
        int posi = racers.Count;
        racers.Sort(SortByPos);
        foreach(PositionInRace p in racers)
        {
            //print(p.name);
            p.position = posi;
            posi--;
        }
        if(killValue >= levelLongness && !fever)
        {
            feverBurnTime = killValue;
            fever = true;
        }
        if(fever)
        {
            feverCar.transform.GetChild(0).GetComponent<MeshRenderer>().materials[2].SetColor("_Color", feverColor);
            if (!feverTriggered)
            {
               // nonFeverMat = feverCar.transform.GetChild(0).GetComponent<MeshRenderer>().materials[2];
                //feverCar.transform.GetChild(0).GetComponent<MeshRenderer>().materials[2] = feverMat;
                carAnim.Play(transformationAnimation, 0, 2);
                carAnim.SetFloat("Car", 2);
                feverTriggered = true;
            }
            endFeverTriggered = false;
            lvlText.transform.parent.gameObject.SetActive(false);
            car.parent.GetComponent<CarController>().speed = car.parent.GetComponent<CarController>().nitroSpeed;
            feverBurnTime -= Time.deltaTime * decreaseMultiplicator;
            fever = feverBurnTime > 0;
            killBar.fillAmount = feverBurnTime / levelLongness;
            killValue = feverBurnTime;
        }
        else
        {
            if(!endFeverTriggered)
            {
                feverCar.transform.GetChild(0).GetComponent<MeshRenderer>().materials[2].SetColor("_Color", nonFeverColor);
                changeLevel(0);
                
                CarChange backChange = getCurrentCarLevel();
                if (backChange.index != 2)
                {
                    carAnim.Play(backChange.animationName);
                    carAnim.Play("FeverToNormal");
                    currentLevelupAnim = backChange.levelupAnim;
                    if (backChange.CoroutineName != "")
                    {
                        StartCoroutine(backChange.CoroutineName);
                    }
                    carAnim.SetFloat("Car", backChange.index);
                }
                    
                endFeverTriggered = true;
                //feverCar.transform.GetChild(0).GetComponent<MeshRenderer>().materials[2] = nonFeverMat;
            }
            feverTriggered = false;
            lvlText.transform.parent.gameObject.SetActive(true);
            car.parent.GetComponent<CarController>().speed = car.parent.GetComponent<CarController>().startSpeed;
            killBar.fillAmount = killValue / levelLongness;
            //killValue -= Time.deltaTime * decreaseMultiplicator;
            killValue = Mathf.Clamp(killValue, 0, levelLongness);
        }
    }

    static int SortByPos(PositionInRace r1, PositionInRace r2)
    {
        if(r1.realLineIndex.CompareTo(r2.realLineIndex) == 0)
        {
            return r1.remainingDistance.CompareTo(r2.remainingDistance) * -1;
        }
        return r1.realLineIndex.CompareTo(r2.realLineIndex);
    }
}
