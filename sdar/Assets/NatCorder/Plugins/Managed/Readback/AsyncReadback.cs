/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Internal.Readback {

    using UnityEngine;
    using UnityEngine.Rendering;
    using System;
    using System.Runtime.InteropServices;

    public sealed class AsyncReadback : IReadback {
        
        #region --IReadback--

        public AsyncReadback () { }

        public void Dispose () {
            pixelBuffer = null;
        }

        public void Readback (Texture texture, Action<IntPtr> handler) {
            AsyncGPUReadback.Request(texture, 0, request => {
                pixelBuffer = pixelBuffer != null && pixelBuffer.Length != texture.width * texture.height * 4 ? null : pixelBuffer;
                pixelBuffer = pixelBuffer ?? new byte[texture.width * texture.height * 4];
                request.GetData<byte>().CopyTo(pixelBuffer);
                var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
                handler(handle.AddrOfPinnedObject());
                handle.Free();
            });
        }
        #endregion

        private byte[] pixelBuffer;
    }
}