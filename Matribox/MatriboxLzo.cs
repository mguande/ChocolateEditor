using System.Runtime.InteropServices;

namespace ChocolateEditor.Matribox;

/// <summary>
/// Descomprime .prst com a mesma DLL do editor Sonicake (toolStartDecompress).
/// </summary>
internal static class MatriboxLzo
{
    private static readonly string[] DllPaths =
    [
        @"C:\Program Files\Sonicake\Matribox II PRO\assets\lib\HTUSBTools.dll",
        @"C:\Program Files\Sonicake\Matribox II PRO\data\flutter_assets\assets\lib\HTUSBTools.dll"
    ];

    private static DecompressFn? _decompress;
    private static bool _tried;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate int DecompressFn(IntPtr input, int size, IntPtr output);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool SetDllDirectory(string? lpPathName);

    public static bool TryDecompress(byte[] compressed, out byte[] raw)
    {
        raw = [];
        if (compressed.Length == 0 || !Ensure())
        {
            return false;
        }

        byte[] dest = new byte[65536];
        var pinIn = GCHandle.Alloc(compressed, GCHandleType.Pinned);
        var pinOut = GCHandle.Alloc(dest, GCHandleType.Pinned);
        try
        {
            int n = _decompress!(pinIn.AddrOfPinnedObject(), compressed.Length, pinOut.AddrOfPinnedObject());
            if (n is <= 0 or > 60000)
            {
                return false;
            }

            raw = dest[..n];
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            pinIn.Free();
            pinOut.Free();
        }
    }

    private static bool Ensure()
    {
        if (_decompress is not null)
        {
            return true;
        }

        if (_tried)
        {
            return false;
        }

        _tried = true;
        string? path = DllPaths.FirstOrDefault(File.Exists);
        if (path is null)
        {
            return false;
        }

        try
        {
            SetDllDirectory(Path.GetDirectoryName(path));
            var lib = NativeLibrary.Load(path);
            _decompress = Marshal.GetDelegateForFunctionPointer<DecompressFn>(
                NativeLibrary.GetExport(lib, "toolStartDecompress"));
            return true;
        }
        catch
        {
            return false;
        }
    }
}
