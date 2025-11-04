using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Панели")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Поля ввода логина")]
    public InputField usernameInput;
    public InputField passwordInput;

    [Header("Поля ввода регистрации")]
    public InputField regUsernameInput;
    public InputField regPasswordInput;

    [Header("Кнопки")]
    public Button loginButton;
    public Button goToRegisterButton;
    public Button registerButton;
    public Button backToLoginButton;
    public Button newGameButton;

    [Header("Текст ошибки")]
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
            errorText.text = "Заполните все поля!";
            return;
        }
        if (password.Length < 6 || password.Length > 30)
        {
            errorText.text = "Пароль должен быть не короче 6 и не больше 30 символов!";
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
            errorText.text = "Заполните все поля!";
            return;
        }
        if (password.Length < 6 || password.Length > 30)
        {
            errorText.text = "Пароль должен быть не короче 6 и не больше 30 символов!";
            return;
        }


        StartCoroutine(apiManager.Register(username, password, OnRegisterSuccess));
    }

    private void OnLoginSuccess(int userId)
    {
        errorText.text = "Успешный вход!";
        Loader.LoadScene("LoadScene");
    }

    private void OnRegisterSuccess()
    {
        errorText.text = "Регистрация успешна! Войдите в аккаунт.";
        regUsernameInput.text = "";
        regPasswordInput.text = "";
        ShowLoginPanel();
    }


    public void ShowError(string message)
    {
        errorText.text = message;
    }
}