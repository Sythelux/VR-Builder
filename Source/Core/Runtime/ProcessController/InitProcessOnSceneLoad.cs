using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VRBuilder.Core;
using VRBuilder.Core.Configuration;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.UX
{
    /// <summary>
    /// Initializes the <see cref="ProcessRunner"/> with the current selected process on scene start.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class InitProcessOnSceneLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(InitProcess());
        }
#elif GODOT
    public partial class InitProcessOnSceneLoad : Node
    {
        private void _Ready()
        {
            StartCoroutine(InitProcess());
        }

        private void StartCoroutine(IEnumerator initProcess)
        {
            //TODO
        }
#endif
        private IEnumerator InitProcess()
        {
            // Load process from a file.
            string processPath = RuntimeConfigurator.Instance.GetSelectedProcess();

            IProcess process;

            // Try to load the in the PROCESS_CONFIGURATION selected process.

            Task<IProcess> loadProcess = RuntimeConfigurator.Configuration.LoadProcess(processPath);
            while (!loadProcess.IsCompleted)
            {
                yield return null;
            }

            process = loadProcess.Result;


            // Initializes the process. That will synthesize an audio for the instructions, too.
            ProcessRunner.Initialize(process);
        }
    }
}