/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Examples
{

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;
    using Clocks;
    using Inputs;
    using System;

    public class ReplayCam : MonoBehaviour
    {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using a `CameraRecorder`.
        * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

        [Header("Recording")]
        public int videoWidth;
        public int videoHeight;

        [Header("Microphone")]
        public bool recordMicrophone;
        public AudioSource microphoneSource;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;

        public void StartRecording()
        {
            // Start recording
            videoWidth = Screen.width;
            videoHeight = videoWidth * Screen.height / Screen.width;

            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                30,
                recordMicrophone ? AudioSettings.outputSampleRate : 0,
                recordMicrophone ? (int)AudioSettings.speakerMode : 0,
                OnReplay
            );
            // Create recording inputs
            cameraInput = new CameraInput(videoRecorder, recordingClock, Camera.main);
            if (recordMicrophone)
            {
                StartMicrophone();
                audioInput = new AudioInput(videoRecorder, recordingClock, microphoneSource, true);
            }
        }

        private void StartMicrophone()
        {
#if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // Create a microphone clip
            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
            // Play through audio source
            microphoneSource.timeSamples = Microphone.GetPosition(null);
            microphoneSource.loop = true;
            microphoneSource.Play();
#endif
        }

        public void StopRecording()
        {
            // Stop the recording inputs
            if (recordMicrophone)
            {
                StopMicrophone();
                audioInput.Dispose();
            }
            cameraInput.Dispose();
            // Stop recording
            videoRecorder.Dispose();
        }

        private void StopMicrophone()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            Microphone.End(null);
            microphoneSource.Stop();
#endif
        }

        private void OnReplay(string path)
        {
            Guid g;
            g = Guid.NewGuid(); Debug.Log("Saved recording to: " + path);
#if UNITY_IOS
            NativeGallery.SaveVideoToGallery("file://" + path, "GalleryTest", g +".mp4");
#elif UNITY_ANDROID
            NativeGallery.SaveVideoToGallery(path, "GalleryTest", g +".mp4");
#endif
            // Playback the video
            //         #if UNITY_EDITOR
            //EditorUtility.OpenWithDefaultApp(path);
            //#elif UNITY_IOS
            //Handheld.PlayFullScreenMovie("file://" + path);
            //#elif UNITY_ANDROID
            //Handheld.PlayFullScreenMovie(path);
            //#endif
        }
    }
}