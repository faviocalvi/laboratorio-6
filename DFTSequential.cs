using System;
using System.Numerics;
using System.Diagnostics;

class DFTSequential
{
    public static void Run()
    {
        int N = 8192;
        Complex[] signal = new Complex[N];
        for (int i = 0; i < N; i++)
        {
            signal[i] = new Complex(Math.Sin(2 * Math.PI * i / N), 0);
        }
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Complex[] dftResult = ComputeDFT(signal);
        stopwatch.Stop();
        Console.WriteLine("Tiempo de ejecución secuencial: " + stopwatch.ElapsedMilliseconds + " ms");
    }
    static Complex[] ComputeDFT(Complex[] signal)
    {
        int N = signal.Length;
        Complex[] result = new Complex[N];
        for (int k = 0; k < N; k++)
        {
            result[k] = new Complex(0, 0);
            for (int n = 0; n < N; n++)
            {
                double angle = -2 * Math.PI * k * n / N;
                result[k] += signal[n] * new Complex(Math.Cos(angle), Math.Sin(angle));
            }
        }
        return result;
    }
}
