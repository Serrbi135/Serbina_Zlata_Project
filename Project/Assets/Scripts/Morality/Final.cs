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
    }

}
