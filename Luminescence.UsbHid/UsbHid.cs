using System.Runtime.InteropServices;
using System.Text;

namespace Luminescence.UsbHid;

internal class UsbHid
{
    private const string DllFilename = "UsbHid";

    #region Native Methods

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_init();

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_exit();

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr hid_enumerate(ushort vendor_id, ushort product_id);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern void hid_free_enumeration(IntPtr devs);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr hid_open(ushort vendor_id, ushort product_id, [In] string serial_number);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr hid_open_path([In] string path);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_write(IntPtr device, [In] byte[] data, uint length);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_read_timeout(IntPtr device, [Out] byte[] buf_data, uint length, int milliseconds);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_read(IntPtr device, [Out] byte[] buf_data, uint length);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_set_nonblocking(IntPtr device, int nonblock);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_send_feature_report(IntPtr device, [In] byte[] data, uint length);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_get_feature_report(IntPtr device, [Out] byte[] buf_data, uint length);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl)]
    public static extern void hid_close(IntPtr device);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern int hid_get_manufacturer_string(IntPtr device, StringBuilder buf_string, uint length);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern int hid_get_product_string(IntPtr device, StringBuilder buf_string, uint length);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern int hid_get_serial_number_string(IntPtr device, StringBuilder buf_serial, uint maxlen);

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern int hid_get_indexed_string
    (
        IntPtr device,
        int string_index,
        StringBuilder buf_string,
        uint maxlen
    );

    [DllImport(DllFilename, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern IntPtr hid_error(IntPtr device);

    #endregion
}