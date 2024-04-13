using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Click : MonoBehaviour
{
    public Vector3 MouseFirstPosition;
    public Vector3 MouseMovePosition;
    public Vector3 MouseLastPosition;

    //public List<GameObject> ActiveBalls;
    public int WhoIsNext;
    public bool MovementContinues = false;

    //public GameObject SuccesPanel;
    //public GameObject FailPanel;
    //public GameObject DrawPanel;
    //public GameObject RestartButton;
    //public GameObject ScoreOneTextObject;
    //public GameObject ScoreTwoTextObject;
    //TMP_Text ScoreOneText;
    //TMP_Text ScoreTwoText;

    BallSlingShot ballSlingShot;
    BallThrowPlayerTwo ballThrowPlayerTwo;
    ClickControl clickControl;
    GameManager gameManager;

    //int ScoreOne;
    //int ScoreTwo;

    private void Start()
    {
        ballSlingShot = GameObject.FindWithTag("SlingPoint1").GetComponent<BallSlingShot>();
        ballThrowPlayerTwo = GameObject.FindWithTag("SlingPoint2").GetComponent<BallThrowPlayerTwo>();
        clickControl = GameObject.FindWithTag("Ground").GetComponent<ClickControl>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        //ScoreOneText = ScoreOneTextObject.GetComponent<TMP_Text>();
        //ScoreTwoText = ScoreTwoTextObject.GetComponent<TMP_Text>();
        ballSlingShot.TopOlustur(clickControl.Ball, clickControl.LivePoint);
        clickControl.TopOlustu = true;
        gameManager.SuccesPanel.SetActive(false);
        gameManager.DrawPanel.SetActive(false);
        gameManager.FailPanel.SetActive(false);
    }
    private void Update()
    {
        if (gameManager.GameEnd == false && MovementContinues == true && gameManager.ActiveBalls.Any(x => x.GetComponent<Rigidbody>().velocity.magnitude != 0) == false && gameManager.ActiveBalls.Any(x => x.GetComponent<BallStopControl>().PuanAlindi == false) == false && gameManager.ActiveBalls.Any(x => x.GetComponent<BallStopControl>().TopAtildi == false) == false)
        {
            if (MovementContinues == true && WhoIsNext == 0)
            {
                WhoIsNext = 1;
                gameManager.IsikPanel1.SetActive(false);
                gameManager.IsikPanel2.SetActive(true);
                //ballThrowPlayerTwo.lnRenderer.SetPosition(0, new Vector3(ballThrowPlayerTwo.LivePoint.transform.position.x, 1, ballThrowPlayerTwo.LivePoint.transform.position.z));
                //ballThrowPlayerTwo.lnRenderer.SetPosition(1, new Vector3(ballThrowPlayerTwo.LivePoint.transform.position.x, 1, ballThrowPlayerTwo.LivePoint.transform.position.z));
                ballThrowPlayerTwo.ArrowDrawCalculate();
                MovementContinues = false;
            }
            if (MovementContinues == true && WhoIsNext == 1 && gameManager.PlayerOneBallCount > 0)
            {
                WhoIsNext = 0;
                ballSlingShot.TopOlustur(clickControl.Ball, clickControl.LivePoint);
                clickControl.TopOlustu = true;
                gameManager.IsikPanel1.SetActive(true);
                gameManager.IsikPanel2.SetActive(false);
                MovementContinues = false;
            }
            else if (gameManager.PlayerTwoBallCount <= 0 && gameManager.PlayerOneBallCount <= 0)
            {
                gameManager.IsikPanel1.SetActive(true);
                gameManager.IsikPanel2.SetActive(true);
                if (gameManager.ScoreOne > gameManager.ScoreTwo)
                {
                    gameManager.GameEnd = true;
                    gameManager.LevelFinished();
                }
                else if (gameManager.ScoreTwo > gameManager.ScoreOne)
                {
                    gameManager.GameEnd = true;
                    gameManager.LevelField();
                }
                else if (gameManager.ScoreOne == gameManager.ScoreTwo)
                {
                    gameManager.GameEnd = true;
                    gameManager.LevelDraw();
                }
            }
        }
        foreach (var item in gameManager.ActiveBalls)
        {
            if (item.GetComponent<Rigidbody>().velocity.magnitude == 0 && item.GetComponent<BallStopControl>().PuanAlindi == false && item.GetComponent<BallStopControl>().TopAtildi == true)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(item.transform.position, transform.TransformDirection(Vector3.up), out raycastHit, 1000))
                {
                    Debug.DrawRay(item.transform.position, transform.TransformDirection(Vector3.up) * raycastHit.distance, Color.red);
                    item.GetComponent<BallStopControl>().Puan = raycastHit.collider.gameObject.GetComponent<ScoreAdd>().Score;
                    Debug.Log("Puan Alýndý = " + item.name + ", " + item.GetComponent<BallStopControl>().Puan);
                    item.GetComponent<BallStopControl>().PuanAlindi = true;

                    gameManager.ScoreOne = 0;
                    gameManager.ScoreTwo = 0;
                    foreach (var Ball in gameManager.ActiveBalls)
                    {
                        if (Ball.tag == "BallOne")
                        {
                            gameManager.ScoreOne += Ball.GetComponent<BallStopControl>().Puan;
                        }
                        if (Ball.tag == "BallTwo")
                        {
                            gameManager.ScoreTwo += Ball.GetComponent<BallStopControl>().Puan;
                        }
                    }
                    gameManager.ScoreOneText.text = gameManager.ScoreOne.ToString();
                    gameManager.ScoreTwoText.text = gameManager.ScoreTwo.ToString();
                }
            }
            if (item.GetComponent<Rigidbody>().velocity.magnitude != 0)
            {
                item.GetComponent<BallStopControl>().PuanAlindi = false;
            }
        }
    }
    public void Hesapla(float ShootPower, Vector3 Direction)
    {
        if (ShootPower >= 0.9f)
        {
            MovementContinues = true;
            gameManager.IsikPanel1.SetActive(false);
            gameManager.IsikPanel2.SetActive(false);
            ballSlingShot.ThrowPlayerOne(ShootPower, Direction);
            StartCoroutine(ballSlingShot.newBall.GetComponent<BallStopControl>().WaitAndPrint());
            gameManager.ActiveBalls.Add(ballSlingShot.newBall);
            gameManager.PlayerOneBallCount -= 1;
            Color color = Color.white;
            color.a = 0.70f;
            gameManager.BlueBallCountPanels[(gameManager.BlueBallCountPanels.Count - gameManager.PlayerOneBallCount) - 1].GetComponent<Image>().color = color;
            clickControl.TopOlustu = false;
        }
        else
        {
            ballSlingShot.newBall.transform.position = new Vector3(clickControl.LivePoint.transform.position.x, ballSlingShot.newBall.transform.position.y, clickControl.LivePoint.transform.position.z);
            MovementContinues = false;
            clickControl.TopOlustu = true;
        }
    }
}
