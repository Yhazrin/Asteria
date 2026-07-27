using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// Detects nearby IInteractable targets and fires Interact on key press.
    /// </summary>
    public sealed class InteractionDetector : MonoBehaviour
    {
        [SerializeField] float radius = 3.5f;
        [SerializeField] KeyCode interactKey = KeyCode.E;
        [SerializeField] LayerMask interactMask = ~0;

        readonly Collider[] _hits = new Collider[16];
        IInteractable _current;

        public IInteractable Current => _current;

        void Update()
        {
            _current = FindBestTarget();
            GameHud.SetPrompt(_current != null ? _current.PromptText : string.Empty);

            if (_current != null && _current.CanInteract && Input.GetKeyDown(interactKey))
            {
                _current.Interact(new InteractionContext(transform));
            }
        }

        IInteractable FindBestTarget()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                radius,
                _hits,
                interactMask,
                QueryTriggerInteraction.Collide);

            IInteractable best = null;
            float bestDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider col = _hits[i];
                if (col == null)
                {
                    continue;
                }

                IInteractable interactable = col.GetComponentInParent<IInteractable>();
                if (interactable == null || !interactable.CanInteract)
                {
                    continue;
                }

                float dist = (col.transform.position - transform.position).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = interactable;
                }
            }

            return best;
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.7f, 0.9f, 1f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
