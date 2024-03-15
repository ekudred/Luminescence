using System;
using System.Runtime.InteropServices;

namespace Luminescence.Utils;

public static class StructUtil
{
    public static T BytesToStruct<T>(byte[] data)
        where T : struct
    {
        int size = data.Length;
        IntPtr memory = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, memory, size);
        T structure = (T)Marshal.PtrToStructure(memory, typeof(T));
        Marshal.FreeHGlobal(memory);

        return structure;
    }

    public static byte[] StructToBytes<T>(T struc)
        where T : struct
    {
        int size = Marshal.SizeOf(struc);
        IntPtr memory = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(struc, memory, true);
        byte[] buffer = new byte[size];
        Marshal.Copy(memory, buffer, 0, size);
        Marshal.FreeHGlobal(memory);

        return buffer;
    }
}