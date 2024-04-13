using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStopControl : MonoBehaviour
{
    public int Puan;
    public bool PuanAlindi;
    public bool TopAtildi = false;
    public IEnumerator WaitAndPrint()
    {
        LeanTween.delayedCall(gameObject, 1f, () => {
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude != 0)
            {
                TopAtildi = true;
            }
            else
            {
                TopAtildi = false;
            }
        });/*bberkakdemir*/
        yield return TopAtildi;
    }
}
