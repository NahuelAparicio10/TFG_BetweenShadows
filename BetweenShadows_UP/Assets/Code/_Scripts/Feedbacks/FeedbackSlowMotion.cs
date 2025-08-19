using System.Collections;
using UnityEngine;

namespace FeedbacksNagu
{
    public class FeedbackSlowMotion : FeedbackBase
    {
        public float slowFactor;
        public float slowDuration;

        private Coroutine _activeCoroutine;
        private const string SLOW_ID = "FeedbackSlowMotion";
        public override void Play(GameObject owner)
        {
            if (!active) return;

            var runner = GameServices.Get<CoroutineRunner>();
            var timeManager = GameServices.Get<TimeScaleManager>();
            
            if(_activeCoroutine != null)
            {
                runner.StopCoroutine(_activeCoroutine);
                timeManager.ReleaseSlow(SLOW_ID);
            }

            _activeCoroutine = runner.StartCoroutine(SlowMotionCoroutine(timeManager));

        }

        private IEnumerator SlowMotionCoroutine(TimeScaleManager timeManager)
        {
            timeManager.RequestSlow(SLOW_ID, slowFactor);
            yield return new WaitForSecondsRealtime(slowDuration);
            timeManager.ReleaseSlow(SLOW_ID);
            _activeCoroutine = null;
        }
    }


}
