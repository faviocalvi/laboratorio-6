using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.Threading;

class DFTParallel
{
    static Complex[] result;
    static object lockObject = new object();

    public static void Run()
    {
        int N = 8192; 
        Complex[] signal = new Complex[N];
        result = new Complex[N];
        for (int i = 0; i < N; i++)
        {
            signal[i] = new Complex(Math.Sin(2 * Math.PI * i / N), 0);
        }
        int numberOfThreads = 4;
        int rangePerThread = N / numberOfThreads;
        List<Thread> threads = new List<Thread>();
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < numberOfThreads; i++)
        {
            int threadStart = i * rangePerThread;
            int threadEnd = (i == numberOfThreads - 1) ? N : threadStart + rangePerThread;

            Thread thread = new Thread(() => ComputeDFT(signal, threadStart, threadEnd));
            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        stopwatch.Stop();
        Console.WriteLine("Tiempo de ejecución (paralelo): " + stopwatch.ElapsedMilliseconds + " ms");
    }

    static void ComputeDFT(Complex[] signal, int start, int end)
    {
        int N = signal.Length;
        for (int k = start; k < end; k++)
        {
            Complex sum = new Complex(0, 0);
            for (int n = 0; n < N; n++)
            {
                double angle = -2 * Math.PI * k * n / N;
                sum += signal[n] * new Complex(Math.Cos(angle), Math.Sin(angle));
            }
            lock (lockObject)
            {
                result[k] = sum;
            }
        }
    }
}
