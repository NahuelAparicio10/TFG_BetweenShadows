using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace FeedbacksNagu
{
    public class FeedbackPlayAudio : FeedbackBase
    {
        public bool isRandomAudio = false;
        [Range(0f, 1f)] public float volume = 1f;

        public AudioClip audio;

        public AudioClip[] audios;

        public override void Play(GameObject owner)
        {
            if (!active) return;

            AudioSource.PlayClipAtPoint(isRandomAudio ? GetRandomClip() : audio, owner.transform.position, volume);
        }

        private AudioClip GetRandomClip() => audios[Random.Range(0, audios.Length)];

    #if UNITY_EDITOR
            public void PreviewPlayAudio()
            {
                if (!isRandomAudio)
                {
                    AudioUtil.PlayClip(audio);
                }
                else if (audios != null && audios.Length > 0)
                {
                    var randomIndex = Random.Range(0, audios.Length);
                    AudioUtil.PlayClip(audios[randomIndex]);
                }
            }

            // - Helper to play sounds in editor
            private static class AudioUtil
            {
                private static MethodInfo _playClipMethod;

                public static void PlayClip(AudioClip clip)
                {
                    if (clip == null) return;

                    if (_playClipMethod == null)
                    {
                        var unityEditorAssembly = typeof(AudioImporter).Assembly;
                        var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
                        _playClipMethod = audioUtilClass.GetMethod(
                            "PlayClip",
                            BindingFlags.Static | BindingFlags.Public,
                            null,
                            new System.Type[] { typeof(AudioClip) },
                            null
                        );
                    }

                    _playClipMethod?.Invoke(null, new object[] { clip });
                }
            }
    #endif
    }
}