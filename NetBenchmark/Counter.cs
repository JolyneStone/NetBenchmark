using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetBenchmark
{
    public class Counter
    {
        public Counter(string name)
        {
            Name = name;
        }

        public string Name { get; set; }


        private long _max;

        private long _min;

        private long _totalCount;

        private long _totalTime;

        public long Avg
        {
            get
            {
                if (_totalCount > 0)
                {
                    return _totalTime / _totalCount;
                }

                return 0;
            }
        }

        public long Max => _max;

        public long Min => _min;

        public long TotalTime => _totalTime;

        public long TotalCount => _totalCount;

        public void Add(long count, long time)
        {
            Interlocked.Add(ref _totalCount, count);
            Interlocked.Add(ref _totalTime, time);

            if (_max < time)
            {
                Interlocked.Exchange(ref _max, time);
            }
            if (_min > time)
            {
                Interlocked.Exchange(ref _min, time);
            }
        }

        public void Print()
        {
            Console.Write("|");
            var value = $"{Name}|".PadLeft(18);
            Console.Write(value);

            value = $"{Max:###,###,##0}|".PadLeft(10);
            Console.Write(value);

            value = $"{Avg:###,###,##0}|".PadLeft(10);
            Console.Write(value);

            value = $"{Min:###,###,##0}|".PadLeft(10);
            Console.Write(value);

            value = $"{TotalCount:###,###,##0}|".PadLeft(26);
            Console.Write(value);
            Console.WriteLine("");

        }
    }
}
