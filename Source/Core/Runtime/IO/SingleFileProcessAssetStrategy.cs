#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using System;
using System.Collections.Generic;
using VRBuilder.Core.Serialization;

namespace VRBuilder.Core.IO
{
    /// <summary>
    /// Process asset strategy that saves the process as a single file.
    /// </summary>
    public class SingleFileProcessAssetStrategy : IProcessAssetStrategy
    {
        /// <inheritdoc/>
        public bool CreateManifest => false;

        /// <inheritdoc/>
        public IDictionary<string, byte[]> CreateSerializedProcessAssets(IProcess process, IProcessSerializer serializer)
        {
            return new Dictionary<string, byte[]>
            {
                { process.Data.Name, serializer.ProcessToByteArray(process) }
            };
        }

        /// <inheritdoc/>
        public IProcess GetProcessFromSerializedData(byte[] processData, IEnumerable<byte[]> additionalData, IProcessSerializer serializer)
        {
            try
            {
                return serializer.ProcessFromByteArray(processData);
            }
            catch (Exception ex)
            {
#if UNITY_5_3_OR_NEWER
                Debug.LogError(ex.Message);
#elif GODOT
            GD.PushError(ex);
#endif
            }

            return null;
        }
    }
}
