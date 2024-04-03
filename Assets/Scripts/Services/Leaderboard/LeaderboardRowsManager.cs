using System.Collections.Generic;
using UnityEngine;

namespace Services.Leaderboard
{
    public class LeaderboardRowsManager: MonoBehaviour
    {
        [SerializeField] private LeaderboardRowData _prefabRowData;

        private void ClearListData()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        
        public void AddHighscoreDataToLeaderboardUI(List<LeaderboardData> leaderboardData)
        {
            ClearListData();
            for (int i = 0; i < leaderboardData.Count; i++)
            {
                var row = Instantiate(_prefabRowData, transform);
                row.Rank.text = (i + 1).ToString();
                row.Username.text = leaderboardData[i].UserName;
                row.Score.text = leaderboardData[i].UserScore.ToString();
            }
        }
    }
}