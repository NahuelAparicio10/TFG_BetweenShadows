using UnityEngine;
using UnityEditor;

namespace FeedbacksNagu
{
    // -- Custom property drawer for FeedbackPlayAudio.
    // - Handles the display of FeedbackPlayAudio properties in the inspector with support for foldout and dynamic layout.

    [CustomPropertyDrawer(typeof(FeedbackPlayAudio))]
    public class FeedbackPlayAudioDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var isRandomAudio = property.FindPropertyRelative("isRandomAudio");
            var active = property.FindPropertyRelative("active");
            var volume = property.FindPropertyRelative("volume");
            var audio = property.FindPropertyRelative("audio");
            var audios = property.FindPropertyRelative("audios");

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                float y = position.y + EditorGUIUtility.singleLineHeight;

                // Toogle

                var toggleActive = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(toggleActive, active);
                y += EditorGUIUtility.singleLineHeight;

                // Toogle

                var toggleRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(toggleRect, isRandomAudio);
                y += EditorGUIUtility.singleLineHeight;

                // Volume Slider
                var volumeRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.Slider(volumeRect, volume, 0f, 1f, new GUIContent("Volume"));
                y += EditorGUIUtility.singleLineHeight;

                // Single audio or Array of Audios
                var fieldRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
                
                if (isRandomAudio.boolValue)
                {
                    EditorGUI.PropertyField(fieldRect, audios, true);
                }
                else
                {
                    EditorGUI.PropertyField(fieldRect, audio);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }
        //Used for get the total height of all the variables in the inspector
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight; // Always at least the foldout

            if (!property.isExpanded) return totalHeight;
            
            totalHeight += EditorGUIUtility.singleLineHeight; 
            totalHeight += EditorGUIUtility.singleLineHeight; 
            totalHeight += EditorGUIUtility.singleLineHeight; 

            var isRandomAudio = property.FindPropertyRelative("isRandomAudio");

            if (isRandomAudio.boolValue)
            {
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("audios"));
            }
            else
            {
                totalHeight += EditorGUIUtility.singleLineHeight; // single audio clip
            }

            return totalHeight;
        }
    }
}
