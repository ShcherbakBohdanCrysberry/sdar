/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Internal.Readback {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;

    public sealed class MTLReadback : IReadback {
        
        #region --IReadback--

        public MTLReadback () { }

        public void Dispose () {
            using (var dispatcher = new RenderDispatcher())
                dispatcher.Dispatch(() => MTLReadbackDispose(readback));
        }

        public void Readback (Texture texture, Action<IntPtr> handler) {
            var texturePtr = texture.GetNativeTexturePtr();
            if (width != texture.width || height != texture.height) {
                MTLReadbackDispose(readback);
                readback = IntPtr.Zero;
            }
            if (readback == IntPtr.Zero) {
                readback = MTLReadbackCreate(texture.width, texture.height);
                width = texture.width;
                height = texture.height;
            }
            using (var dispatcher = new RenderDispatcher())
                dispatcher.Dispatch(() => MTLReadbackReadback(readback, texturePtr, OnReadback, (IntPtr)GCHandle.Alloc(handler, GCHandleType.Normal)));
        }
        #endregion


        #region --Operations--

        private IntPtr readback;
        private int width, height;

        [MonoPInvokeCallback(typeof(Action<IntPtr, IntPtr>))]
        private static void OnReadback (IntPtr context, IntPtr pixelBuffer) {
            var handle = (GCHandle)context;
            Action<IntPtr> commitFrame = handle.Target as Action<IntPtr>;
            handle.Free();
            commitFrame(pixelBuffer);
        }
        #endregion


        #region --Bridge--

        #if UNITY_IOS && !UNITY_EDITOR
        [DllImport(@"__Internal", EntryPoint = @"NCMTLReadbackCreate")]
        private static extern IntPtr MTLReadbackCreate (int width, int height);
        [DllImport(@"__Internal", EntryPoint = @"NCMTLReadbackDispose")]
        private static extern void MTLReadbackDispose (IntPtr readback);
        [DllImport(@"__Internal", EntryPoint = @"NCMTLReadbackReadback")]
        private static extern void MTLReadbackReadback (IntPtr readback, IntPtr texture, Action<IntPtr, IntPtr> handler, IntPtr context);
        #else
        private static IntPtr MTLReadbackCreate (int width, int height) { return IntPtr.Zero; }
        private static void MTLReadbackDispose (IntPtr readback) {}
        private static void MTLReadbackReadback (IntPtr readback, IntPtr texture, Action<IntPtr, IntPtr> handler, IntPtr context) {}
        #endif
        #endregion
    }
}