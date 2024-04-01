using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Services.Scene;
using TMPro;
using UnityEngine;
using Zenject;
using System;

namespace Services.Database.Firebase
{
    public class DatabaseManager : MonoBehaviour
    {
        [Header("Firebase")] 
        [SerializeField] private DependencyStatus _dependencyStatus;
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private DatabaseReference _databaseReference;
        
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

        private string _username;
        private int _score;

        [Inject] private SceneLoader _sceneLoader;
        
        private static DatabaseManager _instance;
        
        public static DatabaseManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            
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
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
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
        
        private IEnumerator Registration(string email, string password, string username)
        {
            if (username == "")
            {
                _warningRegisterText.text = "User name is empty";
            }
            else if (email == "")
            {
                _warningRegisterText.text = "Email field is empty";
            }
            else if (_passwordRegisterField.text != _confirmPasswordRegisterField.text)
            {
                _warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

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
                        UserProfile profile = new UserProfile { DisplayName = username };

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
                            AddUserDataToDatabase(username);
                            _warningRegisterText.text = "";
                            
                            _sceneLoader.TransitionToSceneByIndex(_gameSceneBuildIndex);
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
            var LoginTask = _auth.SignInWithEmailAndPasswordAsync(_email, _password);

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
                StartCoroutine(LoadUserData());
                Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);
                _warningLoginText.text = "";
                _sceneLoader.TransitionToSceneByIndex(_gameSceneBuildIndex);
            }
        }

        private void AddUserDataToDatabase(string username)
        {
            StartCoroutine(UpdateUsernameDatabase(username));
            StartCoroutine(UpdateScoreDatabase(0));
        }
        
        private IEnumerator UpdateUsernameDatabase(string username)
        {
            var dbTask = _databaseReference.Child("users").Child(_user.UserId).Child("username")
                .SetValueAsync(username);
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
            else
            {
                _username = username;
            }
        }

        private void OnDisable()
        {
            _auth.StateChanged -= AuthStateChanged;
            _auth = null;
        }
        
        
        public IEnumerator UpdateScoreDatabase(int score)
        {
            if (_score < score || score == 0 && _score == 0)
            {
                var dbTask = _databaseReference.Child("users").Child(_user.UserId).Child("score")
                    .SetValueAsync(score.ToString());
                yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

                if (dbTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
                }
                else
                {
                    _score = score;
                }
            }
        }
        
        private IEnumerator LoadUserData()
        {
            var dbTask = _databaseReference.Child("users").Child(_user.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
            else if (dbTask.Result.Value == null)
            {
                _score = 0;
            }
            else
            {
                DataSnapshot snapshot = dbTask.Result;
                _score = int.Parse(snapshot.Child("score").Value.ToString());
            }
        }
        
        public IEnumerator LoadLeaderboardData(List<int> scores, List<string> usernames, Action onComplete)
        {
            var dbTask = _databaseReference.Child("users").OrderByChild("score").GetValueAsync();

            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
            else
            {
                DataSnapshot snapshot = dbTask.Result;
                
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    string username = childSnapshot.Child("username").Value.ToString();
                    int score = int.Parse(childSnapshot.Child("score").Value.ToString());
                    
                    scores.Add(score);
                    usernames.Add(username);
                }
                onComplete?.Invoke();
            }
        }
        
    }
}