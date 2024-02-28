using System;
using System.Runtime.InteropServices;

namespace Luminescence.Utils;

public static class StructUtil
{
    public static T BytesToStruct<T>(byte[] data)
        where T : struct
    {
        int size = data.Length;
        // Виделяем память и создаем указатель на нее                                         
        IntPtr memory = Marshal.AllocHGlobal(size);
        // Копируем байты в указатель                                                         
        Marshal.Copy(data, 0, memory, size);
        // Передаем неуправляемую память в управляемую структуру                              
        T structure = (T)Marshal.PtrToStructure(memory, typeof(T));
        // Освобождаем память                                                                 
        Marshal.FreeHGlobal(memory);

        return structure;
    }

    public static byte[] StructToBytes<T>(T struc)
        where T : struct
    {
        int size = Marshal.SizeOf(struc);
        // Указатель на память размером с структуру                                           
        IntPtr memory = Marshal.AllocHGlobal(size);
        // Передаем в неуправляемую память из управляемой                                     
        Marshal.StructureToPtr(struc, memory, true);
        // Создаем массив байтов для хранение сериализованной структуры                       
        byte[] buffer = new byte[size];
        // Копируем из указателя в массив байтов                                              
        Marshal.Copy(memory, buffer, 0, size);
        // Освобождаем память                                                                 
        Marshal.FreeHGlobal(memory);

        return buffer;
    }
}