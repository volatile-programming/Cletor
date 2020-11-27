using System.IO;
using System.Runtime.InteropServices;

namespace Cletor.Views.Helpers
{
    public static class StreamExtensions
    {
        public static void Write<T>(this Stream stream, T value)
            where T : unmanaged
        {
            var tSpan = MemoryMarshal.CreateSpan(ref value, 1);
            var span = MemoryMarshal.AsBytes(tSpan);
            stream.Write(span);
        }

        public static T Read<T>(this Stream stream)
            where T : unmanaged
        {
            var result = default(T);
            var tSpan = MemoryMarshal.CreateSpan(ref result, 1);
            var span = MemoryMarshal.AsBytes(tSpan);
            stream.Read(span);
            return result;
        }
    }
}
