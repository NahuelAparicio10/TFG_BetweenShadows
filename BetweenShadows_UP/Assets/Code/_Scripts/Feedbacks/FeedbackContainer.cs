using UnityEngine;
using System.Collections.Generic;
using FeedbacksNagu;

// -- Stores all the feedback to be played in a specific action

public class FeedbackContainer : MonoBehaviour
{
    [SerializeReference] public List<FeedbackBase> feedbacks = new List<FeedbackBase>();
    
    public void PlayFeedbacks()
    {
        foreach (var feedback in feedbacks)
        {
            feedback.Play(gameObject);
        }
    }
}
