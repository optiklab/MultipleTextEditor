using System;

namespace MultipleTextEditor
{
    internal sealed class SimpleEditException : Exception
    {
        public SimpleEditException() { }
        public SimpleEditException(string message) : base(message) { }
    }
}
