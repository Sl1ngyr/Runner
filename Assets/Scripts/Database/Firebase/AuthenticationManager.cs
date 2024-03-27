using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;

namespace Database.Firebase
{
    public class AuthenticationManager : MonoBehaviour
    {
        [Header("Firebase")] 
        [SerializeField] private DependencyStatus _dependencyStatus;
        private FirebaseAuth _auth;
        private FirebaseUser _user;

        [Space] 
        [Header("Login page fields")] 
        [SerializeField] private TMP_InputField _emailLoginField;
        [SerializeField] private TMP_InputField _passwordLoginField;
        [SerializeField] private TMP_Text _warningLoginText;
        
        [Space] 
        [Header("Registration page fields")] 
        [SerializeField] private TMP_InputField _nameRegisterField;
        [SerializeField] private TMP_InputField _emailRegisterField;
        [SerializeField] private TMP_InputField _passwordRegisterField;
        [SerializeField] private TMP_InputField _confirmPasswordRegisterField;
        [SerializeField] private TMP_Text _warningRegisterText;
        
        [Space] 
        [SerializeField] private int _gameSceneBuildIndex = 1;
        
        public event Action<int> onSceneLoaded;
        
        private void Awake()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.Log("Could not resolve all firebase dependencies: " + _dependencyStatus);
                }
            });
        }

        private void InitializeFirebase()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        private void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            if (_auth.CurrentUser != _user)
            {
                bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null
                                                           && _auth.CurrentUser.IsValid();
                if (!signedIn && _user != null)
                {
                    Debug.Log("Signed out " + _user.UserId);
                }

                _user = _auth.CurrentUser;

                if (signedIn)
                {
                    Debug.Log("Signed in " + _user.UserId);
                }
            }
        }

        public void RegistrationButton()
        {
            StartCoroutine(Registration(_emailRegisterField.text,_passwordRegisterField.text,_nameRegisterField.text));
        }

        public void LoginButton()
        {
            StartCoroutine(Login(_emailLoginField.text,_passwordLoginField.text));
        }
        
        private IEnumerator Registration(string _email, string _password, string _username)
        {
            if (_username == "")
            {
                _warningRegisterText.text = "User name is empty";
            }
            else if (_email == "")
            {
                _warningRegisterText.text = "Email field is empty";
            }
            else if (_passwordRegisterField.text != _confirmPasswordRegisterField.text)
            {
                _warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                Task<AuthResult> RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                if (RegisterTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    _warningRegisterText.text = GetErrorMessage(errorCode);
                }
                else
                {
                    _user = RegisterTask.Result.User;

                    if (_user != null)
                    {
                        UserProfile profile = new UserProfile { DisplayName = _username };

                        Task profileTask = _user.UpdateUserProfileAsync(profile);

                        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                        if (profileTask.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                            
                            FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                            _warningRegisterText.text = "Username Set Failed!";
                        }
                        else
                        {
                            _warningRegisterText.text = "";
                            onSceneLoaded?.Invoke(_gameSceneBuildIndex);
                        }
                    }
                }
            }
        }

        private string GetErrorMessage(AuthError errorCode)
        {
            
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    return "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    return "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    return "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    return "Email Already In Use";
                    break;
            }

            return null;
        }
    

        private IEnumerator Login(string _email, string _password)
        {
            Task<AuthResult> LoginTask = _auth.SignInWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            if (LoginTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                
                _warningLoginText.text = GetErrorMessage(errorCode);
            }
            else
            {
                _user = LoginTask.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);
                _warningLoginText.text = "";
                onSceneLoaded?.Invoke(_gameSceneBuildIndex);
            }
        }

        private void OnDisable()
        {
            _auth.StateChanged -= AuthStateChanged;
            _auth = null;
        }
    }
}