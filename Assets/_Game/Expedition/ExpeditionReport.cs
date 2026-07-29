using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Generates a shareable expedition report after completing an expedition.
    /// Includes statistics, discoveries, and memorable moments.
    /// </summary>
    public sealed class ExpeditionReport : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float reportDisplayTime = 10f;

        // Report data
        ExpeditionReportData _currentReport;

        /// <summary>
        /// Generate a report from expedition results.
        /// </summary>
        public ExpeditionReportData GenerateReport(ExpeditionResult result, Scoring.ScoreSummary summary)
        {
            var report = new ExpeditionReportData
            {
                expeditionId = result.expeditionId,
                timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                duration = result.durationSeconds,
                outcome = result.outcomeType,

                // Statistics
                discoveries = result.discoveredIds.Count,
                restores = result.restoredIds.Count,
                cooperates = result.cooperatedIds.Count,
                rescues = result.rescueCount,

                // Calculate total score
                totalScore = CalculateTotalScore(result, summary),

                // Generate narrative
                narrative = GenerateNarrative(result, summary),

                // Key moments
                keyMoments = ExtractKeyMoments(summary),

                // Recommendations
                recommendations = GenerateRecommendations(result)
            };

            _currentReport = report;
            return report;
        }

        int CalculateTotalScore(ExpeditionResult result, Scoring.ScoreSummary summary)
        {
            int score = 0;
            score += result.discoveredIds.Count * 10;
            score += result.restoredIds.Count * 20;
            score += result.cooperatedIds.Count * 30;
            score += result.rescueCount * 15;

            // Time bonus
            if (result.durationSeconds < 1200f) score += 50; // Under 20 min
            else if (result.durationSeconds > 2400f) score -= 20; // Over 40 min

            return score;
        }

        string GenerateNarrative(ExpeditionResult result, Scoring.ScoreSummary summary)
        {
            var parts = new List<string>();

            // Opening
            parts.Add($"远征持续了 {FormatDuration(result.durationSeconds)}。");

            // Discoveries
            if (result.discoveredIds.Count > 0)
            {
                parts.Add($"发现了 {result.discoveredIds.Count} 个新事物。");
            }

            // Outcome
            switch (result.outcomeType)
            {
                case "perfect":
                    parts.Add("这是一次完美的远征！");
                    break;
                case "success":
                    parts.Add("远征顺利完成。");
                    break;
                case "partial":
                    parts.Add("远征部分完成，还有一些目标未达成。");
                    break;
                case "minimal":
                    parts.Add("远征收获有限，但安全返回。");
                    break;
            }

            // Rescues
            if (result.rescueCount > 0)
            {
                parts.Add($"成功救援了 {result.rescueCount} 次。");
            }

            return string.Join(" ", parts);
        }

        List<string> ExtractKeyMoments(Scoring.ScoreSummary summary)
        {
            var moments = new List<string>();

            foreach (var evt in summary.events)
            {
                switch (evt.type)
                {
                    case "discovery":
                        moments.Add($"发现了 {evt.name}");
                        break;
                    case "restore":
                        moments.Add($"修复了 {evt.name}");
                        break;
                    case "cooperate":
                        moments.Add($"完成了合作：{evt.name}");
                        break;
                    case "rescue":
                        moments.Add(evt.name);
                        break;
                }
            }

            return moments;
        }

        List<string> GenerateRecommendations(ExpeditionResult result)
        {
            var recommendations = new List<string>();

            if (result.discoveredIds.Count == 0)
            {
                recommendations.Add("下次尝试探索更多兴趣点。");
            }

            if (result.restoredIds.Count == 0)
            {
                recommendations.Add("尝试修复一些损坏的设施。");
            }

            if (result.cooperatedIds.Count == 0)
            {
                recommendations.Add("邀请朋友一起完成合作任务。");
            }

            if (result.rescueCount > 0)
            {
                recommendations.Add("准备好救援工具以防万一。");
            }

            return recommendations;
        }

        string FormatDuration(float seconds)
        {
            int minutes = Mathf.FloorToInt(seconds / 60f);
            int secs = Mathf.FloorToInt(seconds % 60f);
            return $"{minutes}分{secs}秒";
        }

        /// <summary>
        /// Get the current report.
        /// </summary>
        public ExpeditionReportData GetCurrentReport()
        {
            return _currentReport;
        }

        [System.Serializable]
        public class ExpeditionReportData
        {
            public string expeditionId;
            public string timestamp;
            public float duration;
            public string outcome;
            public int discoveries;
            public int restores;
            public int cooperates;
            public int rescues;
            public int totalScore;
            public string narrative;
            public List<string> keyMoments;
            public List<string> recommendations;
        }
    }
}
