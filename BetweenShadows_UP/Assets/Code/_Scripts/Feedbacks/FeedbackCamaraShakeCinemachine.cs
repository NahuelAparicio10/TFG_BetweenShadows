using UnityEngine;
using Unity.Cinemachine;

namespace FeedbacksNagu
{
    public class FeedbackCamaraShakeCinemachine : FeedbackBase
    {
        public float intensity = 2f;
        public float duration = 0.5f;
        public float frequency = 10f;

        public CinemachineImpulseDefinition.ImpulseShapes impulseShape;
        public CinemachineImpulseDefinition.ImpulseTypes impulseType;

        public CinemachineImpulseSource impulseSource;

        public override void Play(GameObject owner)
        {
            if (!active || impulseSource == null) return;

            var newImpulseDefinition = new CinemachineImpulseDefinition
            {
                ImpulseDuration = duration,
                AmplitudeGain = intensity,
                FrequencyGain = frequency,
                ImpulseShape = impulseShape,
                ImpulseType = impulseType
            };

            impulseSource.ImpulseDefinition = newImpulseDefinition;

            impulseSource.GenerateImpulse();
        }
    }
}

