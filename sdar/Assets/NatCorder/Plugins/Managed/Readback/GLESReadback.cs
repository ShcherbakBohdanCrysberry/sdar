/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Internal.Readback {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;

    public sealed class GLESReadback : AndroidJavaProxy, IReadback {

        #region --IReadback--

        public GLESReadback () : base("com.olokobayusuf.natcorder.readback.GLESReadback$Callback") {
            this.readback = new AndroidJavaObject("com.olokobayusuf.natcorder.readback.GLESReadback", this);
            this.Unmanaged = new AndroidJavaClass(@"com.olokobayusuf.natrender.Unmanaged");
            using (var dispatcher = new RenderDispatcher())
                dispatcher.Dispatch(() => AndroidJNI.AttachCurrentThread());
        }
        
        public void Dispose () {
            readback.Call(@"release");
            readback.Dispose();
            Unmanaged.Dispose();
        }

        public void Readback (Texture texture, Action<IntPtr> handler) {
            this.handler = handler;
            var texturePtr = texture.GetNativeTexturePtr();
            var width = texture.width;
            var height = texture.height;
            GL.Flush();
            using (var dispatcher = new RenderDispatcher())
                dispatcher.Dispatch(() => readback.Call("readback", (int)texturePtr, width, height));
        }
        #endregion


        #region --Operations--

        private readonly AndroidJavaObject readback;
        private readonly AndroidJavaClass Unmanaged;
        private Action<IntPtr> handler;

        [Preserve]
        private void onReadback (AndroidJavaObject nativeBuffer, int width, int height) {
            var pixelBuffer = (IntPtr)Unmanaged.CallStatic<long>(@"baseAddress", nativeBuffer);
            nativeBuffer.Dispose();
            handler(pixelBuffer);
        }
        #endregion
    }
}