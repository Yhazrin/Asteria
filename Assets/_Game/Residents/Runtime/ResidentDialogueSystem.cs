using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Manages resident dialogue trees and conversation flow.
    /// Residents can have contextual conversations based on their state and relationships.
    /// </summary>
    public sealed class ResidentDialogueSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float dialogueDuration = 5f;
        [SerializeField] float conversationCooldown = 30f;

        readonly Dictionary<string, DialogueTree> _dialogueTrees = new();
        readonly Dictionary<string, float> _lastConversationTime = new();

        void Awake()
        {
            InitializeDefaultDialogues();
        }

        void InitializeDefaultDialogues()
        {
            // Greeting dialogues
            RegisterDialogue("greeting_warm", new DialogueTree
            {
                dialogueId = "greeting_warm",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "你好呀！今天天气真不错~", emotion = "happy" },
                    new DialogueLine { speaker = "player", text = "你好！", emotion = "neutral" },
                    new DialogueLine { speaker = "resident", text = "要不要一起去散步？", emotion = "curious" },
                },
                conditions = new[] { "warmth > 0.5" },
                effects = new[] { "affinity +0.05" }
            });

            RegisterDialogue("greeting_shy", new DialogueTree
            {
                dialogueId = "greeting_shy",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "...你好。", emotion = "shy" },
                    new DialogueLine { speaker = "player", text = "你好！", emotion = "neutral" },
                    new DialogueLine { speaker = "resident", text = "嗯...那个...天气不错。", emotion = "shy" },
                },
                conditions = new[] { "sociability < -0.3" },
                effects = new[] { "familiarity +0.03" }
            });

            // Weather dialogues
            RegisterDialogue("weather_sunny", new DialogueTree
            {
                dialogueId = "weather_sunny",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "今天阳光真好！", emotion = "happy" },
                    new DialogueLine { speaker = "resident", text = "适合出去走走。", emotion = "neutral" },
                },
                conditions = new[] { "weather == clear" },
                effects = new[] { "mood +0.1" }
            });

            RegisterDialogue("weather_rainy", new DialogueTree
            {
                dialogueId = "weather_rainy",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "下雨了呢...", emotion = "melancholy" },
                    new DialogueLine { speaker = "resident", text = "不过雨声很舒服。", emotion = "neutral" },
                },
                conditions = new[] { "weather == rain" },
                effects = new[] { "mood -0.05" }
            });

            // Discovery dialogues
            RegisterDialogue("discovery_new", new DialogueTree
            {
                dialogueId = "discovery_new",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "你发现了什么新东西？", emotion = "curious" },
                    new DialogueLine { speaker = "player", text = "一种会发光的石头！", emotion = "excited" },
                    new DialogueLine { speaker = "resident", text = "哇！好想去看看！", emotion = "excited" },
                },
                conditions = new[] { "new_discovery == true" },
                effects = new[] { "curiosity +0.1", "affinity +0.05" }
            });

            // Relationship dialogues
            RegisterDialogue("conflict_mild", new DialogueTree
            {
                dialogueId = "conflict_mild",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident_a", text = "你怎么又迟到了！", emotion = "angry" },
                    new DialogueLine { speaker = "resident_b", text = "对不起嘛...", emotion = "guilty" },
                    new DialogueLine { speaker = "resident_a", text = "算了，下次注意。", emotion = "neutral" },
                },
                conditions = new[] { "tension > 0.5", "affinity > 0.3" },
                effects = new[] { "tension -0.1", "affinity -0.02" }
            });

            // Wish dialogues
            RegisterDialogue("wish_express", new DialogueTree
            {
                dialogueId = "wish_express",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "我有个小小的愿望...", emotion = "hopeful" },
                    new DialogueLine { speaker = "resident", text = "想看看远方的星球是什么样子。", emotion = "curious" },
                    new DialogueLine { speaker = "player", text = "我会帮你实现的！", emotion = "determined" },
                    new DialogueLine { speaker = "resident", text = "真的吗？太好了！", emotion = "happy" },
                },
                conditions = new[] { "affinity > 0.6", "exploration > 0.5" },
                effects = new[] { "wish_created", "affinity +0.1" }
            });

            // Quirk dialogues
            RegisterDialogue("quirk_plant_namer", new DialogueTree
            {
                dialogueId = "quirk_plant_namer",
                lines = new[]
                {
                    new DialogueLine { speaker = "resident", text = "你看这棵小草！", emotion = "excited" },
                    new DialogueLine { speaker = "resident", text = "我给它取名叫'小绿'。", emotion = "proud" },
                    new DialogueLine { speaker = "player", text = "好可爱的名字。", emotion = "amused" },
                    new DialogueLine { speaker = "resident", text = "嘿嘿，每棵植物都值得有自己的名字。", emotion = "happy" },
                },
                conditions = new[] { "quirk == plant_namer" },
                effects = new[] { "mood +0.15" }
            });
        }

        void RegisterDialogue(string id, DialogueTree tree)
        {
            _dialogueTrees[id] = tree;
        }

        /// <summary>
        /// Get a dialogue for a resident based on their current state.
        /// </summary>
        public DialogueTree GetDialogue(ResidentAgent resident, string context = "idle")
        {
            if (resident?.Definition == null) return null;

            string residentId = resident.Definition.ResidentId;

            // Check cooldown
            if (_lastConversationTime.TryGetValue(residentId, out float lastTime))
            {
                if (Time.time - lastTime < conversationCooldown) return null;
            }

            // Find matching dialogue
            foreach (var tree in _dialogueTrees.Values)
            {
                if (CheckConditions(tree.conditions, resident))
                {
                    _lastConversationTime[residentId] = Time.time;
                    return tree;
                }
            }

            return null;
        }

        bool CheckConditions(string[] conditions, ResidentAgent resident)
        {
            if (conditions == null || conditions.Length == 0) return true;

            foreach (var condition in conditions)
            {
                // Simple condition checking
                if (condition.StartsWith("warmth >"))
                {
                    float threshold = float.Parse(condition.Split('>')[1].Trim());
                    if (resident.Definition.Warmth <= threshold) return false;
                }
                else if (condition.StartsWith("sociability <"))
                {
                    float threshold = float.Parse(condition.Split('<')[1].Trim());
                    if (resident.Definition.Sociability >= threshold) return false;
                }
                else if (condition.StartsWith("affinity >"))
                {
                    float threshold = float.Parse(condition.Split('>')[1].Trim());
                    if (resident.State?.affinity <= threshold) return false;
                }
                else if (condition.StartsWith("tension >"))
                {
                    float threshold = float.Parse(condition.Split('>')[1].Trim());
                    if (resident.State?.tension <= threshold) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Trigger a dialogue for a resident.
        /// </summary>
        public void TriggerDialogue(ResidentAgent resident, string dialogueId)
        {
            if (!_dialogueTrees.TryGetValue(dialogueId, out var tree)) return;

            var bubble = resident.GetComponentInChildren<ResidentDialogueBubble>();
            if (bubble == null) return;

            // Show first line
            if (tree.lines.Length > 0)
            {
                bubble.ShowDialogue(tree.lines[0].text);
            }
        }

        public struct DialogueTree
        {
            public string dialogueId;
            public DialogueLine[] lines;
            public string[] conditions;
            public string[] effects;
        }

        public struct DialogueLine
        {
            public string speaker;
            public string text;
            public string emotion;
        }
    }
}
