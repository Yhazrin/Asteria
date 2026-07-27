using Asteria.Interaction;
using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Interactable beacon that triggers transition to the expedition scene.
    /// </summary>
    public sealed class DepartureBeaconInteractable : MonoBehaviour, IInteractable
    {
        public string PromptText => "按 E 出发远征";
        public bool CanInteract => true;

        public void Interact(InteractionContext context)
        {
            Debug.Log("[Asteria] Departing for expedition...");
            var flow = SceneFlowManager.Instance;
            if (flow != null)
            {
                flow.StartExpedition();
            }
            else
            {
                // Fallback: direct scene load
                UnityEngine.SceneManagement.SceneManager.LoadScene("SphereMoveDemo");
            }
        }
    }
}
