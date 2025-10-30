using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("������")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("���� ����� ������")]
    public InputField usernameInput;
    public InputField passwordInput;

    [Header("���� ����� �����������")]
    public InputField regUsernameInput;
    public InputField regPasswordInput;

    [Header("������")]
    public Button loginButton;
    public Button goToRegisterButton;
    public Button registerButton;
    public Button backToLoginButton;
    public Button newGameButton;

    [Header("����� ������")]
    public TextMeshProUGUI errorText;

    private GameAPIManager apiManager;

    private void Start()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);

        goToRegisterButton.onClick.AddListener(ShowRegisterPanel);
        backToLoginButton.onClick.AddListener(ShowLoginPanel);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        //newGameButton.onClick.AddListener(OnNewGameButtonClicked);

        apiManager = gameObject.AddComponent<GameAPIManager>();
    }

    private void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        errorText.text = "";
    }

    private void ShowLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
        errorText.text = "";
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            errorText.text = "��������� ��� ����!";
            return;
        }
        if (password.Length < 6 || password.Length > 30)
        {
            errorText.text = "������ ������ ���� �� ������ 6 � �� ������ 30 ��������!";
            return;
        }

        StartCoroutine(apiManager.Login(username, password, OnLoginSuccess));
    }

    private void OnRegisterButtonClicked()
    {
        string username = regUsernameInput.text;
        string password = regPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            errorText.text = "��������� ��� ����!";
            return;
        }
        if (password.Length < 6 || password.Length > 30)
        {
            errorText.text = "������ ������ ���� �� ������ 6 � �� ������ 30 ��������!";
            return;
        }


        StartCoroutine(apiManager.Register(username, password, OnRegisterSuccess));
    }

    private void OnLoginSuccess(int userId)
    {
        errorText.text = "�������� ����!";
        Loader.LoadScene("LoadScene");
    }

    private void OnRegisterSuccess()
    {
        errorText.text = "����������� �������! ������� � �������.";
        regUsernameInput.text = "";
        regPasswordInput.text = "";
        ShowLoginPanel();
    }

    /*private void OnNewGameButtonClicked()
    {
        Loader.LoadScene("Sujet_1_A");

        PlayerPrefs.DeleteKey("CurrentProgress");
        if (GameProgress.Instance != null)
        {
            GameProgress.Instance.CurrentProgress = new PlayerProgress()
            {
                sceneIndex = 2, 
                moralityPoints = 0,
                diaryFlags = new int[20] 
            };
        }
    }*/

    public void ShowError(string message)
    {
        errorText.text = message;
    }
}