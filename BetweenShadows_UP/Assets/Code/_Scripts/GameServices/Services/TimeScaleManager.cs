using UnityEngine;
using System.Collections.Generic;

//-- This manages slow motion requests for feedbacks or whatever we want

public class TimeScaleManager : MonoBehaviour, IGameServices
{
    private class SlowRequest
    {
        public string id;
        public float timeScale;
    }

    private readonly List<SlowRequest> _requests = new();

    public void RequestSlow(string id, float scale)
    {
        _requests.RemoveAll(r => r.id == id);
        _requests.Add(new SlowRequest { id = id, timeScale = scale });
        ApplyTimeScale();
    }
    
    public void ReleaseSlow(string id)
    {
        _requests.RemoveAll(r => r.id == id);
        ApplyTimeScale();
    }

    private void ApplyTimeScale()
    {
        float minScale = 1f;
        foreach (var req in _requests)
        {
            minScale = Mathf.Min(minScale, req.timeScale);
        }

        Time.timeScale = minScale;
        Time.fixedDeltaTime = 0.02f * minScale;
    }
}
