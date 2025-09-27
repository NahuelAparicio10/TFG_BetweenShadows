using UnityEngine;

namespace FeedbacksNagu
{
    public class FeedbackCamaraShake : FeedbackBase
    {
        public float intensity = 2f;
        public float duration = 0.5f;
        public float frequency = 10f;

        private Vector3 _originalPosition;
        //private bool _isShaking = false;

        //TODO

        public override void Play(GameObject owner)
        {
            throw new System.NotImplementedException();
        }
    }
}

