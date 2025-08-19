using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FeedbacksNagu
{
    // -- Custom Editor for FeedbackContainer
    // - Allows the user to add specific feedback types to the feedbacks list through a contextual menu.
    // editor reflection asi no se hardcodea
    [CustomEditor(typeof(FeedbackContainer))]
    public class FeedbacksEditor : Editor
    {
        private List<Type> _feedbacksTypes;

        private void OnEnable()
        {
            _feedbacksTypes = AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(asm => asm.GetTypes())
       .Where(t => typeof(FeedbackBase).IsAssignableFrom(t) && !t.IsAbstract)
       .ToList();
        }

        public override void OnInspectorGUI()
        {
            var container = (FeedbackContainer)target;

            if (GUILayout.Button("Add Feedback"))
            {
                var menu = new GenericMenu();

                foreach (var type in _feedbacksTypes)
                {
                    var menuLabel = ObjectNames.NicifyVariableName(type.Name);
                    menu.AddItem(new GUIContent(menuLabel), false, () => AddFeedback(container, type));
                }
                //menu.AddItem(new GUIContent("Play Sound"), false, () => AddFeedback(container, new FeedbackPlayAudio()));
                //menu.AddItem(new GUIContent("Slow Motion"), false, () => AddFeedback(container, new FeedbackSlowMotion()));
                //menu.AddItem(new GUIContent("Camera Shake Cinemachine"), false, () => AddFeedback(container, new FeedbackCamaraShakeCinemachine())) ;

                menu.ShowAsContext();
            }

            DrawDefaultInspector();
        }

        // Adds a new feedback instance to the container
        
        private void AddFeedback(FeedbackContainer container, Type feedbackType)
        {
            var newFeedback = (FeedbackBase)Activator.CreateInstance(feedbackType);

            Undo.RecordObject(container, "Add Feedback");
            container.feedbacks.Add(newFeedback);
            EditorUtility.SetDirty(container);
        }


    }
}



