using System.Diagnostics;

const int MillisToWait = 2000;
const int WaitingWorkers = 50;

(bool processed, bool isThreadPoolThread)[] status = new (bool, bool)[WaitingWorkers];

Measure(PerformAsynchronousOperations, 5);
Console.WriteLine();
Measure(PerformSynchronousOperations, 5);

void Measure(Action launchWorkers, int numberOfMeasurements)
{
    Console.WriteLine($"Measuring execution time with {launchWorkers.Method.Name} usage.");

    var times = new List<long>(numberOfMeasurements);

    for (int i = 0; i < numberOfMeasurements; i++)
    {
        ResetFlags();
        var watch = Stopwatch.StartNew();

        launchWorkers();

        Console.WriteLine("\t[{0} ms] Processing started with {1} threads in thread pool",
            watch.ElapsedMilliseconds.ToString().PadLeft(5),
            ThreadPool.ThreadCount);

        WaitUntilWorkersFinishTheirJobs();

        watch.Stop();
        Console.WriteLine("\t[{0} ms] work is finished. All threads come from thread pool:{1}",
            watch.ElapsedMilliseconds.ToString().PadLeft(5),
            status.All(x => x.isThreadPoolThread));
        Console.WriteLine();

        times.Add(watch.ElapsedMilliseconds);
    }

    Console.WriteLine("Average execution time for {0} method: {1} ms.",
        launchWorkers.Method.Name,
        times.Average());
    Console.WriteLine();
}

void PerformSynchronousOperations()
{
    for (int i = 0; i < WaitingWorkers; i++)
    {
        int index = i;
        Task.Run(() =>
        {
            Thread.Sleep(MillisToWait);
            SetFlags(index);
        });
    }
}

void PerformAsynchronousOperations()
{
    for (int i = 0; i < WaitingWorkers; i++)
    {
        int index = i;
        Task.Run(async () =>
        {
            await Task.Delay(MillisToWait);
            SetFlags(index);
        });
    }
}

void SetFlags(int index) 
    => status[index] = (processed: true, isThreadPoolThread: Thread.CurrentThread.IsThreadPoolThread);

void ResetFlags()
{
    for (int i = 0; i < WaitingWorkers; i++)
    {
        status[i] = (false, false);
    }
}

void WaitUntilWorkersFinishTheirJobs()
{
    while (true)
    {
        if (status.All(x => x.processed))
        {
            break;
        }
    }
}