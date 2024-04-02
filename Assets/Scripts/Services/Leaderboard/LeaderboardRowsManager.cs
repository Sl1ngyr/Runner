using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.Leaderboard
{
    public class LeaderboardRowsManager: MonoBehaviour
    {
        [SerializeField] private LeaderboardRowData _leaderboardRowData;

        private void ClearListData()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        
        public void GetRow(List<LeaderboardData> leaderboardData)
        {
            ClearListData();
            for (int i = 0; i < leaderboardData.Count; i++)
            {
                var row = Instantiate(_leaderboardRowData, transform);
                row.Rank.text = (i + 1).ToString();
                row.Username.text = leaderboardData[i].UserName;
                row.Score.text = leaderboardData[i].UserScore.ToString();
            }
        }
    }
}