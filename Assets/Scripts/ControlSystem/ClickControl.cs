using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickControl : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    Click click;
    BallSlingShot ballSlingShot;
    GameManager gameManager;

    public bool TopOlustu = false;
    GameObject ballYedek;

    public GameObject Ball;
    public GameObject LivePoint;

    public GameObject NewOk;
    LineRenderer lnRenderer;

    float CurrentDistance;
    float BallDistance;
    float MaxDistance = 2.0f;
    float DistanceCircle;
    float ShootPower;
    Vector3 dimXY;
    Vector3 direction;
    public void Start()
    {
        click = GameObject.FindWithTag("Game").GetComponent<Click>();
        ballSlingShot = GameObject.FindWithTag("SlingPoint1").GetComponent<BallSlingShot>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        lnRenderer = NewOk.GetComponent<LineRenderer>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameManager.TutorialHand.activeSelf == true)
        {
            gameManager.TutorialHand.SetActive(false);
        }
        click.MouseFirstPosition = eventData.pointerPressRaycast.worldPosition;
        if (gameManager.GameEnd == false && click.WhoIsNext == 0 && click.MovementContinues == false && TopOlustu == false)
        {
            ballSlingShot.TopOlustur(Ball, LivePoint);
            lnRenderer.SetPosition(0, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
            lnRenderer.SetPosition(1, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
            NewOk.SetActive(true);
            TopOlustu = true;
        }
        if (gameManager.GameEnd == false && click.WhoIsNext == 0 && click.MovementContinues == false && TopOlustu == true && ballSlingShot.newBall.GetComponent<BallStopControl>().TopAtildi == false)
        {
            lnRenderer.SetPosition(0, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
            lnRenderer.SetPosition(1, new Vector3(LivePoint.transform.position.x, 1, LivePoint.transform.position.z));
            NewOk.SetActive(true);
        }
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        click.MouseMovePosition = eventData.pointerCurrentRaycast.worldPosition;
        if (gameManager.GameEnd == false && eventData.eligibleForClick && click.MovementContinues == false && click.WhoIsNext == 0)
        {
            CurrentDistance = Mathf.Clamp(Vector3.Distance(click.MouseFirstPosition, eventData.pointerCurrentRaycast.worldPosition), -4, 4);
            BallDistance = Mathf.Clamp(Vector3.Distance(click.MouseFirstPosition, eventData.pointerCurrentRaycast.worldPosition), -0.6f, 0.6f);
            if (CurrentDistance <= MaxDistance)
            {
                DistanceCircle = CurrentDistance;
            }
            else
            {
                DistanceCircle = MaxDistance;
            }

            ShootPower = Mathf.Abs(DistanceCircle);

            dimXY = (click.MouseFirstPosition - eventData.pointerCurrentRaycast.worldPosition);
            float difference = dimXY.magnitude;
            direction = (LivePoint.transform.position + ((dimXY / difference) * DistanceCircle)) - LivePoint.transform.position;
            lnRenderer.SetPosition(1, LivePoint.transform.position + ((dimXY / difference) * CurrentDistance));
            lnRenderer.SetPosition(1, new Vector3(lnRenderer.GetPosition(1).x, ballSlingShot.newBall.transform.position.y, lnRenderer.GetPosition(1).z));

            Vector3 BallNewPosition = LivePoint.transform.position + ((dimXY / difference) * BallDistance * -1);
            ballSlingShot.newBall.transform.position = new Vector3(BallNewPosition.x, ballSlingShot.newBall.transform.position.y, BallNewPosition.z);

            //float OkMoveX = LivePoint.transform.position.x + Mathf.Clamp(click.MouseFirstPosition.x - eventData.pointerCurrentRaycast.worldPosition.x, -2, 2);
            //float OkMoveZ = LivePoint.transform.position.z + Mathf.Clamp(click.MouseFirstPosition.z - eventData.pointerCurrentRaycast.worldPosition.z, -2, 2);

            //float BallMoveX = LivePoint.transform.position.x - Mathf.Clamp(click.MouseFirstPosition.x - eventData.pointerCurrentRaycast.worldPosition.x, -0.3f, 0.3f);
            //float BallMoveZ = LivePoint.transform.position.z - Mathf.Clamp(click.MouseFirstPosition.z - eventData.pointerCurrentRaycast.worldPosition.z, -0.3f, 0.3f);

            //lnRenderer.SetPosition(1, new Vector3(OkMoveX, 1, OkMoveZ));
            //ballSlingShot.newBall.transform.position = new Vector3(BallMoveX, ballSlingShot.newBall.transform.position.y, BallMoveZ);
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        click.MouseLastPosition = eventData.pointerCurrentRaycast.worldPosition;
        if (gameManager.GameEnd == false && click.WhoIsNext == 0 && click.MovementContinues == false && TopOlustu == true)
        {
            click.Hesapla(ShootPower, direction);
            NewOk.SetActive(false);
        }
    }
}
