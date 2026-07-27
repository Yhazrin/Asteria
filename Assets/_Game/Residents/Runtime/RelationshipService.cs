using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Default implementation of IRelationshipService.
    /// Manages relationships between residents in memory.
    /// </summary>
    public sealed class RelationshipService : MonoBehaviour, IRelationshipService
    {
        readonly List<RelationshipEdge> _edges = new();

        public RelationshipEdge GetEdge(string residentIdA, string residentIdB)
        {
            return _edges.FirstOrDefault(e =>
                (e.residentIdA == residentIdA && e.residentIdB == residentIdB) ||
                (e.residentIdA == residentIdB && e.residentIdB == residentIdA));
        }

        public IReadOnlyList<RelationshipEdge> GetEdgesFor(string residentId)
        {
            return _edges.Where(e => e.residentIdA == residentId || e.residentIdB == residentId)
                .ToList().AsReadOnly();
        }

        public void Modify(string residentIdA, string residentIdB,
            float affinityDelta, float trustDelta, float tensionDelta)
        {
            var edge = GetOrCreateEdge(residentIdA, residentIdB);
            edge.affinity = Mathf.Clamp(edge.affinity + affinityDelta, -1f, 1f);
            edge.trust = Mathf.Clamp(edge.trust + trustDelta, -1f, 1f);
            edge.tension = Mathf.Clamp(edge.tension + tensionDelta, -1f, 1f);
            edge.lastMeaningfulInteractionTime = Time.time;
        }

        public void AddTag(string residentIdA, string residentIdB, string tag)
        {
            var edge = GetOrCreateEdge(residentIdA, residentIdB);
            if (!edge.tags.Contains(tag))
            {
                var tags = new List<string>(edge.tags) { tag };
                edge.tags = tags.ToArray();
            }
        }

        public string GetStatusDescription(string residentIdA, string residentIdB)
        {
            var edge = GetEdge(residentIdA, residentIdB);
            if (edge == null)
            {
                return "陌生人";
            }

            if (edge.affinity > 0.5f && edge.tension > 0.5f)
            {
                return "关系很好但最近闹别扭";
            }

            if (edge.trust > 0.7f && edge.familiarity < 0.3f)
            {
                return "稳定老朋友";
            }

            if (edge.affinity > 0.5f)
            {
                return "亲近";
            }

            if (edge.tension > 0.5f)
            {
                return "紧张";
            }

            return "认识";
        }

        RelationshipEdge GetOrCreateEdge(string residentIdA, string residentIdB)
        {
            var edge = GetEdge(residentIdA, residentIdB);
            if (edge == null)
            {
                edge = new RelationshipEdge
                {
                    residentIdA = residentIdA,
                    residentIdB = residentIdB
                };
                _edges.Add(edge);
            }

            return edge;
        }
    }
}
