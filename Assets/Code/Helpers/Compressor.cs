using System.IO;
using System.IO.Compression;
using UnityEngine;

public static class Compressor
{
    public static byte[] Compress(byte[] data)
    {
        using var output = new MemoryStream();
        using ( var gzip = new GZipStream(output, CompressionMode.Compress, leaveOpen: true))
        {
            gzip.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }

    public static byte[] Decompress(byte[] compressedData)
    {
        using var input = new MemoryStream(compressedData);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);

        return output.ToArray();
    }
}
