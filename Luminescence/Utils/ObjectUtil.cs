using System;

namespace Luminescence.Utils;

public static class ObjectUtil
{
    public static bool Implements<TInterface>(this object obj)
    {
        unsafe
        {
            IntPtr iMTPointer = typeof(TInterface).TypeHandle.Value;

            TypedReference trObj = __makeref(obj);
            IntPtr ptrObj = **(IntPtr**)&trObj;

            void* methodTable = (*(IntPtr*)ptrObj.ToPointer()).ToPointer();
            IntPtr* interfaces = (IntPtr*)((IntPtr*)methodTable)[9].ToPointer();
            int count = ((ushort*)methodTable)[7];

            for (int i = 0; i < count; ++interfaces, ++i)
            {
                if (*interfaces == iMTPointer)
                {
                    return true;
                }
            }

            return false;
        }
    }
}