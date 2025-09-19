using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoralitySystem : MonoBehaviour
{
    public static MoralitySystem Instance;
    public int Points { get; private set; }


    public event Action OnMoralityChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void AddPoints(int amount)
    {
        if( Points + amount <= 0)
        {
            Points = 0;
        }
        else
        {
            Points += amount;
        }
        OnMoralityChanged?.Invoke();
    }

    public void SetPoints(int newPoints)
    {
        Points = newPoints;

        OnMoralityChanged?.Invoke();
    }
}
