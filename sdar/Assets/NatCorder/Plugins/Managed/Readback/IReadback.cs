/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Internal.Readback {

    using UnityEngine;
    using System;

    public interface IReadback : IDisposable {

        void Readback (Texture texture, Action<IntPtr> handler);
    }
}