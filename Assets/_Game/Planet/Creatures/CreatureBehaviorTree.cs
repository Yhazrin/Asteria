using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Creatures
{
    /// <summary>
    /// Behavior tree for more complex creature AI.
    /// Supports sequences, selectors, and decorators.
    /// </summary>
    public abstract class BehaviorNode
    {
        public enum Status { Running, Success, Failure }

        public abstract Status Evaluate(CreatureContext context);
    }

    public class CreatureContext
    {
        public CreatureAgent agent;
        public Transform player;
        public PlanetBody planet;
        public float playerDistance;
        public Vector3 homePosition;
        public float trustLevel;
        public List<CreatureAgent> nearbyCreatures;
    }

    // === Composite Nodes ===

    public class SequenceNode : BehaviorNode
    {
        readonly List<BehaviorNode> _children = new();

        public void AddChild(BehaviorNode child) => _children.Add(child);

        public override Status Evaluate(CreatureContext context)
        {
            foreach (var child in _children)
            {
                var status = child.Evaluate(context);
                if (status != Status.Success) return status;
            }
            return Status.Success;
        }
    }

    public class SelectorNode : BehaviorNode
    {
        readonly List<BehaviorNode> _children = new();

        public void AddChild(BehaviorNode child) => _children.Add(child);

        public override Status Evaluate(CreatureContext context)
        {
            foreach (var child in _children)
            {
                var status = child.Evaluate(context);
                if (status != Status.Failure) return status;
            }
            return Status.Failure;
        }
    }

    // === Decorator Nodes ===

    public class InverterNode : BehaviorNode
    {
        readonly BehaviorNode _child;

        public InverterNode(BehaviorNode child) => _child = child;

        public override Status Evaluate(CreatureContext context)
        {
            var status = _child.Evaluate(context);
            return status switch
            {
                Status.Success => Status.Failure,
                Status.Failure => Status.Success,
                _ => status
            };
        }
    }

    public class RepeatNode : BehaviorNode
    {
        readonly BehaviorNode _child;
        readonly int _maxRepeats;
        int _currentRepeat;

        public RepeatNode(BehaviorNode child, int maxRepeats)
        {
            _child = child;
            _maxRepeats = maxRepeats;
        }

        public override Status Evaluate(CreatureContext context)
        {
            if (_currentRepeat >= _maxRepeats)
            {
                _currentRepeat = 0;
                return Status.Success;
            }

            var status = _child.Evaluate(context);
            if (status == Status.Success)
            {
                _currentRepeat++;
                return Status.Running;
            }

            return status;
        }
    }

    // === Leaf Nodes (Conditions) ===

    public class IsPlayerCloseNode : BehaviorNode
    {
        readonly float _distance;

        public IsPlayerCloseNode(float distance) => _distance = distance;

        public override Status Evaluate(CreatureContext context)
        {
            return context.playerDistance < _distance ? Status.Success : Status.Failure;
        }
    }

    public class IsPlayerFarNode : BehaviorNode
    {
        readonly float _distance;

        public IsPlayerFarNode(float distance) => _distance = distance;

        public override Status Evaluate(CreatureContext context)
        {
            return context.playerDistance > _distance ? Status.Success : Status.Failure;
        }
    }

    public class HasTrustNode : BehaviorNode
    {
        readonly float _threshold;

        public HasTrustNode(float threshold) => _threshold = threshold;

        public override Status Evaluate(CreatureContext context)
        {
            return context.trustLevel >= _threshold ? Status.Success : Status.Failure;
        }
    }

    public class IsBehaviorNode : BehaviorNode
    {
        readonly CreatureBehavior _behavior;

        public IsBehaviorNode(CreatureBehavior behavior) => _behavior = behavior;

        public override Status Evaluate(CreatureContext context)
        {
            return context.agent.Definition.behavior == _behavior ? Status.Success : Status.Failure;
        }
    }

    // === Leaf Nodes (Actions) ===

    public class MoveToPlayerNode : BehaviorNode
    {
        public override Status Evaluate(CreatureContext context)
        {
            if (context.player == null) return Status.Failure;

            Vector3 direction = (context.player.position - context.agent.transform.position).normalized;
            context.agent.transform.position += direction * context.agent.Definition.moveSpeed * Time.deltaTime;

            float dist = Vector3.Distance(context.agent.transform.position, context.player.position);
            return dist < 2f ? Status.Success : Status.Running;
        }
    }

    public class FleeFromPlayerNode : BehaviorNode
    {
        public override Status Evaluate(CreatureContext context)
        {
            if (context.player == null) return Status.Failure;

            Vector3 direction = (context.agent.transform.position - context.player.position).normalized;
            context.agent.transform.position += direction * context.agent.Definition.moveSpeed * 1.5f * Time.deltaTime;

            float dist = Vector3.Distance(context.agent.transform.position, context.player.position);
            return dist > context.agent.Definition.detectionRadius * 2f ? Status.Success : Status.Running;
        }
    }

    public class WanderNode : BehaviorNode
    {
        Vector3 _targetPosition;
        float _wanderTimer;

        public override Status Evaluate(CreatureContext context)
        {
            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0f)
            {
                // Pick new random position
                Vector3 randomDir = Random.onUnitSphere;
                _targetPosition = context.planet.GetPointOnSurface(
                    (context.agent.transform.position.normalized + randomDir * 0.3f).normalized, 1f);
                _wanderTimer = Random.Range(3f, 8f);
            }

            Vector3 direction = (_targetPosition - context.agent.transform.position).normalized;
            context.agent.transform.position += direction * context.agent.Definition.moveSpeed * 0.5f * Time.deltaTime;

            float dist = Vector3.Distance(context.agent.transform.position, _targetPosition);
            return dist < 1f ? Status.Success : Status.Running;
        }
    }

    public class IdleNode : BehaviorNode
    {
        float _idleTimer;

        public override Status Evaluate(CreatureContext context)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0f)
            {
                _idleTimer = Random.Range(2f, 5f);
                return Status.Success;
            }
            return Status.Running;
        }
    }

    public class ShowReactionNode : BehaviorNode
    {
        readonly string _reaction;

        public ShowReactionNode(string reaction) => _reaction = reaction;

        public override Status Evaluate(CreatureContext context)
        {
            // Show reaction bubble
            Debug.Log($"[Creature] {context.agent.Definition.displayName}: {_reaction}");
            return Status.Success;
        }
    }

    /// <summary>
    /// Builds behavior trees for different creature types.
    /// </summary>
    public static class CreatureBehaviorTreeBuilder
    {
        public static BehaviorNode BuildCuriousTree()
        {
            var root = new SelectorNode();

            // If player close and has trust, approach
            var approachSequence = new SequenceNode();
            approachSequence.AddChild(new IsPlayerCloseNode(10f));
            approachSequence.AddChild(new HasTrustNode(0.3f));
            approachSequence.AddChild(new MoveToPlayerNode());
            approachSequence.AddChild(new ShowReactionNode("curious"));
            root.AddChild(approachSequence);

            // If player close but no trust, flee
            var fleeSequence = new SequenceNode();
            fleeSequence.AddChild(new IsPlayerCloseNode(5f));
            fleeSequence.AddChild(new InverterNode(new HasTrustNode(0.3f)));
            fleeSequence.AddChild(new FleeFromPlayerNode());
            root.AddChild(fleeSequence);

            // Otherwise wander
            root.AddChild(new WanderNode());

            return root;
        }

        public static BehaviorNode BuildShyTree()
        {
            var root = new SelectorNode();

            // Always flee if player close
            var fleeSequence = new SequenceNode();
            fleeSequence.AddChild(new IsPlayerCloseNode(8f));
            fleeSequence.AddChild(new FleeFromPlayerNode());
            root.AddChild(fleeSequence);

            // Otherwise idle
            root.AddChild(new IdleNode());

            return root;
        }

        public static BehaviorNode BuildGuideTree()
        {
            var root = new SelectorNode();

            // If player far, wait
            var waitSequence = new SequenceNode();
            waitSequence.AddChild(new IsPlayerFarNode(15f));
            waitSequence.AddChild(new IdleNode());
            root.AddChild(waitSequence);

            // If player close, lead
            var leadSequence = new SequenceNode();
            leadSequence.AddChild(new IsPlayerCloseNode(10f));
            leadSequence.AddChild(new MoveToPlayerNode());
            leadSequence.AddChild(new ShowReactionNode("follow me"));
            root.AddChild(leadSequence);

            return root;
        }
    }
}
