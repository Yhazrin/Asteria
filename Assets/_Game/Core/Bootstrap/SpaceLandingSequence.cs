using UnityEngine;
using System.Collections;

namespace Asteria.Core
{
    /// <summary>
    /// Cinematic space-to-surface landing sequence.
    /// The player starts in orbit, sees the planet below, then descends through
    /// the atmosphere to the surface. This is the "first wow moment."
    /// </summary>
    public sealed class SpaceLandingSequence : MonoBehaviour
    {
        [Header("Orbit")]
        [SerializeField] float orbitAltitude = 800f;
        [SerializeField] float orbitSpeed = 10f;

        [Header("Descent")]
        [SerializeField] float descentDuration = 8f;
        [SerializeField] float atmosphereEntryAltitude = 300f;
        [SerializeField] float surfaceAltitude = 5f;
        [SerializeField] AnimationCurve descentCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Atmosphere")]
        [SerializeField] float atmosphereFadeStart = 400f;
        [SerializeField] float atmosphereFadeEnd = 100f;

        [Header("References")]
        [SerializeField] Camera landingCamera;
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform player;

        // State
        LandingPhase _phase = LandingPhase.WaitingToStart;
        float _timer;
        Vector3 _orbitPosition;
        Vector3 _landingTarget;
        Quaternion _initialRotation;

        // Events
        public event System.Action OnLandingComplete;
        public event System.Action OnAtmosphereEntry;
        public event System.Action OnSurfaceReached;

        public LandingPhase CurrentPhase => _phase;

        void Start()
        {
            if (planet == null) planet = FindFirstObjectByType<PlanetBody>();
            if (landingCamera == null) landingCamera = Camera.main;

            if (planet != null)
            {
                InitializeLanding();
            }
        }

        void Update()
        {
            switch (_phase)
            {
                case LandingPhase.WaitingToStart:
                    break;
                case LandingPhase.InOrbit:
                    UpdateOrbit();
                    break;
                case LandingPhase.Descending:
                    UpdateDescent();
                    break;
                case LandingPhase.AtmosphereEntry:
                    UpdateAtmosphereEntry();
                    break;
                case LandingPhase.SurfaceApproach:
                    UpdateSurfaceApproach();
                    break;
                case LandingPhase.Landed:
                    break;
            }
        }

        /// <summary>
        /// Start the landing sequence.
        /// </summary>
        public void StartLanding(Vector3 targetPosition)
        {
            _landingTarget = targetPosition;
            _phase = LandingPhase.InOrbit;
            _timer = 0f;

            // Position camera in orbit
            Vector3 orbitDir = Random.onUnitSphere;
            _orbitPosition = planet.Center + orbitDir * (planet.Radius + orbitAltitude);

            if (landingCamera != null)
            {
                landingCamera.transform.position = _orbitPosition;
                landingCamera.transform.LookAt(planet.Center);
                _initialRotation = landingCamera.transform.rotation;
            }

            // Disable player control during landing
            if (player != null)
            {
                var motor = player.GetComponent<Player.SphericalMotor>();
                if (motor != null) motor.enabled = false;
            }

            Debug.Log("[Asteria] Landing sequence started.");
        }

        void InitializeLanding()
        {
            // Auto-start landing after a brief delay
            StartCoroutine(AutoStartLanding());
        }

        IEnumerator AutoStartLanding()
        {
            yield return new WaitForSeconds(1f);
            StartLanding(planet.GetPointOnSurface(Vector3.forward, surfaceAltitude));
        }

        void UpdateOrbit()
        {
            _timer += Time.deltaTime;

            // Rotate around planet
            float angle = _timer * orbitSpeed;
            Vector3 orbitDir = Quaternion.AngleAxis(angle, Vector3.up) * (_orbitPosition - planet.Center).normalized;
            _orbitPosition = planet.Center + orbitDir * (planet.Radius + orbitAltitude);

            if (landingCamera != null)
            {
                landingCamera.transform.position = _orbitPosition;
                landingCamera.transform.LookAt(planet.Center);
            }

            // Transition to descent after one orbit or on input
            if (_timer > 5f || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                _phase = LandingPhase.Descending;
                _timer = 0f;
                Debug.Log("[Asteria] Beginning descent.");
            }
        }

        void UpdateDescent()
        {
            _timer += Time.deltaTime;
            float t = descentCurve.Evaluate(Mathf.Clamp01(_timer / descentDuration));

            // Lerp from orbit to surface
            Vector3 startPos = _orbitPosition;
            Vector3 endPos = _landingTarget;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);

            // Keep looking at planet center
            if (landingCamera != null)
            {
                landingCamera.transform.position = currentPos;
                landingCamera.transform.LookAt(planet.Center);
            }

            // Check for atmosphere entry
            float altitude = (currentPos - planet.Center).magnitude - planet.Radius;
            if (altitude < atmosphereEntryAltitude && _phase == LandingPhase.Descending)
            {
                _phase = LandingPhase.AtmosphereEntry;
                OnAtmosphereEntry?.Invoke();
                Debug.Log("[Asteria] Entering atmosphere.");
            }

            // Check for surface approach
            if (altitude < 50f)
            {
                _phase = LandingPhase.SurfaceApproach;
            }

            // Complete
            if (_timer >= descentDuration)
            {
                CompleteLanding();
            }
        }

        void UpdateAtmosphereEntry()
        {
            // Handled by descent - just update atmosphere effects
            float altitude = (landingCamera.transform.position - planet.Center).magnitude - planet.Radius;
            float atmosphereAlpha = 1f - Mathf.InverseLerp(atmosphereFadeEnd, atmosphereFadeStart, altitude);

            // Could trigger atmosphere shader effects here
        }

        void UpdateSurfaceApproach()
        {
            // Final approach - slow down and settle
            _timer += Time.deltaTime;

            if (_timer >= descentDuration)
            {
                CompleteLanding();
            }
        }

        void CompleteLanding()
        {
            _phase = LandingPhase.Landed;

            // Snap camera to player
            if (player != null && landingCamera != null)
            {
                var orbit = landingCamera.GetComponent<Player.SphericalThirdPersonCamera>();
                if (orbit != null)
                {
                    orbit.enabled = true;
                }

                var motor = player.GetComponent<Player.SphericalMotor>();
                if (motor != null) motor.enabled = true;
            }

            OnSurfaceReached?.Invoke();
            OnLandingComplete?.Invoke();

            Debug.Log("[Asteria] Landing complete!");
        }

        public enum LandingPhase
        {
            WaitingToStart,
            InOrbit,
            Descending,
            AtmosphereEntry,
            SurfaceApproach,
            Landed
        }
    }
}
