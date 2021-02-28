namespace Assets.CBC.Scripts.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Windows.Speech;

    public class VoiceControlUnity : EditorWindow
    {

        private Dictionary<string, Action> _voiceCommands = new Dictionary<string, Action>();

        private KeywordRecognizer _keywordRecognizer;
        private string message = string.Empty;

        [MenuItem("Tools/UnityVoiceControl")]
        static void CreateWindow()
        {
            Debug.Log("CreateWindow");
            const bool IsFloatingWindow = false;
            const string WindowTitle = "Voice Control";
            var window = GetWindow<VoiceControlUnity>(utility: IsFloatingWindow, title: WindowTitle, focus: true);
            SubscribeToPhaseRecognitionSystem();
            window.Initialise();

        }

        private static void SubscribeToPhaseRecognitionSystem()
        {
            PhraseRecognitionSystem.OnError += PhraseRecognitionSystem_OnError;
            PhraseRecognitionSystem.OnStatusChanged += PhraseRecognitionSystem_OnStatusChanged;
        }

        private static void PhraseRecognitionSystem_OnStatusChanged(SpeechSystemStatus status)
        {
            Debug.Log($"PhraseRecognitionSystem_OnStatusChanged {status}");
        }

        private static void PhraseRecognitionSystem_OnError(SpeechError errorCode)
        {
            Debug.Log($"PhraseRecognitionSystem_OnError {errorCode}");
        }

        private void Initialise()
        {
            AddVoiceCommands();
            SubscribeToRecognizer();

            SubscribeToEditorApplication();
            Debug.Log("Recognizer initialised");
        }

        private static void SubscribeToEditorApplication()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
        }

        private void SubscribeToRecognizer()
        {
            _keywordRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
            _keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        }

        private void AddVoiceCommands()
        {
            _voiceCommands.Add("Play", Play);
            _voiceCommands.Add("Stop", Stop);
            _keywordRecognizer = new KeywordRecognizer(_voiceCommands.Keys.ToArray());
        }

        void OnDestroy()
        {
            Debug.Log("VoiceControlUnity::OnDestroy");
            if (_keywordRecognizer != null)
            {
                _keywordRecognizer.Dispose();
            }
        }

        private void OnSelectionChange()
        {
            Debug.Log("OnSelectionChange");
        }

        private void OnProjectChange()
        {
            Debug.Log("OnProjectChange");
        }

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            Debug.Log($"EditorApplication_PlayModeStateChanged {playModeStateChange}");
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.Log("Command: " + args.text);
            _voiceCommands[args.text].Invoke();
        }

        private void Play()
        {
            message = $"{DateTime.Now.ToLongTimeString()} Play";
            EditorApplication.EnterPlaymode();
        }

        private void Stop()
        {
            message = $"{DateTime.Now.ToLongTimeString()} Stop";
            EditorApplication.ExitPlaymode();
        }

        private void OnGUI()
        {
            var isRunningMessage = $"Is Listening {(_keywordRecognizer == null ? "null" : _keywordRecognizer.IsRunning.ToString())}";

            // HACK: My lack of understanding of Editor means when we go into Play mode this editor loses its instance data. So this restablishes the recognizer (???)
            if (_keywordRecognizer == null)
            {
                Initialise();
                _keywordRecognizer.Start();
            }


            GUILayout.Label(isRunningMessage);
            GUILayout.Label(message);

            if (GUILayout.Button("Listen"))
            {
                Listen();
            }

            if (GUILayout.Button("Stop Listening"))
            {
                StopListening();
            }
        }

        private void StopListening()
        {
            _keywordRecognizer.Stop();
        }

        private void Listen()
        {
            if (_keywordRecognizer == null)
            {
                Initialise();
            }

            _keywordRecognizer.Start();
        }
    }
}