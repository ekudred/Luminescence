using System.Runtime.InteropServices;

namespace Luminescence.Utils;

public static class StructUtil
{
    public static T BytesToStruct<T>(byte[] data)
        where T : struct
    {
        var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
        T result = (T)Marshal.PtrToStructure(pData.AddrOfPinnedObject(), typeof(T))!;
        pData.Free();

        return result;
    }

    public static byte[] StructToBytes<T>(T data)
        where T : struct
    {
        byte[] result = new byte[Marshal.SizeOf(typeof(T))];
        var pResult = GCHandle.Alloc(result, GCHandleType.Pinned);
        Marshal.StructureToPtr(data, pResult.AddrOfPinnedObject(), true);
        pResult.Free();

        return result;
    }
}