using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningEnemy : MonoBehaviour
{
    public float speed;
    public Transform[] moveSpots;
    private int numOfSpot;
    public int CountOfSpots;
    private bool movingToEndOfList = true;

    void Update()
    {
        Moving();
        MovingToEndOrNot();
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[numOfSpot].position, speed * Time.deltaTime);

    }

    void MovingToEndOrNot()
    {
        if (numOfSpot == (CountOfSpots - 1))
        {
            movingToEndOfList = false;
        }
        else if (numOfSpot == 0)
        {
            movingToEndOfList = true;
        }
    }

    void Moving()
    {
        if (transform.position == moveSpots[numOfSpot].transform.position && movingToEndOfList)
        {
            numOfSpot++;

        }
        else if (transform.position == moveSpots[numOfSpot].transform.position && !movingToEndOfList)
        {
            numOfSpot--;

        }
    }


}

