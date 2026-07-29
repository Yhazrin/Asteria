using UnityEngine;

namespace Asteria.Art
{
    /// <summary>
    /// Creates all particle effects procedurally.
    /// Wind, fireflies, aurora, dust, discovery sparkle, etc.
    /// </summary>
    public static class ParticleEffects
    {
        /// <summary>Wind particles flowing across the surface</summary>
        public static ParticleSystem MakeWindParticles(Color? color = null)
        {
            var go = new GameObject("WindParticles");
            var ps = go.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.startLifetime = 3f;
            main.startSpeed = 5f;
            main.startSize = 0.1f;
            main.startColor = color ?? new Color(1f, 1f, 1f, 0.3f);
            main.maxParticles = 200;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 50f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(50f, 1f, 50f);

            var velocity = ps.velocityOverLifetime;
            velocity.enabled = true;
            velocity.x = new ParticleSystem.MinMaxCurve(-3f, 3f);
            velocity.z = new ParticleSystem.MinMaxCurve(5f, 10f);

            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = 0.5f;
            noise.frequency = 0.3f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) },
                new[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(0.5f, 0.3f), new GradientAlphaKey(0, 1) }
            );
            colorOverLifetime.color = gradient;

            return ps;
        }

        /// <summary>Firefly particles for night atmosphere</summary>
        public static ParticleSystem MakeFireflies(Color? color = null)
        {
            var go = new GameObject("Fireflies");
            var ps = go.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.startLifetime = 5f;
            main.startSpeed = 0.5f;
            main.startSize = 0.15f;
            main.startColor = color ?? new Color(0.9f, 0.9f, 0.3f, 0.8f);
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 10f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(30f, 10f, 30f);

            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = 2f;
            noise.frequency = 0.2f;

            var sizeOverLifetime = ps.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0, 0, 0.5f, 1f));

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(new Color(0.9f, 0.9f, 0.3f), 0), new GradientColorKey(new Color(0.3f, 0.9f, 0.3f), 1) },
                new[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1f, 0.2f), new GradientAlphaKey(0, 1) }
            );
            colorOverLifetime.color = gradient;

            return ps;
        }

        /// <summary>Discovery sparkle when player finds something</summary>
        public static ParticleSystem MakeDiscoverySparkle(Color? color = null)
        {
            var go = new GameObject("DiscoverySparkle");
            var ps = go.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.startLifetime = 1.5f;
            main.startSpeed = 3f;
            main.startSize = 0.2f;
            main.startColor = color ?? new Color(1f, 0.9f, 0.3f);
            main.maxParticles = 30;
            main.loop = false;
            main.playOnAwake = true;

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0, 30) });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.5f;

            var sizeOverLifetime = ps.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0, 1, 1, 0));

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(Color.white, 0), new GradientColorKey(new Color(1f, 0.9f, 0.3f), 1) },
                new[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(0, 1) }
            );
            colorOverLifetime.color = gradient;

            return ps;
        }

        /// <summary>Aurora borealis effect</summary>
        public static ParticleSystem MakeAurora()
        {
            var go = new GameObject("Aurora");
            var ps = go.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.startLifetime = 8f;
            main.startSpeed = 0.2f;
            main.startSize = new ParticleSystem.MinMaxCurve(5f, 15f);
            main.startColor = new Color(0.3f, 0.9f, 0.5f, 0.3f);
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 5f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(200f, 1f, 10f);

            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = 3f;
            noise.frequency = 0.1f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[] {
                    new GradientColorKey(new Color(0.3f, 0.9f, 0.5f), 0),
                    new GradientColorKey(new Color(0.3f, 0.5f, 0.9f), 0.5f),
                    new GradientColorKey(new Color(0.7f, 0.3f, 0.9f), 1)
                },
                new[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(0.4f, 0.3f), new GradientAlphaKey(0, 1) }
            );
            colorOverLifetime.color = gradient;

            return ps;
        }

        /// <summary>Dust particles for atmosphere</summary>
        public static ParticleSystem MakeDust(Color? color = null)
        {
            var go = new GameObject("Dust");
            var ps = go.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.startLifetime = 10f;
            main.startSpeed = 0.3f;
            main.startSize = 0.05f;
            main.startColor = color ?? new Color(1f, 1f, 0.9f, 0.2f);
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 10f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(40f, 15f, 40f);

            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = 1f;
            noise.frequency = 0.1f;

            return ps;
        }

        /// <summary>Rain particles</summary>
        public static ParticleSystem MakeRain()
        {
            var go = new GameObject("Rain");
            var ps = go.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.startLifetime = 2f;
            main.startSpeed = 15f;
            main.startSize = 0.05f;
            main.startColor = new Color(0.7f, 0.8f, 0.9f, 0.5f);
            main.maxParticles = 500;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 200f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(50f, 1f, 50f);

            var velocity = ps.velocityOverLifetime;
            velocity.enabled = true;
            velocity.y = -15f;

            return ps;
        }
    }
}
