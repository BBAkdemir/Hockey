using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSlingShot : MonoBehaviour
{
    public GameObject newBall;
    public void TopOlustur(GameObject Ball, GameObject LivePoint)
    {
        newBall = Instantiate(Ball, new Vector3(LivePoint.transform.position.x, LivePoint.transform.position.y, LivePoint.transform.position.z), new Quaternion(0, 0, 0, 0));

    }
    public void ThrowPlayerOne(float ShootPower, Vector3 direction)
    {
        if (ShootPower >= 0.9f)
        {
            newBall.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x * ShootPower * 4.75f, 0, direction.z * ShootPower * 4.75f), ForceMode.Impulse);
        }
    }
    public void ThrowPlayerTwo(/*float randX, float randZ*/float ShootPower, Vector3 direction)
    {
        //newBall.GetComponent<Rigidbody>().AddForce(new Vector3(randX, 0, randZ));
        newBall.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x * ShootPower * 4.75f, 0, direction.z * ShootPower * 4.75f), ForceMode.Impulse);
    }
}
