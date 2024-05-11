using System;

using NAudio;
using NAudio.Wave;

public class MicModule
{
	public MicModule()
	{
        // Specify the file name and format
        string fileName = "tester.wav";
        WaveFormat waveFormat = new WaveFormat(44100, 16, 2); // 44.1kHz, 16-bit, stereo

        // Create a WaveIn instance for recording
        using (WaveInEvent waveIn = new WaveInEvent())
        {
            // Set the desired WaveFormat
            waveIn.WaveFormat = waveFormat;

            // Set the callback for when data is recorded
            waveIn.DataAvailable += (sender, e) =>
            {
                // Write the recorded data to a file
                using (WaveFileWriter writer = new WaveFileWriter(fileName, waveIn.WaveFormat))
                {
                    writer.Write(e.Buffer, 0, e.BytesRecorded);
                    writer.Flush();
                }
            };

            // Start recording
            waveIn.StartRecording();

            // Wait for user input to stop recording
            Console.WriteLine("Recording... Press Enter to stop.");
            Console.ReadLine();

            // Stop recording
            waveIn.StopRecording();
        }
    }
}
