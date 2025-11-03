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
            status.text = "Поздравляем! Вы успешно закончили лицей и поступили в ВУЗ мечты. При этом вы не заработали выгорания! (Секретная концовка)";
        }
        else if(points > 55 && points < 90)
        {
            status.text = "Поздравляем! Вы закончили лицей и поступили в ВУЗ. При этом вы не заработали выгорания и хорошо запомнили жизнь в лицее! ";
        }
        else if (points >= 90)
        {
            status.text = "Поздравляем! Вы кое-как закончили лицей и поступили в ВУЗ на платное. Зато вы много отдыхали и заработали много полезных связей в лицее! ";
        }
        else if (points < 45 && points > 10 )
        {
            status.text = "Поздравляем! Вы закончили лицей на отлично и поступили в ВУЗ мечты по ЕГЭ. У вас было мало свободного времени, но вы обрели хороших друзей! ";
        }
        else if (points <= 10)
        {
            status.text = "Поздравляем! Вы закончили лицей на отлично и поступили в ВУЗ по олимпиаде. Вы научились учиться, хотя слегка устали. Также вы обрели друзей-олимпиадников! ";
        }
        
        /*if (openedFlags == 20)
        {
            statusDiary.text = "Ого, да ты постарался и собрал всю информацию о лицее! Отдельное спасибо от автора";
        }
        else
        {
            statusDiary.text = "Открыто записей: " + openedFlags + "/20";
        }*/
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
                    statusDiary.text = "Открыто записей: " + openedFlags + "/20";
                }
                if (openedFlags == 20)
                {
                    statusDiary.text = "Ого, да ты постарался и собрал всю информацию о лицее! Отдельное спасибо от автора";
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
