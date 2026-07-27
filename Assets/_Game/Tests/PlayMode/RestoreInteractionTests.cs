using System.Collections;
using Asteria.Interaction;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for Restore multi-phase interaction.
    /// Required by TEST_SPEC.md PlayMode matrix.
    /// </summary>
    [TestFixture]
    public class RestoreInteractionTests
    {
        [UnityTest]
        public IEnumerator RestoreInteractable_CanInteract_WhenNotRestored()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var restore = go.AddComponent<RestoreInteractable>();

            yield return null;

            Assert.IsTrue(restore.CanInteract, "Should be interactable when not restored");
            Object.DestroyImmediate(go);
        }

        [UnityTest]
        public IEnumerator RestoreInteractable_BlocksAfterRestore()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var restore = go.AddComponent<RestoreInteractable>();

            yield return null;

            // Simulate completing the restore
            restore.Interact(new InteractionContext(null));

            yield return null;

            // After one-shot, should not be interactable
            Assert.IsFalse(restore.CanInteract, "Should not be interactable after restore");

            Object.DestroyImmediate(go);
        }
    }
}
