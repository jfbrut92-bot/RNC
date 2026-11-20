using System;
using System.Diagnostics;

namespace Utac.Rnc2.Infrastructure.types
{
    [Serializable]
    public class Image
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] _internalBytes;

        public byte[] InternalBytes
        {
            get { return _internalBytes; }
            set { _internalBytes = value; }
        }

        public Image(byte[] b)
        {
            this._internalBytes = b;
        }
    }
}
