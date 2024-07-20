// From https://github.com/LagrangeDev/Lagrange.Core/blob/master/Lagrange.Core/Utility/Extension/ByteExtension.cs

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DXKumaBot.Utils;

internal static class ByteExtension
{
    public static string Hex(this byte[] bytes, bool lower = false, bool space = false)
    {
        return Hex(bytes.AsSpan(), lower, space);
    }

    public static string Hex(this Span<byte> bytes, bool lower = true, bool space = false)
    {
        return Hex((ReadOnlySpan<byte>)bytes, lower, space);
    }

    public static string Hex(this ReadOnlySpan<byte> bytes, bool lower = true, bool space = false)
    {
        return space
            ? HexInternal<WithSpaceHexByteStruct>(bytes, lower)
            : HexInternal<NoSpaceHexByteStruct>(bytes, lower);
    }

    private static string HexInternal<TStruct>(ReadOnlySpan<byte> bytes, bool lower)
        where TStruct : struct, IHexByteStruct
    {
        if (bytes.Length is 0)
        {
            return string.Empty;
        }

        uint casing = lower ? 0x200020u : 0;
        int structSize = Marshal.SizeOf<TStruct>();
        if (structSize % 2 is 1)
        {
            throw new ArgumentException($"{nameof(TStruct)}'s size of must be a multiple of 2, currently {structSize}");
        }

        int charCountPerByte = structSize / 2; // 2 is the size of char
        string result = new('\0', bytes.Length * charCountPerByte);
        Span<TStruct> resultSpan =
            MemoryMarshal.CreateSpan(ref Unsafe.As<char, TStruct>(ref Unsafe.AsRef(in result.GetPinnableReference())),
                bytes.Length);

        for (int i = 0; i < bytes.Length; i++)
        {
            resultSpan[i].Write(ToCharsBuffer(bytes[i], casing));
        }

        return result;
    }

    // By Executor-Cheng https://github.com/KonataDev/Lagrange.Core/pull/344#pullrequestreview-2027515322
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ToCharsBuffer(byte value, uint casing = 0)
    {
        uint difference = BitConverter.IsLittleEndian
            ? ((uint)value >> 4) + ((value & 0x0Fu) << 16) - 0x890089u
            : ((value & 0xF0u) << 12) + (value & 0x0Fu) - 0x890089u;
        uint packedResult = (((uint)-(int)difference & 0x700070u) >> 4) + difference + 0xB900B9u | casing;
        return packedResult;
    }

    private interface IHexByteStruct
    {
        void Write(uint hexChar);
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 6)]
    private struct WithSpaceHexByteStruct : IHexByteStruct
    {
        [FieldOffset(0)] public uint twoChar;
        [FieldOffset(4)] public char space;

        public void Write(uint hexChar)
        {
            twoChar = hexChar;
            space = ' ';
        }
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 4)]
    private struct NoSpaceHexByteStruct : IHexByteStruct
    {
        [FieldOffset(0)] public uint twoChar;

        public void Write(uint hexChar)
        {
            twoChar = hexChar;
        }
    }
}