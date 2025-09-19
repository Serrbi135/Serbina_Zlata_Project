using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Final : MonoBehaviour
{
    private MoralitySystem moralitySystem;
    public TextMeshProUGUI status;
    public int points;
    void Start()
    {
        moralitySystem = FindObjectOfType<MoralitySystem>();
        points = moralitySystem.Points;
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
    }

}
