using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBenchmark
{
    public class Runner : IDisposable
    {

        public const int WIDTH = 75;

        public string Name { get; set; }

        public Counter Success { get; private set; } = new Counter("Success");

        public Counter Error { get; private set; } = new Counter("Error");

        public Counter Fail { get; private set; } = new Counter("Fail");

        public bool Status { get; set; } = false;

        public List<ITester> Testers { get; private set; } = new List<ITester>();

        public int Interval { get; set; }

        private TimesStatistics _timesStatistics = new TimesStatistics();

        public Action<ITester, Exception> OnError { get; set; }

        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        private async Task OnPreheating(ITester item)
        {
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    await item.Execute();
                }
                catch (Exception e_)
                {
                    try
                    {
                        OnError?.Invoke(item, e_);
                    }
                    catch { }
                }

            }
        }

        private async void OnRunItem(ITester item)
        {
            while (Status)
            {
                var startTime = stopwatch.ElapsedMilliseconds;
                long time = 0;
                try
                {
                    var result = await item.Execute();
                    time = stopwatch.ElapsedMilliseconds - startTime;
                    if(result)
                    {
                        Success.Add(1, time);
                    }
                    else
                    {
                        Fail.Add(1, time);
                    }
                }
                catch (Exception e_)
                {
                    time = stopwatch.ElapsedMilliseconds - startTime;
                    Error.Add(1, time);
                    try
                    {
                        OnError?.Invoke(item, e_);
                    }
                    catch { }
                }
                finally
                {
                    _timesStatistics.Add(time);
                }
            }
        }

        private void OnStatistics(object state)
        {
            if (Status)
                this.Print();
        }

        private System.Threading.Timer _statisticsTimer;
        private List<Task> _tasks;

        private void Print()
        {
            string value = "NetBenchmark";
            Console.Clear();
            Console.SetWindowPosition(0, 0);
            Console.WriteLine("-".PadRight(WIDTH, '-'));
            int span = WIDTH / 2 - value.Length / 2;
            Console.WriteLine("".PadLeft(span) + value);

            value = Name;
            span = 70 / 2 - value.Length / 2;
            Console.WriteLine("".PadLeft(span) + value);

            value = $"{stopwatch.Elapsed}";
            span = WIDTH / 2 - value.Length / 2;
            Console.WriteLine("".PadLeft(span) + value);

            Console.WriteLine("-".PadRight(WIDTH, '-'));
            Console.Write("|");
            value = $"Name|".PadLeft(18);
            Console.Write(value);

            value = $"Max|".PadLeft(10);
            Console.Write(value);

            value = $"Avg|".PadLeft(10);
            Console.Write(value);

            value = $"Min|".PadLeft(10);
            Console.Write(value);

            value = $"Total|".PadLeft(26);
            Console.Write(value);
            Console.WriteLine("");

            Console.WriteLine("-".PadRight(WIDTH, '-'));
            Success.Print();
            Fail.Print();
            Error.Print();
            Console.WriteLine("-".PadRight(WIDTH, '-'));
            _timesStatistics.Print();
        }

        public async void Run()
        {
            Status = true;
            foreach (var item in Testers)
            {
                await OnPreheating(item);
            }
            stopwatch.Restart();
            _tasks = new List<Task>();
            foreach (var item in Testers)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    if (Status)
                        OnRunItem(item);
                    Thread.Sleep(Interval);
                });
                _tasks.Add(task);
            }
            _statisticsTimer = new System.Threading.Timer(OnStatistics, null, 1000, 1000);
        }

        public void Stop()
        {
            Status = false;
            if (_statisticsTimer != null)
                _statisticsTimer.Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _statisticsTimer?.Dispose();
                    if (_tasks != null)
                    {
                        foreach(var task in _tasks)
                        {
                            task.Dispose();
                        }
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
