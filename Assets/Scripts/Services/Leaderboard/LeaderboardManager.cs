using System.Collections.Generic;
using System.Linq;
using Services.Database.Firebase;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Leaderboard
{
    public class LeaderboardData
    {
        public string UserName;
        public float UserScore;

        public LeaderboardData(string name, float score)
        {
            UserName = name;
            UserScore = score;
        }
    }

    public class LeaderboardManager : MonoBehaviour
    {
        private List<int> _scores = new List<int>();
        private List<string> _usernames = new List<string>();
        private List<LeaderboardData> _leaderboardData = new List<LeaderboardData>();

        [SerializeField] private Canvas _leaderbordCanvas;
        [SerializeField] private CalculationHighscore leaderbordData;
        
        [SerializeField] private Button _openLeaderboard;
        [SerializeField] private Button _closeLeaderboard;
        
        private void OpenLeaderboard()
        {
            _scores.Clear();
            _usernames.Clear();
            _leaderboardData.Clear();
            
            StartCoroutine(DatabaseManager.Instance.LoadLeaderboardData(_scores, _usernames,
                () =>
                {
                    for (int i = 0; i < _scores.Count; i++)
                    {
                        _leaderboardData.Add(new LeaderboardData(_usernames[i], _scores[i]));
                    }

                    GetHighScores(_leaderboardData);
                    EnableLeaderboard();
                    leaderbordData.GetRow(_leaderboardData);
                }));
            
        }

        private void EnableLeaderboard()
        {
            _leaderbordCanvas.gameObject.SetActive(true);
        }

        private void DisableLeaderboard()
        {
            _leaderbordCanvas.gameObject.SetActive(false);
        }
        
        private IEnumerable<LeaderboardData> GetHighScores(List<LeaderboardData> leaderboardDatas)
        {
            return leaderboardDatas.OrderByDescending(x => x.UserScore);
        }

        private void OnEnable()
        {
            _openLeaderboard.onClick.AddListener(OpenLeaderboard);
            _closeLeaderboard.onClick.AddListener(DisableLeaderboard);
            
        }

        private void OnDisable()
        {
            _openLeaderboard.onClick.RemoveListener(OpenLeaderboard);
            _closeLeaderboard.onClick.RemoveListener(DisableLeaderboard);
        }
    }
}