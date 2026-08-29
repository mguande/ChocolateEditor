using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

// Magic from HTUSBTools.dll: ASCII "!%~GP-2"
byte[] Magic = [0x21, 0x25, 0x7E, 0x47, 0x50, 0x2D, 0x32];
const int FlagRsp = 0x11;
const int SoftwareState = unchecked((int)0x12000000);
const int PresetSignal = unchecked((int)0x10000001);
const int PresetList = 0x010A0000;

static byte[] Be(int v) => [(byte)(v >> 24), (byte)(v >> 16), (byte)(v >> 8), (byte)v];
static byte[] Le(int v) => [(byte)v, (byte)(v >> 8), (byte)(v >> 16), (byte)(v >> 24)];
static string Hex(byte[] d) => string.Join(" ", d.Select(b => b.ToString("X2")));

byte[] Packet(int addr, byte[] payload)
{
    return [.. Magic, (byte)FlagRsp, .. Be(addr), .. payload];
}

var input = InputDevice.GetAll().First(d => d.Name.Contains("Matribox", StringComparison.OrdinalIgnoreCase));
var output = OutputDevice.GetAll().First(d => d.Name.Contains("Matribox", StringComparison.OrdinalIgnoreCase));
Console.WriteLine($"IN={input.Name} OUT={output.Name}");

input.EventReceived += (_, e) =>
{
    if (e.Event is SysExEvent sx)
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] SYSEX {Hex(sx.Data ?? [])}");
    else
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {e.Event}");
};
input.StartEventsListening();

void Tx(byte[] inner)
{
    var p = new byte[inner.Length + 1];
    Buffer.BlockCopy(inner, 0, p, 0, inner.Length);
    p[^1] = 0xF7;
    Console.WriteLine("TX " + Hex(inner));
    output.SendEvent(new NormalSysExEvent(p));
}

Tx(Packet(SoftwareState, []));
Thread.Sleep(400);
Tx(Packet(SoftwareState, []));
Thread.Sleep(400);
Tx(Packet(PresetSignal, Le(PresetList)));
Thread.Sleep(400);
Tx(Packet(PresetSignal, Be(PresetList)));
Console.WriteLine("listening 6s — pisa num switch na Matribox se quiseres");
Thread.Sleep(6000);
input.Dispose();
output.Dispose();
