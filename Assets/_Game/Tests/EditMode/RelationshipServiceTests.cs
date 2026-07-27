using Asteria.Residents;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    [TestFixture]
    public class RelationshipServiceTests
    {
        GameObject _go;
        RelationshipService _service;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("TestRelationships");
            _service = _go.AddComponent<RelationshipService>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_go);
        }

        [Test]
        public void GetEdge_ReturnsNull_WhenNoRelationship()
        {
            var edge = _service.GetEdge("a", "b");
            Assert.IsNull(edge);
        }

        [Test]
        public void Modify_CreatesEdge_WhenNoneExists()
        {
            _service.Modify("a", "b", 0.5f, 0.3f, -0.1f);
            var edge = _service.GetEdge("a", "b");

            Assert.IsNotNull(edge);
            Assert.AreEqual(0.5f, edge.affinity, 0.001f);
            Assert.AreEqual(0.3f, edge.trust, 0.001f);
            Assert.AreEqual(-0.1f, edge.tension, 0.001f);
        }

        [Test]
        public void Modify_AccumulatesValues()
        {
            _service.Modify("a", "b", 0.3f, 0.2f, 0.1f);
            _service.Modify("a", "b", 0.2f, 0.1f, -0.05f);

            var edge = _service.GetEdge("a", "b");
            Assert.AreEqual(0.5f, edge.affinity, 0.001f);
            Assert.AreEqual(0.3f, edge.trust, 0.001f);
            Assert.AreEqual(0.05f, edge.tension, 0.001f);
        }

        [Test]
        public void Modify_ClampsToRange()
        {
            _service.Modify("a", "b", 2f, -2f, 0f);
            var edge = _service.GetEdge("a", "b");

            Assert.AreEqual(1f, edge.affinity, 0.001f);
            Assert.AreEqual(-1f, edge.trust, 0.001f);
        }

        [Test]
        public void GetEdge_IsCommutative()
        {
            _service.Modify("a", "b", 0.5f, 0f, 0f);
            var edge1 = _service.GetEdge("a", "b");
            var edge2 = _service.GetEdge("b", "a");

            Assert.AreSame(edge1, edge2);
        }

        [Test]
        public void GetEdgesFor_ReturnsAllEdgesForResident()
        {
            _service.Modify("a", "b", 0.5f, 0f, 0f);
            _service.Modify("a", "c", 0.3f, 0f, 0f);

            var edges = _service.GetEdgesFor("a");
            Assert.AreEqual(2, edges.Count);
        }

        [Test]
        public void AddTag_AddsTagToEdge()
        {
            _service.Modify("a", "b", 0.5f, 0f, 0f);
            _service.AddTag("a", "b", "close_friend");

            var edge = _service.GetEdge("a", "b");
            Assert.Contains("close_friend", edge.tags);
        }

        [Test]
        public void AddTag_NoDuplicate()
        {
            _service.Modify("a", "b", 0.5f, 0f, 0f);
            _service.AddTag("a", "b", "close_friend");
            _service.AddTag("a", "b", "close_friend");

            var edge = _service.GetEdge("a", "b");
            Assert.AreEqual(1, edge.tags.Length);
        }

        [Test]
        public void GetStatusDescription_Unknown_ReturnsStranger()
        {
            string status = _service.GetStatusDescription("a", "b");
            Assert.AreEqual("陌生人", status);
        }

        [Test]
        public void GetStatusDescription_HighAffinity_HighTension()
        {
            _service.Modify("a", "b", 0.8f, 0f, 0.7f);
            string status = _service.GetStatusDescription("a", "b");
            Assert.AreEqual("关系很好但最近闹别扭", status);
        }

        [Test]
        public void GetStatusDescription_HighAffinity_LowTension()
        {
            _service.Modify("a", "b", 0.6f, 0f, 0f);
            string status = _service.GetStatusDescription("a", "b");
            Assert.AreEqual("亲近", status);
        }
    }
}
