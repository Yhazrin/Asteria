using Asteria.Interaction;
using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Interactable beacon in the expedition scene that returns the player home.
    /// </summary>
    public sealed class ReturnHomeInteractable : MonoBehaviour, IInteractable
    {
        public string PromptText => "按 E 返回家园";
        public bool CanInteract => true;

        public void Interact(InteractionContext context)
        {
            Debug.Log("[Asteria] Returning home...");
            var flow = SceneFlowManager.Instance;
            if (flow != null)
            {
                flow.GoHome();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("HomePlanet");
            }
        }
    }
}
