using System.Collections.Generic;
using System.Linq;
using Asteria.Expedition;
using Asteria.Persistence;
using Asteria.Residents;
using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Handles expedition settlement when returning home.
    /// Processes discoveries, fulfills wishes, triggers follow-up events.
    /// </summary>
    public static class ExpeditionSettlement
    {
        /// <summary>
        /// Settle an expedition result. Called when the player returns home.
        /// </summary>
        public static void Settle(ExpeditionResult result, ISaveService saveService, ResidentManager residentManager)
        {
            if (result == null || saveService == null)
            {
                return;
            }

            var save = saveService.Current;

            // Record expedition in history
            var historyEntry = new ExpeditionResultDTO
            {
                expeditionId = result.expeditionId,
                durationSeconds = result.durationSeconds,
                discoveredIds = new List<string>(result.discoveredIds),
                outcomeType = result.outcomeType
            };

            save.expeditionHistory.Add(historyEntry);

            // Check wish fulfillment
            foreach (var wish in save.activeWishes.Where(w => w.status == "active").ToList())
            {
                if (result.discoveredIds.Contains(wish.wishId.Replace("wish_", "observe_")))
                {
                    wish.status = "fulfilled";
                    wish.fulfilledByExpeditionId = result.expeditionId;

                    // Notify the resident
                    if (residentManager != null)
                    {
                        var resident = residentManager.GetResident(wish.residentId);
                        if (resident != null)
                        {
                            resident.RecordWish($"Wish fulfilled: {wish.wishId}");

                            // Improve relationship
                            resident.State.affinity = Mathf.Min(1f, resident.State.affinity + 0.15f);
                            resident.State.trust = Mathf.Min(1f, resident.State.trust + 0.1f);
                        }
                    }

                    Debug.Log($"[Asteria] Wish {wish.wishId} fulfilled by expedition {result.expeditionId}");
                }
            }

            // Trigger follow-up events for residents
            if (residentManager != null)
            {
                TriggerFollowUpEvents(result, residentManager);
            }

            saveService.Save();
        }

        static void TriggerFollowUpEvents(ExpeditionResult result, ResidentManager residentManager)
        {
            var agents = residentManager.Agents;
            if (agents.Count < 2)
            {
                return;
            }

            // If expedition was successful, trigger a celebration interaction
            if (result.outcomeType == "success" && result.discoveredIds.Count > 0)
            {
                // Force an interaction between the two residents
                var a = agents[0];
                var b = agents[1];

                // Move them closer together for the celebration
                Vector3 midPoint = (a.transform.position + b.transform.position) / 2f;
                a.transform.position = Vector3.Lerp(a.transform.position, midPoint, 0.5f);
                b.transform.position = Vector3.Lerp(b.transform.position, midPoint, 0.5f);

                Debug.Log($"[Asteria] Follow-up: {a.Definition.displayName} and {b.Definition.displayName} celebrate the expedition results.");
            }
        }
    }
}
