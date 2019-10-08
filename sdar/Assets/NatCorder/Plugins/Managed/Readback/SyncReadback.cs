/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Internal.Readback {

    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;

    public sealed class SyncReadback : IReadback {

        #region --IReadback--

        public SyncReadback () { }
        
        public void Dispose () {
            Texture2D.Destroy(readbackBuffer);
            readbackBuffer = null;
            pixelBuffer = null;
        }

        public void Readback (Texture texture, Action<IntPtr> handler) {
            if (texture is RenderTexture)
                Readback(texture as RenderTexture, handler);
            else { // INCOMPLETE
                
            }
        }
        #endregion


        #region --Operations--

        private Texture2D readbackBuffer;
        private byte[] pixelBuffer;

        private void Readback (RenderTexture texture, Action<IntPtr> handler) {
            // Check readback buffer
            readbackBuffer = readbackBuffer ?? new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false);
            if (readbackBuffer && (readbackBuffer.width != texture.width || readbackBuffer.height != texture.height))
                readbackBuffer.Resize(texture.width, texture.height);
            // Check pixel buffer
            pixelBuffer = pixelBuffer != null && pixelBuffer.Length != texture.width * texture.height * 4 ? null : pixelBuffer;
            pixelBuffer = pixelBuffer ?? new byte[texture.width * texture.height * 4];
            // Readback
            RenderTexture.active = texture;
            readbackBuffer.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0, false);
            readbackBuffer.GetRawTextureData<byte>().CopyTo(pixelBuffer);
            // Invoke handler
            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            handler(handle.AddrOfPinnedObject());
            handle.Free();
        }
        #endregion
    }
}