using Services.Database.Firebase;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Services.Auth
{
    public class AuthButtonsController : MonoBehaviour
    {
        [Header("Registration page")] 
        [SerializeField] private Button _signUp;
        [SerializeField] private Button _transitionToLoginPage;

        [Space] 
        [Header("Login page")] 
        [SerializeField] private Button _logIn;
        [SerializeField] private Button _transitionToRegistrationPage;

        [Space]
        [Header("Pages")]
        [SerializeField] private Canvas _registrationPage;
        [SerializeField] private Canvas _loginPage;

        [FormerlySerializedAs("_authenticationManager")]
        [Space]
        [SerializeField] private DatabaseManager databaseManager;

        private void ActiveLoginPage()
        {
            _loginPage.gameObject.SetActive(true);
            _registrationPage.gameObject.SetActive(false);
        }
        
        private void ActiveRegistrationPage()
        {
            _registrationPage.gameObject.SetActive(true);
            _loginPage.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _signUp.onClick.AddListener(databaseManager.RegistrationButton);
            _logIn.onClick.AddListener(databaseManager.LoginButton);

            _transitionToLoginPage.onClick.AddListener(ActiveLoginPage);
            _transitionToRegistrationPage.onClick.AddListener(ActiveRegistrationPage);
        }
        
        private void OnDisable()
        {
            _signUp.onClick.RemoveListener(databaseManager.RegistrationButton);
            _logIn.onClick.RemoveListener(databaseManager.LoginButton);

            _transitionToLoginPage.onClick.RemoveListener(ActiveLoginPage);
            _transitionToRegistrationPage.onClick.RemoveListener(ActiveRegistrationPage);
        }
    }
}