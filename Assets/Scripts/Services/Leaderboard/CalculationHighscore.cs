using System.Collections.Generic;
using UnityEngine;

namespace Services.Leaderboard
{
    public class CalculationHighscore: MonoBehaviour
    {
        [SerializeField] private RowData rowData;

        private void ClearListData()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        
        public void GetRow(List<LeaderboardData> _leaderboardData)
        {
            ClearListData();
            for (int i = 0; i < _leaderboardData.Count; i++)
            {
                var row = Instantiate(rowData, transform).GetComponent<RowData>();
                row.rank.text = (i + 1).ToString();
                row.username.text = _leaderboardData[i].UserName;
                row.score.text = _leaderboardData[i].UserScore.ToString();
            }
        }
    }
}