using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int PlayerOneBallCount;
    public int PlayerTwoBallCount;

    public GameObject RestartButton;
    public GameObject SuccesPanel;
    public GameObject FailPanel;
    public GameObject DrawPanel;
    public TextMeshProUGUI LevelText;
    public GameObject LevelFinishedConfetti;
    public GameObject IsikPanel1;
    public GameObject IsikPanel2;
    public GameObject ScoreOneTextObject;
    public GameObject ScoreTwoTextObject;
    public GameObject TutorialHand;
    public TMP_Text ScoreOneText;
    public TMP_Text ScoreTwoText;
    public int ScoreOne;
    public int ScoreTwo;
    public List<GameObject> BlueBallCountPanels;
    public List<GameObject> RedBallCountPanels;
    public List<GameObject> ActiveBalls;



    public bool GameEnd = false;

    public List<GameObject> Levels;
    public int ActiveLevel;

    Color colorBlue;
    Color colorRed;

    //Click click;
    private void Awake()
    {
        colorBlue = Color.blue;
        colorBlue.r = 0.2028302f;
        colorBlue.g = 0.6265889f;
        colorRed = Color.red;
        colorRed.r = 0.754717f;
        colorRed.g = 0.01779993f;
        colorRed.b = 0.01779993f;
        ActiveBalls = new List<GameObject>();
    }
    private void Start()
    {
        //PlayerPrefs.SetInt("LastLevel", 0);
        ScoreOneText = ScoreOneTextObject.GetComponent<TMP_Text>();
        ScoreTwoText = ScoreTwoTextObject.GetComponent<TMP_Text>();
        //click = GameObject.FindWithTag("Game").GetComponent<Click>();
        //click = Levels[ActiveLevel].transform.GetChild(0).GetComponent<Click>();
        ActiveLevel = PlayerPrefs.GetInt("LastLevel", 0);

        foreach (var item in Levels)
        {
            if (item != Levels[ActiveLevel%4])
            {
                item.SetActive(false);
            }
        }
        Levels[ActiveLevel%4].SetActive(true);
        LevelText.text = "Level " + (ActiveLevel + 1).ToString();
    }
    public void RestartLevel()
    {
        LevelText.text = "Level " + (ActiveLevel + 1).ToString();
        RestartButton.SetActive(true);
        FailPanel.SetActive(false);
        GameEnd = false;
        SceneManager.LoadScene("Oyun");
    }
    public void NextLevel()
    {

       // LevelText.text = "Level " + (ActiveLevel + 1).ToString();
        //click = Levels[ActiveLevel].transform.GetChild(0).GetComponent<Click>();
        GameEnd = false;

        foreach (var item in ActiveBalls)
        {
            Destroy(item);
        }
        ActiveBalls.Clear();
        Levels[ActiveLevel%4].SetActive(false);
        ActiveLevel ++;
        PlayerPrefs.SetInt("LastLevel", ActiveLevel);

        LevelText.text = "Level " + (ActiveLevel + 1).ToString();
        /*
        if (Levels[ActiveLevel] == Levels[Levels.Count - 1])
        {
            ActiveLevel = 0;
        }
        else
        {
            ActiveLevel += 1;
        }*/
        ScoreOne = 0;
        ScoreTwo = 0;
        ScoreOneText.text = 0.ToString();
        ScoreTwoText.text = 0.ToString();
        PlayerOneBallCount = 5;
        PlayerTwoBallCount = 5;
        foreach (var item in BlueBallCountPanels)
        {
            item.GetComponent<Image>().color = colorBlue;
        }
        foreach (var item in RedBallCountPanels)
        {
            item.GetComponent<Image>().color = colorRed;
        }

        Levels[ActiveLevel%4].SetActive(true);
        TutorialHand.SetActive(true);
        IsikPanel1.SetActive(true);
        IsikPanel2.SetActive(false);
        RestartButton.SetActive(true);
        SuccesPanel.SetActive(false);
        DrawPanel.SetActive(false);

        //SceneManager.LoadScene("Oyun");
    }

    public void LevelFinished()
    {
        LevelFinishedConfetti.SetActive(true);
        LeanTween.delayedCall(gameObject, 2f, () => {
            SuccesPanel.SetActive(true);
            RestartButton.SetActive(false);
        });
    }
    public void LevelDraw()
    {
        LeanTween.delayedCall(gameObject, 2f, () => {
            DrawPanel.SetActive(true);
            RestartButton.SetActive(false);
        });
    }
    public void LevelField()
    {
        LeanTween.delayedCall(gameObject, 2f, () => {
            FailPanel.SetActive(true);
            RestartButton.SetActive(false);
        });
    }
}
