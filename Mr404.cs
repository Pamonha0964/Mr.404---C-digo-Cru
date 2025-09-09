using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Media;
using Microsoft.Win32;
using System.Text;

class SingleBeatPlayer
{
    private const int SampleRate = 8000; // 8kHz
    private const int Duration = 200; // seconds
    private const int BitsPerSample = 8;
    private const int Channels = 1; // mono
    private const double Frequency = 220.0; // A4 note (440 Hz)

    // Main function - can be called without blocking other functions
    public static async Task PlaySingleBeatAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Single Beat Player - Generating audio...");
                Console.WriteLine("Frequency: " + Frequency + "Hz");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate single frequency audio data
                byte[] audioData = GenerateSingleBeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Single beat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });
    }

    // Synchronous version that doesn't block (plays in background)
    public static void PlaySingleBeatNonBlocking()
    {
        Thread audioThread = new Thread(() =>
        {
            try
            {
                Console.WriteLine("Single Beat Player - Generating audio in background...");
                Console.WriteLine("Frequency: " + Frequency + "Hz");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate single frequency audio data
                byte[] audioData = GenerateSingleBeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Single beat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });

        audioThread.IsBackground = true; // Won't prevent app from closing
        audioThread.Start();
    }

    static byte[] GenerateSingleBeatAudio()
    {
        int totalSamples = SampleRate * Duration;
        byte[] samples = new byte[totalSamples];

        Console.WriteLine("Generating samples...");

        for (int t = 0; t < totalSamples; t++)
        {
            // Generate a simple sine wave at the specified frequency
            double time = (double)t / SampleRate;
            double sineWave = Math.Sin(2 * Math.PI * Frequency * time);
            
            // Convert to 8-bit unsigned sample (0-255)
            // Scale sine wave (-1 to 1) to byte range (0-255)
            int sample = (int)((sineWave + 1.0) * 127.5);
            samples[t] = (byte)Math.Max(0, Math.Min(255, sample));

            // Progress indicator
            if (t % (totalSamples / 10) == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine("\nSamples generated!");
        return samples;
    }

    // Alternative: Simple bytebeat-style single frequency
    static byte[] GenerateBytebeatSingleBeat()
    {
        int totalSamples = SampleRate * Duration;
        byte[] samples = new byte[totalSamples];

        Console.WriteLine("Generating bytebeat-style single beat...");

        for (int t = 0; t < totalSamples; t++)
        {
            // Simple bytebeat formula for a single tone
            // This creates a square wave-like tone
            int beatFreq = 55; // Adjust this value to change the frequency
            int sample = ((t * beatFreq) >> 8) & 255;
            
            samples[t] = (byte)sample;

            // Progress indicator
            if (t % (totalSamples / 10) == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine("\nBytebeat samples generated!");
        return samples;
    }

    static byte[] CreateWavFile(byte[] audioData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                // WAV Header
                writer.Write("RIFF".ToCharArray());
                writer.Write((uint)(36 + audioData.Length)); // File size
                writer.Write("WAVE".ToCharArray());

                // Format chunk
                writer.Write("fmt ".ToCharArray());
                writer.Write((uint)16); // Chunk size
                writer.Write((ushort)1); // Audio format (PCM)
                writer.Write((ushort)Channels); // Number of channels
                writer.Write((uint)SampleRate); // Sample rate
                writer.Write((uint)(SampleRate * Channels * BitsPerSample / 8)); // Byte rate
                writer.Write((ushort)(Channels * BitsPerSample / 8)); // Block align
                writer.Write((ushort)BitsPerSample); // Bits per sample

                // Data chunk
                writer.Write("data".ToCharArray());
                writer.Write((uint)audioData.Length); // Data size
                writer.Write(audioData); // Audio data
            }

            return ms.ToArray();
        }
    }

    static void PlayAudio(byte[] wavData)
    {
        Console.WriteLine("Playing audio...");

        // Save to temporary file
        string tempFile = Path.GetTempFileName() + ".wav";
        File.WriteAllBytes(tempFile, wavData);

        try
        {
            // Play using SoundPlayer
            using (SoundPlayer player = new SoundPlayer(tempFile))
            {
                player.PlaySync();
            }
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
class BytebeatPlayer_glitch
{
    private const int SampleRate = 10000; // 8kHz
    private const int Duration = 23; // seconds
    private const int BitsPerSample = 8;
    private const int Channels = 1; // mono

    // Main function - can be called without blocking other functions
    public static async Task PlayBytebeatAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio...");
                Console.WriteLine("Formula: i=t&8191, (((t*((t>>9^((t>>9)-1)^1)%13)&255)/2)+((((t>>3it<<(t>>12&2))*(i<4096)+(t>>4it*(t^t+t/256))*(i>4095)))&255)/2)*(2+(t>>16))");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });
    }

    // Synchronous version that doesn't block (plays in background)
    public static void PlayBytebeatNonBlocking()
    {
        Thread audioThread = new Thread(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio in background...");
                Console.WriteLine("Formula: i=t&8191, (((t*((t>>9^((t>>9)-1)^1)%13)&255)/2)+((((t>>3it<<(t>>12&2))*(i<4096)+(t>>4it*(t^t+t/256))*(i>4095)))&255)/2)*(2+(t>>16))");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });

        audioThread.IsBackground = true; // Won't prevent app from closing
        audioThread.Start();
    }

    static byte[] GenerateBytebeatAudio()
    {
        int totalSamples = SampleRate * Duration;
        byte[] samples = new byte[totalSamples];

        Console.WriteLine("Generating samples...");

        for (int t = 0; t < totalSamples; t++)
        {
            // Bytebeat formula: i=t&8191, (((t*((t>>9^((t>>9)-1)^1)%13)&255)/2)+((((t>>3it<<(t>>12&2))*(i<4096)+(t>>4it*(t^t+t/256))*(i>4095)))&255)/2)*(2+(t>>16))
            int i = t & 8191;

            // First part: ((t*((t>>9^((t>>9)-1)^1)%13)&255)/2)
            int part1 = ((t * ((t >> 9 ^ ((t >> 9) - 1) ^ 1) % 13)) & 255) / 2;

            // Second part with conditional logic based on i
            int conditionalPart;
            if (i < 4096)
            {
                conditionalPart = (t >> 3 & t << (t >> 12 & 2));
            }
            else
            {
                conditionalPart = (t >> 4 & t * (t ^ t + t / 256));
            }

            int part2 = (conditionalPart & 255) / 2;

            // Final result
            int result = (part1 + part2) * (2 + (t >> 16));

            // Convert to 8-bit unsigned sample (0-255)
            samples[t] = (byte)(result & 0xFF);

            // Progress indicator
            if (t % (totalSamples / 10) == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine("\nSamples generated!");
        return samples;
    }

    static byte[] CreateWavFile(byte[] audioData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                // WAV Header
                writer.Write("RIFF".ToCharArray());
                writer.Write((uint)(36 + audioData.Length)); // File size
                writer.Write("WAVE".ToCharArray());

                // Format chunk
                writer.Write("fmt ".ToCharArray());
                writer.Write((uint)16); // Chunk size
                writer.Write((ushort)1); // Audio format (PCM)
                writer.Write((ushort)Channels); // Number of channels
                writer.Write((uint)SampleRate); // Sample rate
                writer.Write((uint)(SampleRate * Channels * BitsPerSample / 8)); // Byte rate
                writer.Write((ushort)(Channels * BitsPerSample / 8)); // Block align
                writer.Write((ushort)BitsPerSample); // Bits per sample

                // Data chunk
                writer.Write("data".ToCharArray());
                writer.Write((uint)audioData.Length); // Data size
                writer.Write(audioData); // Audio data
            }

            return ms.ToArray();
        }
    }

    static void PlayAudio(byte[] wavData)
    {
        Console.WriteLine("Playing audio...");

        // Save to temporary file
        string tempFile = Path.GetTempFileName() + ".wav";
        File.WriteAllBytes(tempFile, wavData);

        try
        {
            // Play using SoundPlayer
            using (SoundPlayer player = new SoundPlayer(tempFile))
            {
                player.PlaySync();
            }
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
class BytebeatPlayer_bold
{
    private const int SampleRate = 8000; // 8kHz
    private const int Duration = 17; // seconds
    private const int BitsPerSample = 8;
    private const int Channels = 1; // mono

    // Main function - can be called without blocking other functions
    public static async Task PlayBytebeatAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio...");
                Console.WriteLine("Formula: d=(t*(t&t>>12)*8/11025)¦0, ((d&16)/8−1)*(d*(d^15)+d+127)");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });
    }

    // Synchronous version that doesn't block (plays in background)
    public static void PlayBytebeatNonBlocking()
    {
        Thread audioThread = new Thread(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio in background...");
                Console.WriteLine("Formula: d=(t*(t&t>>12)*8/11025)¦0, ((d&16)/8−1)*(d*(d^15)+d+127)");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });

        audioThread.IsBackground = true; // Won't prevent app from closing
        audioThread.Start();
    }

    static byte[] GenerateBytebeatAudio()
    {
        int totalSamples = SampleRate * Duration;
        byte[] samples = new byte[totalSamples];

        Console.WriteLine("Generating samples...");

        for (int t = 0; t < totalSamples; t++)
        {
            // Bytebeat formula: d=(t*(t&t>>12)*8/11025)¦0, ((d&16)/8−1)*(d*(d^15)+d+127)
            int d = (int)((t * (t & t >> 12) * 8) / 11025.0);
            int result = ((d & 16) / 8 - 1) * (d * (d ^ 15) + d + 127);

            // Convert to 8-bit unsigned sample (0-255)
            samples[t] = (byte)(result & 0xFF);

            // Progress indicator
            if (t % (totalSamples / 10) == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine("\nSamples generated!");
        return samples;
    }

    static byte[] CreateWavFile(byte[] audioData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                // WAV Header
                writer.Write("RIFF".ToCharArray());
                writer.Write((uint)(36 + audioData.Length)); // File size
                writer.Write("WAVE".ToCharArray());

                // Format chunk
                writer.Write("fmt ".ToCharArray());
                writer.Write((uint)16); // Chunk size
                writer.Write((ushort)1); // Audio format (PCM)
                writer.Write((ushort)Channels); // Number of channels
                writer.Write((uint)SampleRate); // Sample rate
                writer.Write((uint)(SampleRate * Channels * BitsPerSample / 8)); // Byte rate
                writer.Write((ushort)(Channels * BitsPerSample / 8)); // Block align
                writer.Write((ushort)BitsPerSample); // Bits per sample

                // Data chunk
                writer.Write("data".ToCharArray());
                writer.Write((uint)audioData.Length); // Data size
                writer.Write(audioData); // Audio data
            }

            return ms.ToArray();
        }
    }

    static void PlayAudio(byte[] wavData)
    {
        Console.WriteLine("Playing audio...");

        // Save to temporary file
        string tempFile = Path.GetTempFileName() + ".wav";
        File.WriteAllBytes(tempFile, wavData);

        try
        {
            // Play using SoundPlayer
            using (SoundPlayer player = new SoundPlayer(tempFile))
            {
                player.PlaySync();
            }
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
class BytebeatPlayer_tunel
{
    private const int SampleRate = 8000; // 8kHz
    private const int Duration = 14; // seconds
    private const int BitsPerSample = 8;
    private const int Channels = 1; // mono

    // Main function - can be called without blocking other functions
    public static async Task PlayBytebeatAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio...");
                Console.WriteLine("Formula: (t&t+t/256)-t*(t>>15)&64");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });
    }

    // Synchronous version that doesn't block (plays in background)
    public static void PlayBytebeatNonBlocking()
    {
        Thread audioThread = new Thread(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio in background...");
                Console.WriteLine("Formula: (t&t+t/256)-t*(t>>15)&64");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });

        audioThread.IsBackground = true; // Won't prevent app from closing
        audioThread.Start();
    }

    static byte[] GenerateBytebeatAudio()
    {
        int totalSamples = SampleRate * Duration;
        byte[] samples = new byte[totalSamples];

        Console.WriteLine("Generating samples...");

        for (int t = 0; t < totalSamples; t++)
        {
            // Bytebeat formula: (t&t+t/256)-t*(t>>15)&64
            int result = (t & t + t / 256) - t * (t >> 15) & 64;

            // Convert to 8-bit unsigned sample (0-255)
            samples[t] = (byte)(result & 0xFF);

            // Progress indicator
            if (t % (totalSamples / 10) == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine("\nSamples generated!");
        return samples;
    }

    static byte[] CreateWavFile(byte[] audioData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                // WAV Header
                writer.Write("RIFF".ToCharArray());
                writer.Write((uint)(36 + audioData.Length)); // File size
                writer.Write("WAVE".ToCharArray());

                // Format chunk
                writer.Write("fmt ".ToCharArray());
                writer.Write((uint)16); // Chunk size
                writer.Write((ushort)1); // Audio format (PCM)
                writer.Write((ushort)Channels); // Number of channels
                writer.Write((uint)SampleRate); // Sample rate
                writer.Write((uint)(SampleRate * Channels * BitsPerSample / 8)); // Byte rate
                writer.Write((ushort)(Channels * BitsPerSample / 8)); // Block align
                writer.Write((ushort)BitsPerSample); // Bits per sample

                // Data chunk
                writer.Write("data".ToCharArray());
                writer.Write((uint)audioData.Length); // Data size
                writer.Write(audioData); // Audio data
            }

            return ms.ToArray();
        }
    }

    static void PlayAudio(byte[] wavData)
    {
        Console.WriteLine("Playing audio...");

        // Save to temporary file
        string tempFile = Path.GetTempFileName() + ".wav";
        File.WriteAllBytes(tempFile, wavData);

        try
        {
            // Play using SoundPlayer
            using (SoundPlayer player = new SoundPlayer(tempFile))
            {
                player.PlaySync();
            }
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
class BytebeatPlayer_icons
{
    private const int SampleRate = 16000; // 8kHz
    private const int Duration = 4; // seconds
    private const int BitsPerSample = 8;
    private const int Channels = 1; // mono

    // Main function - can be called without blocking other functions
    public static async Task PlayBytebeatAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio...");
                Console.WriteLine("Formula: t*(t*287/256&t>>11&31)");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });
    }

    // Synchronous version that doesn't block (plays in background)
    public static void PlayBytebeatNonBlocking()
    {
        Thread audioThread = new Thread(() =>
        {
            try
            {
                Console.WriteLine("Bytebeat Player - Generating audio in background...");
                Console.WriteLine("Formula: t*(t*287/256&t>>11&31)");
                Console.WriteLine("Sample Rate: " + SampleRate + "Hz, Duration: " + Duration + "s");

                // Generate bytebeat audio data
                byte[] audioData = GenerateBytebeatAudio();

                // Create WAV file in memory
                byte[] wavData = CreateWavFile(audioData);

                // Play the audio
                PlayAudio(wavData);

                Console.WriteLine("Bytebeat playback complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        });

        audioThread.IsBackground = true; // Won't prevent app from closing
        audioThread.Start();
    }

    static byte[] GenerateBytebeatAudio()
    {
        int totalSamples = SampleRate * Duration;
        byte[] samples = new byte[totalSamples];

        Console.WriteLine("Generating samples...");

        for (int t = 0; t < totalSamples; t++)
        {
            // Bytebeat formula: t*(t*287/256&t>>11&31)
            int result = t * ((t * 287 / 256) & (t >> 11) & 31);

            // Convert to 8-bit unsigned sample (0-255)
            samples[t] = (byte)(result & 0xFF);

            // Progress indicator
            if (t % (totalSamples / 10) == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine("\nSamples generated!");
        return samples;
    }

    static byte[] CreateWavFile(byte[] audioData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                // WAV Header
                writer.Write("RIFF".ToCharArray());
                writer.Write((uint)(36 + audioData.Length)); // File size
                writer.Write("WAVE".ToCharArray());

                // Format chunk
                writer.Write("fmt ".ToCharArray());
                writer.Write((uint)16); // Chunk size
                writer.Write((ushort)1); // Audio format (PCM)
                writer.Write((ushort)Channels); // Number of channels
                writer.Write((uint)SampleRate); // Sample rate
                writer.Write((uint)(SampleRate * Channels * BitsPerSample / 8)); // Byte rate
                writer.Write((ushort)(Channels * BitsPerSample / 8)); // Block align
                writer.Write((ushort)BitsPerSample); // Bits per sample

                // Data chunk
                writer.Write("data".ToCharArray());
                writer.Write((uint)audioData.Length); // Data size
                writer.Write(audioData); // Audio data
            }

            return ms.ToArray();
        }
    }

    static void PlayAudio(byte[] wavData)
    {
        Console.WriteLine("Playing audio...");

        // Save to temporary file
        string tempFile = Path.GetTempFileName() + ".wav";
        File.WriteAllBytes(tempFile, wavData);

        try
        {
            // Play using SoundPlayer
            using (SoundPlayer player = new SoundPlayer(tempFile))
            {
                player.PlaySync();
            }
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}

public class ScreenEffect2
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    private const uint SRCAND = 0x008800C6;
    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;

    public static void RunScreenEffect2()
    {
        SetProcessDPIAware();

        int w = GetSystemMetrics(SM_CXSCREEN);
        int h = GetSystemMetrics(SM_CYSCREEN);

        Random random = new Random();

        for (int i = 0; i < 350; i++)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);

            BitBlt(
                hdc,
                random.Next(1, 11) % 2,
                random.Next(1, 11) % 2,
                w,
                h,
                hdc,
                random.Next(1, 1001) % 2,
                random.Next(1, 1001) % 2,
                SRCAND
            );

            Thread.Sleep(10);
            ReleaseDC(IntPtr.Zero, hdc);
        }
    }
}
public class ScreenEffects
{
    // Import necessary Windows API functions
    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    [DllImport("gdi32.dll")]
    private static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest,
        int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc,
        int hSrc, uint rop);

    // Constants
    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;
    private const uint SRCCOPY = 0x00CC0020;

    public static void CreateStretchEffect()
    {
        // Set DPI awareness
        SetProcessDPIAware();

        // Get screen dimensions
        int sw = GetSystemMetrics(SM_CXSCREEN);
        int sh = GetSystemMetrics(SM_CYSCREEN);

        int delay = 10; // 100ms delay (equivalent to 0.1 seconds)
        int size = 100;

        for (int i = 0; i < 300; i++)
        {
            // Get device context for the entire screen
            IntPtr hdc = GetDC(IntPtr.Zero);

            try
            {
                // Perform the stretch blit operation
                StretchBlt(
                    hdc,                    // destination DC
                    size / 2,              // destination x
                    size / 2,              // destination y
                    sw - size,             // destination width
                    sh - size,             // destination height
                    hdc,                   // source DC (same as destination)
                    0,                     // source x
                    0,                     // source y
                    sw,                    // source width
                    sh,                    // source height
                    SRCCOPY                // raster operation
                );
            }
            finally
            {
                // Always release the device context
                ReleaseDC(IntPtr.Zero, hdc);
            }

            // Sleep for the specified delay
            Thread.Sleep(delay);
        }
    }
}

public class BackgroundMP4Player
{
    private Dictionary<string, Process> _processes;
    private Dictionary<string, bool> _isLooping;

    [DllImport("winmm.dll")]
    private static extern int mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

    public BackgroundMP4Player()
    {
        _processes = new Dictionary<string, Process>();
        _isLooping = new Dictionary<string, bool>();
    }

    /// <summary>
    /// Plays an MP4 file in the background using Windows Media Player
    /// </summary>
    /// <param name="filePath">Path to the MP4 file</param>
    /// <param name="volume">Volume level (0.0 to 1.0)</param>
    /// <param name="loop">Whether to loop the audio</param>
    /// <param name="playerId">Optional unique identifier for this player instance</param>
    /// <returns>The player ID for managing this specific instance</returns>
    public string PlayMP4(string filePath, double volume = 0.5, bool loop = false, string playerId = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("MP4 file not found: " + filePath);
        }

        if (volume < 0.0 || volume > 1.0)
        {
            throw new ArgumentException("Volume must be between 0.0 and 1.0");
        }

        // Generate unique ID if not provided
        if (string.IsNullOrEmpty(playerId))
        {
            playerId = Guid.NewGuid().ToString();
        }

        // Stop existing player with same ID if it exists
        if (_processes.ContainsKey(playerId))
        {
            StopMP4(playerId);
        }

        try
        {
            // First try MCI with different types
            string[] mciTypes = { "mpegvideo", "AVIVideo", "waveaudio" };
            bool mciSuccess = false;
            
            foreach (string mciType in mciTypes)
            {
                string command = "open \"" + filePath + "\" type " + mciType + " alias " + playerId;
                int result = mciSendString(command, null, 0, IntPtr.Zero);

                if (result == 0)
                {
                    // Set volume (0-1000 scale for MCI)
                    int mciVolume = (int)(volume * 1000);
                    mciSendString("setaudio " + playerId + " volume to " + mciVolume, null, 0, IntPtr.Zero);

                    // Play the file
                    if (loop)
                    {
                        mciSendString("play " + playerId + " repeat", null, 0, IntPtr.Zero);
                    }
                    else
                    {
                        mciSendString("play " + playerId, null, 0, IntPtr.Zero);
                    }

                    _isLooping[playerId] = loop;
                    mciSuccess = true;
                    break;
                }
            }

            // If MCI fails, fall back to Windows Media Player
            if (!mciSuccess)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "wmplayer.exe";
                startInfo.Arguments = "\"" + filePath + "\"";
                if (loop)
                {
                    startInfo.Arguments += " /repeat";
                }
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = true;

                Process process = Process.Start(startInfo);
                _processes[playerId] = process;
                _isLooping[playerId] = loop;
            }

            return playerId;
        }
        catch (Exception ex)
        {
            // Final fallback: try to play with default system player
            try
            {
                ProcessStartInfo fallbackInfo = new ProcessStartInfo();
                fallbackInfo.FileName = filePath;
                fallbackInfo.UseShellExecute = true;
                fallbackInfo.WindowStyle = ProcessWindowStyle.Hidden;
                
                Process fallbackProcess = Process.Start(fallbackInfo);
                _processes[playerId] = fallbackProcess;
                _isLooping[playerId] = loop;
                
                return playerId;
            }
            catch
            {
                throw new InvalidOperationException("Failed to play MP4 file with all methods: " + ex.Message, ex);
            }
        }
    }

    /// <summary>
    /// Stops a specific MP4 player
    /// </summary>
    /// <param name="playerId">ID of the player to stop</param>
    public void StopMP4(string playerId)
    {
        if (_isLooping.ContainsKey(playerId))
        {
            mciSendString("stop " + playerId, null, 0, IntPtr.Zero);
            mciSendString("close " + playerId, null, 0, IntPtr.Zero);
            _isLooping.Remove(playerId);
        }

        if (_processes.ContainsKey(playerId))
        {
            try
            {
                if (!_processes[playerId].HasExited)
                {
                    _processes[playerId].Kill();
                }
                _processes[playerId].Dispose();
            }
            catch { }
            _processes.Remove(playerId);
        }
    }

    /// <summary>
    /// Pauses a specific MP4 player
    /// </summary>
    /// <param name="playerId">ID of the player to pause</param>
    public void PauseMP4(string playerId)
    {
        if (_isLooping.ContainsKey(playerId))
        {
            mciSendString("pause " + playerId, null, 0, IntPtr.Zero);
        }
    }

    /// <summary>
    /// Resumes a paused MP4 player
    /// </summary>
    /// <param name="playerId">ID of the player to resume</param>
    public void ResumeMP4(string playerId)
    {
        if (_isLooping.ContainsKey(playerId))
        {
            mciSendString("resume " + playerId, null, 0, IntPtr.Zero);
        }
    }

    /// <summary>
    /// Sets the volume for a specific player
    /// </summary>
    /// <param name="playerId">ID of the player</param>
    /// <param name="volume">Volume level (0.0 to 1.0)</param>
    public void SetVolume(string playerId, double volume)
    {
        if (volume < 0.0 || volume > 1.0)
        {
            throw new ArgumentException("Volume must be between 0.0 and 1.0");
        }

        if (_isLooping.ContainsKey(playerId))
        {
            int mciVolume = (int)(volume * 1000);
            mciSendString("setaudio " + playerId + " volume to " + mciVolume, null, 0, IntPtr.Zero);
        }
    }

    /// <summary>
    /// Stops all currently playing MP4s
    /// </summary>
    public void StopAll()
    {
        var playerIds = new List<string>(_isLooping.Keys);
        foreach (var id in playerIds)
        {
            StopMP4(id);
        }
    }

    /// <summary>
    /// Gets the list of currently active player IDs
    /// </summary>
    public List<string> GetActivePlayerIds()
    {
        return new List<string>(_isLooping.Keys);
    }

    /// <summary>
    /// Checks if a specific player is currently playing
    /// </summary>
    /// <param name="playerId">ID of the player to check</param>
    public bool IsPlaying(string playerId)
    {
        return _isLooping.ContainsKey(playerId);
    }

    /// <summary>
    /// Disposes all resources
    /// </summary>
    public void Dispose()
    {
        StopAll();
    }
}

// Example usage class
public class MP4PlayerExample
{
    public static void ExampleUsage()
    {
        var player = new BackgroundMP4Player();

        try
        {
            // Play multiple MP4 files simultaneously
            string music1 = player.PlayMP4("C:\\path\\to\\background_music.mp4", volume: 0.3, loop: true, playerId: "bgmusic");
            string sfx1 = player.PlayMP4("C:\\path\\to\\sound_effect1.mp4", volume: 0.8, playerId: "effect1");
            string sfx2 = player.PlayMP4("C:\\path\\to\\sound_effect2.mp4", volume: 0.6, playerId: "effect2");

            // Control individual players
            System.Threading.Thread.Sleep(5000); // Wait 5 seconds

            player.SetVolume("bgmusic", 0.1); // Lower background music
            player.PauseMP4("bgmusic");        // Pause background music

            System.Threading.Thread.Sleep(2000); // Wait 2 seconds

            player.ResumeMP4("bgmusic");       // Resume background music

            // Stop specific sound
            player.StopMP4("effect1");

            // Stop all sounds when done
            player.StopAll();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            player.Dispose();
        }
    }
}

namespace FullscreenImageOverlay
{
    public partial class FullscreenForm : Form
    {
        private PictureBox pictureBox;
        private string imagePath;
        private Image imageObject;
        private PictureBoxSizeMode sizeMode;
        
        public FullscreenForm(string imagePath, PictureBoxSizeMode sizeMode = PictureBoxSizeMode.StretchImage)
        {
            this.imagePath = imagePath;
            this.sizeMode = sizeMode;
            InitializeComponent();
            SetupFullscreenOverlay();
        }
        
        public FullscreenForm(Image image, PictureBoxSizeMode sizeMode = PictureBoxSizeMode.StretchImage)
        {
            this.imageObject = image;
            this.sizeMode = sizeMode;
            InitializeComponent();
            SetupFullscreenOverlay();
        }
        
        private void SetupFullscreenOverlay()
        {
            // Make the form fullscreen and topmost
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Cover all screens in multi-monitor setup
            Rectangle screenBounds = SystemInformation.VirtualScreen;
            this.Location = new Point(screenBounds.Left, screenBounds.Top);
            this.Size = new Size(screenBounds.Width, screenBounds.Height);
            
            // Create and configure PictureBox
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = this.sizeMode;
            pictureBox.BackColor = Color.Black;
            
            // Load image from path or use provided image object
            try
            {
                if (imageObject != null)
                {
                    pictureBox.Image = imageObject;
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    pictureBox.Image = Image.FromFile(imagePath);
                }
                else
                {
                    throw new ArgumentException("No image provided");
                }
            }
            catch (Exception ex)
            {
                // If image fails to load, show error message
                Label errorLabel = new Label();
                errorLabel.Text = "Failed to load image: " + ex.Message;
                errorLabel.ForeColor = Color.White;
                errorLabel.BackColor = Color.Black;
                errorLabel.Dock = DockStyle.Fill;
                errorLabel.TextAlign = ContentAlignment.MiddleCenter;
                errorLabel.Font = new Font("Arial", 16);
                this.Controls.Add(errorLabel);
                return;
            }
            
            this.Controls.Add(pictureBox);
            
            // Add event handlers for closing the overlay
            this.KeyDown += FullscreenForm_KeyDown;
            pictureBox.Click += PictureBox_Click;
            
            // Make sure the form can receive key events
            this.KeyPreview = true;
        }
        
        private void FullscreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Close on Escape key or Alt+F4
            if (e.KeyCode == Keys.Escape || 
                (e.KeyCode == Keys.F4 && e.Alt))
            {
                this.Close();
            }
        }
        
        private void PictureBox_Click(object sender, EventArgs e)
        {
            // Optional: Close on click
            // this.Close();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FullscreenForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 600);
            this.Name = "FullscreenForm";
            this.Text = "Fullscreen Image Overlay";
            this.ResumeLayout(false);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && pictureBox != null && pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    
    // Static class to provide the function
    public static class FullscreenImageDisplay
    {
        /// <summary>
        /// Shows a fullscreen image overlay that blocks the entire screen
        /// </summary>
        /// <param name="imagePath">Path to the image file to display</param>
        /// <param name="sizeMode">How the image should be displayed (default: StretchImage)</param>
        public static void ShowFullscreenImage(string imagePath, PictureBoxSizeMode sizeMode = PictureBoxSizeMode.StretchImage)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            using (var form = new FullscreenForm(imagePath, sizeMode))
            {
                Application.Run(form);
            }
        }
        
        /// <summary>
        /// Shows a fullscreen image overlay using an Image object
        /// </summary>
        /// <param name="image">Image object to display</param>
        /// <param name="sizeMode">How the image should be displayed (default: StretchImage)</param>
        public static void ShowFullscreenImage(Image image, PictureBoxSizeMode sizeMode = PictureBoxSizeMode.StretchImage)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            using (var form = new FullscreenForm(image, sizeMode))
            {
                Application.Run(form);
            }
        }
    }
}

public static class DesktopBackgroundChanger
{
    // Import the SystemParametersInfo function from user32.dll
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    // Constants for SystemParametersInfo
    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDCHANGE = 0x02;

    /// <summary>
    /// Changes desktop background using SystemParametersInfo API
    /// This is the recommended method as it immediately applies the change
    /// </summary>
    /// <param name="imagePath">Full path to the image file</param>
    public static void ChangeBackgroundWithSystemParams(string imagePath)
    {
        // Validate the image file exists
        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException("Image file not found: " + imagePath);
        }

        // Validate image format (Windows supports BMP, JPG, PNG, etc.)
        string extension = Path.GetExtension(imagePath).ToLower();
        string[] supportedFormats = { ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".tiff" };
        
        if (Array.IndexOf(supportedFormats, extension) == -1)
        {
            throw new ArgumentException("Unsupported image format: " + extension);
        }

        // Set the desktop wallpaper
        int result = SystemParametersInfo(
            SPI_SETDESKWALLPAPER,
            0,
            imagePath,
            SPIF_UPDATEINIFILE | SPIF_SENDCHANGE
        );

        if (result == 0)
        {
            throw new InvalidOperationException("Failed to set desktop wallpaper");
        }
    }

    /// <summary>
    /// Alternative method using Registry
    /// Note: This requires a system restart or manual refresh to take effect
    /// </summary>
    /// <param name="imagePath">Full path to the image file</param>
    public static void ChangeBackgroundWithRegistry(string imagePath)
    {
        // Validate the image file exists
        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException("Image file not found: " + imagePath);
        }

        // Open the registry key for desktop settings
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
        {
            if (key != null)
            {
                // Set the wallpaper path
                key.SetValue("Wallpaper", imagePath);
                
                // Set wallpaper style (optional)
                // 0 = Center, 1 = Tile, 2 = Stretch, 6 = Fit, 10 = Fill
                key.SetValue("WallpaperStyle", "2"); // Stretch
                key.SetValue("TileWallpaper", "0");   // Don't tile
            }
        }

        // Refresh the desktop (requires additional system call)
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, null, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
    }

    /// <summary>
    /// Helper method to set wallpaper style along with the image
    /// </summary>
    /// <param name="imagePath">Full path to the image file</param>
    /// <param name="style">Wallpaper style (Center=0, Tile=1, Stretch=2, Fit=6, Fill=10)</param>
    public static void ChangeBackgroundWithStyle(string imagePath, WallpaperStyle style)
    {
        // First set the wallpaper style in registry
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
        {
            if (key != null)
            {
                key.SetValue("WallpaperStyle", ((int)style).ToString());
                key.SetValue("TileWallpaper", style == WallpaperStyle.Tile ? "1" : "0");
            }
        }

        // Then set the wallpaper image
        ChangeBackgroundWithSystemParams(imagePath);
    }

    /// <summary>
    /// Enumeration for wallpaper display styles
    /// </summary>
    public enum WallpaperStyle
    {
        Center = 0,
        Tile = 1,
        Stretch = 2,
        Fit = 6,
        Fill = 10
    }
}

public struct IconSoundPair
{
    public IntPtr icon;
    public uint sound;

    public IconSoundPair(IntPtr icon, uint sound)
    {
        this.icon = icon;
        this.sound = sound;
    }
}

public class IconDrawer
{
    // Windows API imports
    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

    [DllImport("user32.dll")]
    static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr LoadImage(IntPtr hInst, string lpszName, uint uType, 
        int cxDesired, int cyDesired, uint fuLoad);

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    static extern bool SetProcessDPIAware();

    [DllImport("user32.dll")]
    static extern bool MessageBeep(uint uType);

    // Constants
    const int SM_CXSCREEN = 0;  // Screen width
    const int SM_CYSCREEN = 1;  // Screen height
    const uint IMAGE_ICON = 1;
    const uint LR_LOADFROMFILE = 0x00001110;
    const uint LR_DEFAULTSIZE = 0x00707500;
    
    // System icons with corresponding sounds
    static readonly IconSoundPair[] ICON_SOUND_PAIRS = new IconSoundPair[]
    {
        new IconSoundPair(new IntPtr(32513), 0x00000010), // IDI_ERROR -> MB_ICONHAND (error sound)
        new IconSoundPair(new IntPtr(32514), 0x00000020), // IDI_QUESTION -> MB_ICONQUESTION (question sound)
        new IconSoundPair(new IntPtr(32515), 0x00000030), // IDI_WARNING -> MB_ICONEXCLAMATION (warning sound)
        new IconSoundPair(new IntPtr(32516), 0x00000040), // IDI_INFORMATION -> MB_ICONASTERISK (info sound)
        new IconSoundPair(new IntPtr(32512), 0x00000000), // IDI_APPLICATION -> MB_OK (default sound)
        new IconSoundPair(new IntPtr(32517), 0x00000040), // IDI_WINLOGO -> MB_ICONASTERISK (info sound)
        new IconSoundPair(new IntPtr(32518), 0x00000030), // IDI_SHIELD -> MB_ICONEXCLAMATION (warning sound)
    };

    /// <summary>
    /// Draws random system icons across the screen with corresponding Windows sounds
    /// </summary>
    /// <param name="iterations">Number of icons to draw (default: 7000000)</param>
    /// <param name="delayMs">Delay between each icon in milliseconds (default: 200)</param>
    /// <param name="enableSounds">Whether to play Windows system sounds (default: true)</param>
    /// <param name="ponte">Whether to use ponte icon (default: true)</param>
    public static void DrawRandomIcons(int iterations = 7000000, int delayMs = 10, bool enableSounds = true, bool ponte = true)
    {
        // Get device context for the entire screen
        IntPtr hdc = GetDC(IntPtr.Zero);
        
        // Set DPI awareness
        SetProcessDPIAware();
        
        // Get screen dimensions
        int sw = GetSystemMetrics(SM_CXSCREEN);
        int sh = GetSystemMetrics(SM_CYSCREEN);
        
        Random random = new Random();
        
        for (int i = 0; i < iterations; i++)
        {            
            // Select a random icon-sound pair
            var randomPair = ICON_SOUND_PAIRS[random.Next(0, ICON_SOUND_PAIRS.Length)];
            IntPtr loadedIcon;

            if (ponte == true) {
                loadedIcon = LoadImage(IntPtr.Zero, @"C:\Images\ponte.ico", 
                    IMAGE_ICON, 0, 0, LR_LOADFROMFILE | LR_DEFAULTSIZE);
            } else {
                loadedIcon = LoadIcon(IntPtr.Zero, randomPair.icon);
            }
            
            // Draw the icon at random position
            DrawIcon(
                hdc,
                random.Next(0, sw),
                random.Next(0, sh),
                loadedIcon
            );
        }
        
        // Second loop for additional icons
        for (int i = 0; i < iterations; i++)
        {
            
            // Select a random icon-sound pair
            var randomPair = ICON_SOUND_PAIRS[random.Next(0, ICON_SOUND_PAIRS.Length)];
            IntPtr loadedIcon;

            if (ponte == true) {
                loadedIcon = LoadImage(IntPtr.Zero, @"C:\Images\ponte.ico", 
                    IMAGE_ICON, 0, 0, LR_LOADFROMFILE | LR_DEFAULTSIZE);
            } else {
                loadedIcon = LoadIcon(IntPtr.Zero, randomPair.icon);
            }
            
            // Draw the icon at random position
            DrawIcon(
                hdc,
                random.Next(0, sw),
                random.Next(0, sh),
                loadedIcon
            );
        }
    }
}

namespace RandomAppLauncher
{
    class RandomAppLauncher
    {
        // List of common Windows applications with their executable names or commands
        private static readonly List<string> WindowsApps = new List<string>
        {
            "calc",           // Calculator
            "chrome",         // Google Chrome
            "firefox",        // Firefox
            "notepad",        // Notepad
            "mspaint",        // Paint
            "explorer",       // File Explorer
            "cmd",            // Command Prompt
            "control",        // Control Panel
            "ms-settings:",   // Windows Settings
            "snippingtool",   // Snipping Tool
            "charmap",        // Character Map
            "msinfo32",       // System Information
            "regedit",        // Registry Editor
            "devmgmt.msc"     // Device Manager
        };

        /// <summary>
        /// Opens random Windows applications at 10-second intervals
        /// </summary>
        /// <param name="count">Number of times to loop and open apps</param>
        public static void OpenRandomApps(int count)
        {
            if (count <= 0)
            {
                Console.WriteLine("Count must be greater than 0.");
                return;
            }

            Random random = new Random();

            for (int i = 1; i <= count; i++)
            {
                try
                {
                    // Select a random app from the list
                    string randomApp = WindowsApps[random.Next(WindowsApps.Count)];

                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Attempt " + i + "/" + count + ": Opening " + randomApp + "...");

                    // Launch the application
                    LaunchApplication(randomApp);

                    Console.WriteLine("Successfully launched " + randomApp);

                    // Wait 10 seconds before next iteration (except for the last one)
                    if (i < count)
                    {
                        Console.WriteLine("Waiting 10 seconds...\n");
                        Thread.Sleep(10000); // 10 seconds
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to launch app: " + ex.Message);

                    // Still wait before next iteration if not the last one
                    if (i < count)
                    {
                        Console.WriteLine("Waiting 10 seconds before next attempt...\n");
                        Thread.Sleep(10000);
                    }
                }
            }
        }

        /// <summary>
        /// Launches a specific application
        /// </summary>
        /// <param name="appName">Name or command of the application to launch</param>
        private static void LaunchApplication(string appName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            // Handle special cases for Windows Settings and other URI-based apps
            if (appName.StartsWith("ms-settings:"))
            {
                startInfo.FileName = appName;
                startInfo.UseShellExecute = true;
            }
            else if (appName.EndsWith(".msc"))
            {
                // For Microsoft Management Console files
                startInfo.FileName = "mmc";
                startInfo.Arguments = appName;
                startInfo.UseShellExecute = true;
            }
            else
            {
                startInfo.FileName = appName;
                startInfo.UseShellExecute = true;
            }

            // Set window state to normal (not minimized)
            startInfo.WindowStyle = ProcessWindowStyle.Normal;

            // Start the process
            Process.Start(startInfo);
        }

        /// <summary>
        /// Alternative method with custom interval
        /// </summary>
        /// <param name="count">Number of apps to open</param>
        /// <param name="intervalSeconds">Interval between launches in seconds</param>
        public static void OpenRandomAppsWithInterval(int count, int intervalSeconds)
        {
            if (count <= 0)
            {
                Console.WriteLine("Count must be greater than 0.");
                return;
            }

            if (intervalSeconds < 0)
            {
                Console.WriteLine("Interval must be non-negative.");
                return;
            }

            Random random = new Random();

            for (int i = 1; i <= count; i++)
            {
                try
                {
                    string randomApp = WindowsApps[random.Next(WindowsApps.Count)];

                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Attempt " + i + "/" + count + ": Opening " + randomApp + "...");

                    LaunchApplication(randomApp);

                    Console.WriteLine("Successfully launched " + randomApp);

                    if (i < count && intervalSeconds > 0)
                    {
                        Console.WriteLine("Waiting " + intervalSeconds + " seconds...\n");
                        Thread.Sleep(intervalSeconds * 1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to launch app: " + ex.Message);

                    if (i < count && intervalSeconds > 0)
                    {
                        Console.WriteLine("Waiting " + intervalSeconds + " seconds before next attempt...\n");
                        Thread.Sleep(intervalSeconds * 1000);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of available apps
        /// </summary>
        /// <returns>List of application names</returns>
        public static List<string> GetAvailableApps()
        {
            return new List<string>(WindowsApps);
        }

        /// <summary>
        /// Adds a custom app to the list
        /// </summary>
        /// <param name="appName">Name or path of the application</param>
        public static void AddCustomApp(string appName)
        {
            if (!string.IsNullOrWhiteSpace(appName) && !WindowsApps.Contains(appName))
            {
                WindowsApps.Add(appName);
                Console.WriteLine("Added " + appName + " to the app list.");
            }
        }
    }
}

// Fixed ChromeSearcherSync class - replace the existing one
class ChromeSearcherSync
{
    private static readonly string[] searchTerms = {
        "feira de ciências do Culto à Ciência",
        "como remover um virus 100% seguro atualizado 2025",
        "qual é o malware mais perigoso do mundo?",
        "programadores ainda possuem chance no mercado de trabalho?",
        "IA vai dominar o mundo?",
        "memes de gatos fofos",
        "guarani é o melhor time do brasil?",
        "ponte preta",
        "lucas arbusto",
        "onde comprar um pc novo?",
    };

    /// <summary>
    /// Synchronous version - Opens Google Chrome with random search queries at 10-second intervals
    /// </summary>
    /// <param name="loopCount">Number of times to open Chrome with a search</param>
    public static void SearchRandomThingsSync(int loopCount)
    {
        Console.WriteLine("Starting Chrome search loop - " + loopCount + " iterations");
        Random random = new Random();

        // Fixed: Use Math.Min to ensure we don't exceed available search terms
        int maxIterations = Math.Min(loopCount, searchTerms.Length);
        
        for (int i = 0; i < maxIterations; i++)
        {
            try
            {
                string searchTerm = searchTerms[i];
                string encodedSearchTerm = Uri.EscapeDataString(searchTerm);
                string searchUrl = "https://www.google.com/search?q=" + encodedSearchTerm;

                Console.WriteLine("[" + (i + 1) + "/" + maxIterations + "] Searching: \"" + searchTerm + "\"");

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "chrome.exe",
                    Arguments = searchUrl,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                
                if (searchTerm == "ponte preta") 
                {
                    IconDrawer.DrawRandomIcons(250, 1, true, true);
                }
                
                if (i < maxIterations - 1)
                {
                    Thread.Sleep(8000); // 8 seconds
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}

class Program
{
    // Win32 API imports
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    static extern bool SetProcessDPIAware();

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    [DllImport("gdi32.dll")]
    static extern IntPtr CreateSolidBrush(uint crColor);

    [DllImport("gdi32.dll")]
    static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
        IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

    [DllImport("winmm.dll")]
    static extern int mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

    // Constants
    const int SW_HIDE = 0;
    const int SW_SHOW = 5;
    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;
    const uint SRCCOPY = 0x00CC0020;
    const uint PATINVERT = 0x005A0049;

    // Static fields for audio players - moved inside class and made static
    private static BackgroundMP4Player Musica = new BackgroundMP4Player();
    private static BackgroundMP4Player Infectar = new BackgroundMP4Player();
    private static BackgroundMP4Player Risos = new BackgroundMP4Player();
    private static BackgroundMP4Player Glitch = new BackgroundMP4Player();

    // Static fields for audio
    private static Random random = new Random();
    private static bool audioPlaying = false;

    // Path to your MP3 file - will be set to same directory as executable
    private static string mp3FilePath;

    [STAThread]
    static void Main(string[] args)
    {
        // Set the MP3 file path to the same directory as the executable
        string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        mp3FilePath = Path.Combine(exeDirectory, "culto_audio.mp3");

        // Hide console window immediately but keep dialogs
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);

        // First confirmation dialog
        DialogResult firstResult = MessageBox.Show(
            "Você está prestes a executar um malware! Apesar de ele não ser prejudicial ao seu computador, ele possuí efeitos piscantes que podem afetar algumas pessoas, caso não queira apenas aperte Não",
            "Pera ai meu amigo!",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (firstResult == DialogResult.No)
        {
            return;
        }

        // Second confirmation dialog
        DialogResult secondResult = MessageBox.Show(
            "ESSE É O ÚLTIMO AVISO! NÃO SOMOS RESPONSABILIZADOS POR NENHUM ESTRAGO!",
            "ÚLTIMO AVISO!",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (secondResult == DialogResult.No)
        {
            return;
        }

        // Both confirmations passed, execute desktop effects
        try
        {
            Thread.Sleep(2000);
            DialogResult seferro = MessageBox.Show(
                "Acredito que não há mais volta né?",
                "Diga adeus...",
                MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Information
            );
            Thread.Sleep(2000);
            // som de infectar
            string playerIdINFECTAR = Infectar.PlayMP4("C:\\Images\\infectado.wav");
            DesktopBackgroundChanger.ChangeBackgroundWithStyle(@"C:\Images\myImage.png", DesktopBackgroundChanger.WallpaperStyle.Stretch);
            Thread.Sleep(4000);
            // musica
            string playerIdMUSICA = Musica.PlayMP4("C:\\Images\\musicao.wav");
            RandomAppLauncher.RandomAppLauncher.OpenRandomAppsWithInterval(20, 2);
            Thread.Sleep(7500);
            ChromeSearcherSync.SearchRandomThingsSync(12);
            // risada
            Musica.StopMP4(playerIdMUSICA);
            BytebeatPlayer_icons.PlayBytebeatNonBlocking();
            IconDrawer.DrawRandomIcons(30000, 1, true, false);
            BytebeatPlayer_tunel.PlayBytebeatNonBlocking();
            ScreenEffects.CreateStretchEffect();
            BytebeatPlayer_bold.PlayBytebeatNonBlocking();
            ScreenEffect2.RunScreenEffect2();
            // glitch
            BytebeatPlayer_glitch.PlayBytebeatNonBlocking();
            ExecuteDesktopEffects();
            SingleBeatPlayer.PlaySingleBeatNonBlocking();
            FullscreenImageOverlay.FullscreenImageDisplay.ShowFullscreenImage(@"C:\Images\bluescreen.png", PictureBoxSizeMode.Zoom);
        }
        catch (Exception ex)
        {
            // Log error to a file instead of showing MessageBox
            try
            {
                File.WriteAllText(Path.Combine(exeDirectory, "error.log"), DateTime.Now + ": " + ex.Message + "\n" + ex.StackTrace);
            }
            catch
            {
                // If we can't even write to file, just exit silently
            }
        }
        finally
        {
            // Clean up audio
            CleanupAudio();
        }
    }

    private static void ExecuteDesktopEffects()
    {
        // Initialize audio first
        bool audioInitialized = InitializeAudio();

        // Wait a moment for audio to start
        if (audioInitialized)
        {
            Thread.Sleep(500);
        }

        // Initialize
        SetProcessDPIAware();

        // Get screen dimensions
        int sw = GetSystemMetrics(SM_CXSCREEN);
        int sh = GetSystemMetrics(SM_CYSCREEN);

        double color = 0;

        // Main loop
        for (int i = 0; i < 600; i++)
        {
            Thread.Sleep(1);

            // Get device context for the entire screen
            IntPtr hdc = GetDC(IntPtr.Zero);

            try
            {
                // Convert HSV to RGB
                Color rgbColor = HSVToRGB(color, 1.0, 1.0);

                // Create brush with the color
                uint colorRef = (uint)((rgbColor.B << 16) | (rgbColor.G << 8) | rgbColor.R);
                IntPtr brush = CreateSolidBrush(colorRef);
                IntPtr oldBrush = SelectObject(hdc, brush);

                // Perform BitBlt operations with random offsets
                BitBlt(hdc,
                       random.Next(-50, 51),
                       random.Next(-50, 51),
                       sw, sh,
                       hdc, 0, 0,
                       SRCCOPY);

                BitBlt(hdc,
                       random.Next(-50, 51),
                       random.Next(-50, 51),
                       sw, sh,
                       hdc, 0, 0,
                       PATINVERT);

                // Clean up GDI objects
                SelectObject(hdc, oldBrush);
                DeleteObject(brush);
            }
            finally
            {
                ReleaseDC(IntPtr.Zero, hdc);
            }

            color += 0.05;
        }

        // Clean up audio after effect is done
        CleanupAudio();
    }

    private static bool InitializeAudio()
    {
        try
        {
            // Check if the MP3 file exists
            if (!File.Exists(mp3FilePath))
            {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(mp3FilePath), "audio_error.log"),
                    "MP3 file not found: " + mp3FilePath);
                return false;
            }

            // Use MCI to play the audio file
            string command = "open \"" + mp3FilePath + "\" type mpegvideo alias MediaFile";
            int result = mciSendString(command, null, 0, IntPtr.Zero);

            if (result != 0)
            {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(mp3FilePath), "audio_error.log"),
                    "Failed to open audio file. MCI Error: " + result.ToString());
                return false;
            }

            // Set volume to maximum
            mciSendString("setaudio MediaFile volume to 1000", null, 0, IntPtr.Zero);

            // Play the file on repeat
            mciSendString("play MediaFile repeat", null, 0, IntPtr.Zero);

            audioPlaying = true;

            File.WriteAllText(Path.Combine(Path.GetDirectoryName(mp3FilePath), "audio_success.log"),
                "Audio started successfully: " + mp3FilePath);

            return true;
        }
        catch (Exception ex)
        {
            try
            {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(mp3FilePath), "audio_error.log"),
                    "Audio initialization error: " + ex.Message + "\n" + ex.StackTrace);
            }
            catch { }
            return false;
        }
    }

    private static void CleanupAudio()
    {
        try
        {
            if (audioPlaying)
            {
                mciSendString("stop MediaFile", null, 0, IntPtr.Zero);
                mciSendString("close MediaFile", null, 0, IntPtr.Zero);
                audioPlaying = false;
            }
        }
        catch { }
    }

    private static Color HSVToRGB(double h, double s, double v)
    {
        // Normalize hue to 0-360 range
        h = h % 1.0;
        if (h < 0) h += 1.0;

        double c = v * s;
        double x = c * (1 - Math.Abs((h * 6) % 2 - 1));
        double m = v - c;

        double r, g, b;

        if (h < 1.0 / 6.0)
        {
            r = c; g = x; b = 0;
        }
        else if (h < 2.0 / 6.0)
        {
            r = x; g = c; b = 0;
        }
        else if (h < 3.0 / 6.0)
        {
            r = 0; g = c; b = x;
        }
        else if (h < 4.0 / 6.0)
        {
            r = 0; g = x; b = c;
        }
        else if (h < 5.0 / 6.0)
        {
            r = x; g = 0; b = c;
        }
        else
        {
            r = c; g = 0; b = x;
        }

        return Color.FromArgb(
            (int)((r + m) * 255),
            (int)((g + m) * 255),
            (int)((b + m) * 255)
        );
    }
}