using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.Leaderboard
{
    public class LeaderboardRowData : MonoBehaviour
    {
        public TextMeshProUGUI Rank;
        public TextMeshProUGUI Username;
        public TextMeshProUGUI Score;
    }
}