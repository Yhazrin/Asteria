using System.Collections.Generic;

namespace Asteria.Residents
{
    /// <summary>
    /// Manages relationships between residents. Provides query and modification APIs.
    /// </summary>
    public interface IRelationshipService
    {
        /// <summary>Get the relationship edge between two residents, or null.</summary>
        RelationshipEdge GetEdge(string residentIdA, string residentIdB);

        /// <summary>Get all relationship edges for a resident.</summary>
        IReadOnlyList<RelationshipEdge> GetEdgesFor(string residentId);

        /// <summary>Modify the relationship between two residents.</summary>
        void Modify(string residentIdA, string residentIdB, float affinityDelta, float trustDelta, float tensionDelta);

        /// <summary>Add a tag to a relationship (e.g., "close_friend", "rival").</summary>
        void AddTag(string residentIdA, string residentIdB, string tag);

        /// <summary>Get the relationship status description.</summary>
        string GetStatusDescription(string residentIdA, string residentIdB);
    }

    /// <summary>
    /// Represents the multi-dimensional relationship between two residents.
    /// </summary>
    [System.Serializable]
    public class RelationshipEdge
    {
        public string residentIdA;
        public string residentIdB;
        public float familiarity;
        public float affinity;
        public float trust;
        public float admiration;
        public float tension;
        public string[] tags = { };
        public string[] sharedMemoryIds = { };
        public float lastMeaningfulInteractionTime;
    }
}
