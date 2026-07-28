using System.Collections.Generic;
using System.Linq;
using Asteria.Planet;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.Residents
{
    /// <summary>
    /// Manages all residents on the home planet. Handles spawn, schedule ticks,
    /// and autonomous interactions between residents.
    /// </summary>
    public sealed class ResidentManager : MonoBehaviour
    {
        [SerializeField] ResidentDefinition[] residentDefinitions;
        [SerializeField] PlanetBody planet;

        readonly List<ResidentAgent> _agents = new();
        float _interactionCheckTimer;

        public IReadOnlyList<ResidentAgent> Agents => _agents;

        /// <summary>
        /// Initialize with definitions and planet before Start() runs.
        /// Call this from code that creates the manager at runtime.
        /// </summary>
        public void Initialize(ResidentDefinition[] definitions, PlanetBody targetPlanet)
        {
            residentDefinitions = definitions;
            planet = targetPlanet;
        }

        void Start()
        {
            if (planet == null)
            {
                planet = FindFirstObjectByType<PlanetBody>();
            }

            SpawnResidents();
        }

        void SpawnResidents()
        {
            if (residentDefinitions == null || residentDefinitions.Length == 0)
            {
                Debug.Log("[Asteria] No resident definitions assigned to ResidentManager.");
                return;
            }

            foreach (var def in residentDefinitions)
            {
                if (def == null)
                {
                    continue;
                }

                var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                go.name = $"Resident_{def.DisplayName}";
                Destroy(go.GetComponent<CapsuleCollider>());
                CapsuleCollider col = go.AddComponent<CapsuleCollider>();
                col.height = 2f;
                col.radius = 0.35f;

                MaterialHelper.ApplyColor(go.GetComponent<MeshRenderer>(), def.BodyColor);

                // Add agent
                var agent = go.AddComponent<ResidentAgent>();
                agent.Initialize(def, planet);

                // Add mood bubble
                var moodBubbleGo = new GameObject("MoodBubble");
                moodBubbleGo.transform.SetParent(go.transform, false);
                moodBubbleGo.transform.localPosition = new Vector3(0, 2.5f, 0);
                var moodRenderer = moodBubbleGo.AddComponent<SpriteRenderer>();
                moodRenderer.sortingOrder = 10;
                var moodBubble = moodBubbleGo.AddComponent<ResidentMoodBubble>();

                // Add dialogue bubble
                var dialogueGo = new GameObject("DialogueBubble");
                dialogueGo.transform.SetParent(go.transform, false);
                dialogueGo.transform.localPosition = new Vector3(0, 3f, 0);
                var dialogueBubble = dialogueGo.AddComponent<ResidentDialogueBubble>();

                _agents.Add(agent);

                // Show initial mood based on personality
                ShowInitialMood(agent, moodBubble);
            }

            Debug.Log($"[Asteria] Spawned {_agents.Count} residents with mood and dialogue systems.");
        }

        void Update()
        {
            // Check for autonomous interactions periodically
            _interactionCheckTimer -= Time.deltaTime;
            if (_interactionCheckTimer <= 0f)
            {
                _interactionCheckTimer = AsteriaConstants.ResidentInteractionCheckInterval;
                CheckAutonomousInteractions();
            }
        }

        void CheckAutonomousInteractions()
        {
            for (int i = 0; i < _agents.Count; i++)
            {
                for (int j = i + 1; j < _agents.Count; j++)
                {
                    var a = _agents[i];
                    var b = _agents[j];

                    if (a == null || b == null) continue;

                    float distance = Vector3.Distance(a.transform.position, b.transform.position);
                    if (distance < AsteriaConstants.ResidentInteractionDistance)
                    {
                        bool interacted = a.TryInteract(b);
                        if (interacted)
                        {
                            // Show mood and dialogue for interaction
                            TriggerInteractionFeedback(a, b);
                        }
                    }
                }
            }
        }

        void TriggerInteractionFeedback(ResidentAgent a, ResidentAgent b)
        {
            // Show mood bubbles
            var moodA = a.GetComponentInChildren<ResidentMoodBubble>();
            var moodB = b.GetComponentInChildren<ResidentMoodBubble>();

            if (moodA != null) moodA.ShowMood(ResidentMoodBubble.MoodType.Happy);
            if (moodB != null) moodB.ShowMood(ResidentMoodBubble.MoodType.Happy);

            // Show dialogue bubbles
            var dialogueA = a.GetComponentInChildren<ResidentDialogueBubble>();
            var dialogueB = b.GetComponentInChildren<ResidentDialogueBubble>();

            string[] greetings = { "你好呀！", "今天天气真好~", "好久不见！", "一起走走？", "嘿嘿" };
            string greeting = greetings[Random.Range(0, greetings.Length)];

            if (dialogueA != null) dialogueA.ShowDialogue(greeting);
            if (dialogueB != null) dialogueB.ShowDialogue("好呀！");
        }

        void ShowInitialMood(ResidentAgent agent, ResidentMoodBubble moodBubble)
        {
            if (agent.Definition == null) return;

            // Show mood based on personality
            float warmth = agent.Definition.Warmth;
            float curiosity = agent.Definition.Curiosity;

            if (warmth > 0.5f)
            {
                moodBubble.ShowMood(ResidentMoodBubble.MoodType.Happy);
            }
            else if (curiosity > 0.5f)
            {
                moodBubble.ShowMood(ResidentMoodBubble.MoodType.Curious);
            }
        }

        /// <summary>
        /// Get a resident by ID.
        /// </summary>
        public ResidentAgent GetResident(string residentId)
        {
            return _agents.FirstOrDefault(a => a.Definition != null && a.Definition.ResidentId == residentId);
        }

        /// <summary>
        /// Get all resident states for saving.
        /// </summary>
        public ResidentState[] GetStatesForSave()
        {
            return _agents.Where(a => a.State != null).Select(a => a.State).ToArray();
        }
    }
}
