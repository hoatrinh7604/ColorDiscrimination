using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int score;
    [SerializeField] int highscore;
    public Color[] template = { new Color32(255, 81, 81, 255), new Color32(255, 129, 82, 255), new Color32(255, 233, 82, 255), new Color32(163, 255, 82, 255), new Color32(82, 207, 255, 255), new Color32(170, 82, 255, 255) };

    private UIController uiController;

    private float time;
    [SerializeField] float timeOfGame;

    [SerializeField] NumberContentController numberContentController;
    [SerializeField] ContentController contentController;

    [SerializeField] List<Color> currentArr;
    [SerializeField] int currentUserValue;
    [SerializeField] int leng;

    [SerializeField] int theFirstNumber;
    [SerializeField] int theSecondNumber;
    [SerializeField] int theResultNumber;

    private int currentMath;
    private int rightIndex;

    enum math
    {
        Summation = 0,
        Subtraction = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        Reset();
    }

    float timeSystem = 0;
    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeSystem += Time.deltaTime;
        UpdateSlider();

        if(time < 0)
        {
            GameOver();
        }

        if(timeSystem > 10)
        {
            timeSystem = 0;
            timeOfGame -= 1;
            if (timeOfGame < 3) timeOfGame = 3;
        }
    }

    public void UpdateSlider()
    {
        uiController.UpdateSlider(time);
    }

    public void SetSlider()
    {
        uiController.SetSlider(timeOfGame);
    }

    public void OnPressHandle(int index)
    {
        if (rightIndex == index)
        {
            SoundController.Instance.PlayAudio(SoundController.Instance.firing, 0.4f, false);
            UpdateScore();
            StartCoroutine(StartNextTurn());
        }
        else
        {
            GameOver();
        }
    }

    private void UpdateInfo(string value)
    {
        //numberContentController.UpdateInfo(currentUserValue, value);
        currentUserValue++;
        if (currentUserValue >= leng)
        {
            UpdateScore();
            StartCoroutine(StartNextTurn());
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiController.GameOver();
    }

    public void UpdateScore()
    {
        score++;
        if(highscore <= score)
        {
            highscore = score;
            PlayerPrefs.SetInt("score", highscore);
            uiController.UpdateHighScore(highscore);
        }
        uiController.UpdateScore(score);
    }

    IEnumerator StartNextTurn()
    {
        yield return new WaitForSeconds(0.5f);
        NextTurn();
    }

    List<int> listWrong;
    private Color MakeAWrongAnswer(Color col)
    {
        var A = col.a;

        float targetA = A;
        int random = 1;
        if (random == 1)
        {
            if (targetA < 0.8f)
            {
                targetA += Random.Range(0.1f, 0.2f);
            }
            else
            {
                targetA -= Random.Range(0.1f, 0.3f);
            }
        }
    
        return new Color(col.r, col.g, col.b, targetA);
    }

    bool CheckIndexNotInList(int index)
    {
        for (int i = 0; i < listWrong.Count; i++)
        {
            if (listWrong[i] == index)
            {
                return false;
            }
        }

        return true;
    }

    private Color MakeARightAnswer(Color col)
    {
        return col;
    }

    [SerializeField] List<string> listCountryIndex;
    public void NextTurn()
    {
        // Get random math
        rightIndex = Random.Range(0, 4);

        SetSlider();
        currentUserValue = 0;

        float randomRColor = Random.Range(0, 1f);
        float randomGColor = Random.Range(0, 1f);
        float randomBColor = Random.Range(0, 1f);
        float randomAColor = Random.Range(0.3f, 1f);

        currentArr = new List<Color>();
        leng = 4;
        var currentColor = new Color(randomRColor, randomGColor, randomBColor, randomAColor);
        numberContentController.Spaw(currentColor);

        //listWrong = new List<int>();
        //listWrong.Add(randomCountryIndex);
        for (int i = 0; i < leng; i++)
        {
            if(rightIndex == i)
            {
                currentArr.Add(MakeARightAnswer(currentColor));
            }
            else
                currentArr.Add(MakeAWrongAnswer(currentColor));
        }

        contentController.UpdateInfo(currentArr);

        time = timeOfGame;
    }

    public void Reset()
    {
        SoundController.Instance.PlayAudio(SoundController.Instance.bg, 0.3f, true);
        Time.timeScale = 1;

        time = timeOfGame;
        SetSlider();
        score = 0;
        highscore = PlayerPrefs.GetInt("score");
        uiController.UpdateScore(score);
        uiController.UpdateHighScore(highscore);

        NextTurn();
    }

}
