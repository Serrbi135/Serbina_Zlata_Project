using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
    private MoralitySystem moralitySystem;

    public TextMeshProUGUI status;
    public TextMeshProUGUI statusDiary;
    public Button toTitres;

    public int points;
    public int openedFlags;

    private GameAPIManager apiManager;
    private int currentUserId;



    void Start()
    {
        moralitySystem = FindObjectOfType<MoralitySystem>();
        points = moralitySystem.Points;
        currentUserId = PlayerPrefs.GetInt("CurrentUserId");
        apiManager = FindObjectOfType<GameAPIManager>();
        toTitres.onClick.AddListener(ToTitres);

        LoadAndDisplaySaveStats();
        SetStatus();
        
    }

    public void SetStatus()
    {
        if(points >= 45 && points <= 55)
        {
            status.text = "�����������! �� ������� ��������� ����� � ��������� � ��� �����. ��� ���� �� �� ���������� ���������! (��������� ��������)";
        }
        else if(points > 55 && points < 90)
        {
            status.text = "�����������! �� ��������� ����� � ��������� � ���. ��� ���� �� �� ���������� ��������� � ������ ��������� ����� � �����! ";
        }
        else if (points >= 90)
        {
            status.text = "�����������! �� ���-��� ��������� ����� � ��������� � ��� �� �������. ���� �� ����� �������� � ���������� ����� �������� ������ � �����! ";
        }
        else if (points < 45 && points > 10 )
        {
            status.text = "�����������! �� ��������� ����� �� ������� � ��������� � ��� ����� �� ���. � ��� ���� ���� ���������� �������, �� �� ������ ������� ������! ";
        }
        else if (points <= 10)
        {
            status.text = "�����������! �� ��������� ����� �� ������� � ��������� � ��� �� ���������. �� ��������� �������, ���� ������ ������. ����� �� ������ ������-�������������! ";
        }
        
        if (openedFlags == 20)
        {
            statusDiary.text = "���, �� �� ���������� � ������ ��� ���������� � �����! ��������� ������� �� ������";
        }
        else
        {
            statusDiary.text = $"������� {openedFlags}/20 �������";
        }
    }

    void LoadAndDisplaySaveStats()
    {
        if (apiManager != null)
        {
            StartCoroutine(apiManager.LoadGame(currentUserId, (progress) =>
            {
                if (progress != null)
                {
                    openedFlags = CountUnlockedDiaryFlags(progress.diaryFlags);
                }
            }));
        }
        else
        {
            Debug.LogError("GameAPIManager not found!");
        }
    }

    private void ToTitres()
    {
        Loader.LoadScene("Titres");
    }

    private int CountUnlockedDiaryFlags(int[] diaryFlags)
    {
        if (diaryFlags == null) return 0;

        int count = 0;
        foreach (int flag in diaryFlags)
        {
            if (flag == 1) count++;
        }
        return count;
    }

}
