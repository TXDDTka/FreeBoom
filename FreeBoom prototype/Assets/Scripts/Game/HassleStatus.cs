using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HassleStatus : MonoBehaviour
{
    public GameObject pressureController;
    public GameObject centerController;
    public void SetBalance(int playerForce, int enemyForce)
    {
        float precent = (float)playerForce / (playerForce + enemyForce);
        pressureController.transform.localScale = new Vector2(precent, 1);
        centerController.transform.localPosition = new Vector2(precent * 2.4f, 0);
    }
}
