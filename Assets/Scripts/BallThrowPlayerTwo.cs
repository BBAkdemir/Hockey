using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BallThrowPlayerTwo : MonoBehaviour
{
    public GameObject LivePoint;
    public GameObject Ball;
    public GameObject NewOk;

    BallSlingShot ballSlingShot;
    GameManager gameManager;
    public LineRenderer lnRenderer;

    Click click;
    System.Random rnd;

    //public float randomX;
    //public float randomZ;
    Vector3 direction;

    //public int BallMinX;
    //public int BallMaxX;
    //public int BallMinZ;
    //public int BallMaxZ;
    //public bool taraf;

    float CurrentDistance;
    float BallDistance;
    float MaxDistance = 2.0f;
    float DistanceCircle;
    float ShootPower;
    float difference;
    Vector3 dimXY;

    float FirstClickRandomX, FirstClickRandomZ;
    float LastClickRandomX, LastClickRandomZ;

    public float FirstClickPositionMinX, FirstClickPositionMaxX, FirstClickPositionMinZ, FirstClickPositionMaxZ;
    public float LastClickPositionMinX, LastClickPositionMaxX, LastClickPositionMinZ, LastClickPositionMaxZ;
    private void Start()
    {
        click = GameObject.FindWithTag("Game").GetComponent<Click>();
        ballSlingShot = GameObject.FindWithTag("SlingPoint2").GetComponent<BallSlingShot>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        lnRenderer = NewOk.GetComponent<LineRenderer>();/*bberkakdemir*/
        rnd = new System.Random();
    }
    public void ArrowDrawCalculate()
    {
        lnRenderer.SetPosition(0, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
        lnRenderer.SetPosition(1, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
        ballSlingShot.TopOlustur(Ball, LivePoint);

        FirstClickRandomX = UnityEngine.Random.Range(FirstClickPositionMinX, FirstClickPositionMaxX);
        FirstClickRandomZ = UnityEngine.Random.Range(FirstClickPositionMinZ, FirstClickPositionMaxZ);
        LastClickRandomX = UnityEngine.Random.Range(LastClickPositionMinX, LastClickPositionMaxX);
        LastClickRandomZ = UnityEngine.Random.Range(LastClickPositionMinZ, LastClickPositionMaxZ);

        Vector3 FirstClickPosition = new Vector3(FirstClickRandomX, 0, FirstClickRandomZ);
        Vector3 LastClickPosition = new Vector3(LastClickRandomX, 0, LastClickRandomZ);

        CurrentDistance = Mathf.Clamp(Vector3.Distance(FirstClickPosition, LastClickPosition), -4, 4);
        if (CurrentDistance <= MaxDistance)
        {
            DistanceCircle = CurrentDistance;
        }
        else
        {
            DistanceCircle = MaxDistance;
        }

        ShootPower = Mathf.Abs(DistanceCircle);

        dimXY = (FirstClickPosition - LastClickPosition);
        difference = dimXY.magnitude;
        direction = (LivePoint.transform.position + ((dimXY / difference) * DistanceCircle)) - LivePoint.transform.position;
        lnRenderer.SetPosition(1, LivePoint.transform.position + ((dimXY / difference) * CurrentDistance));
        lnRenderer.SetPosition(1, new Vector3(lnRenderer.GetPosition(1).x, ballSlingShot.newBall.transform.position.y, lnRenderer.GetPosition(1).z));

        Vector3 BallNewPosition = LivePoint.transform.position + ((dimXY / difference) * BallDistance * -1);
        ballSlingShot.newBall.transform.position = new Vector3(BallNewPosition.x, ballSlingShot.newBall.transform.position.y, BallNewPosition.z);
        NewOk.SetActive(true);
        StartCoroutine(BallThrow());

        //lnRenderer.SetPosition(0, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
        //lnRenderer.SetPosition(1, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
        //ballSlingShot.TopOlustur(Ball, LivePoint);
        //randomX = rnd.Next(BallMinX, BallMaxX/*-60, 60*/);
        //randomZ = rnd.Next(BallMinZ, BallMaxZ/*160, 250*/);

        //Vector3 dimXY = new Vector3(randomX, 1, randomZ);
        //float Difference = dimXY.magnitude;
        //direction = LivePoint.transform.position + (dimXY / Difference) * -2;
        //direction = new Vector3(direction.x, 1, direction.z);
        //NewOk.SetActive(true);/*bberkakdemir*/
        //StartCoroutine(DrawArrow());
    }
    public IEnumerator BallThrow()
    {
        LeanTween.delayedCall(gameObject, 0.1f, () =>
        {
            ballSlingShot.ThrowPlayerTwo(ShootPower, direction);
            NewOk.SetActive(false);
            gameManager.ActiveBalls.Add(ballSlingShot.newBall);
            gameManager.PlayerTwoBallCount -= 1;
            Color color = Color.white;
            color.a = 0.70f;
            gameManager.RedBallCountPanels[(gameManager.RedBallCountPanels.Count - gameManager.PlayerTwoBallCount) - 1].GetComponent<Image>().color = color;
            StartCoroutine(ballSlingShot.newBall.GetComponent<BallStopControl>().WaitAndPrint());

            click.MovementContinues = true;
            gameManager.IsikPanel1.SetActive(false);
            gameManager.IsikPanel2.SetActive(false);
        });/*bberkakdemir*/
        yield return null;
    }

    public IEnumerator DrawArrow()
    {
        while (lnRenderer.GetPosition(1).x != (LivePoint.transform.position + ((dimXY / difference) * CurrentDistance)).x && lnRenderer.GetPosition(1).z != (LivePoint.transform.position + ((dimXY / difference) * CurrentDistance)).z)
        {
            lnRenderer.SetPosition(1, Vector3.MoveTowards(lnRenderer.GetPosition(1), LivePoint.transform.position + ((dimXY / difference) * CurrentDistance), 5* Time.deltaTime) );
            lnRenderer.SetPosition(1, new Vector3(lnRenderer.GetPosition(1).x, ballSlingShot.newBall.transform.position.y, lnRenderer.GetPosition(1).z));
            if (lnRenderer.GetPosition(1).x >= (LivePoint.transform.position + ((dimXY / difference) * CurrentDistance)).x && lnRenderer.GetPosition(1).z >= (LivePoint.transform.position + ((dimXY / difference) * CurrentDistance)).z)
            {
                StartCoroutine(BallThrow());
                StopCoroutine(DrawArrow());
            }
            //lnRenderer.SetPosition(1, Vector3.MoveTowards(lnRenderer.GetPosition(1), direction, 5 * Time.deltaTime));
            //if (lnRenderer.GetPosition(1).x >= direction.x && lnRenderer.GetPosition(1).z >= direction.z)
            //{
            //    StartCoroutine(BallThrow());
            //}/*bberkakdemir*/
            yield return null;
        }
    }
}
