﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NetBenchmark
{
    public class TimesStatistics
    {

        private long _count;

        public long Count => _count;

        public List<TimeConuterItem> Items { get; private set; } = new List<TimeConuterItem>();

        public TimesStatistics()
        {
            TimeConuterItem item = new TimeConuterItem("1", "<1ms");
            Items.Add(item);

            item = new TimeConuterItem("1_5", "1-5ms");
            Items.Add(item);

            item = new TimeConuterItem("1_10", "1-10ms");
            Items.Add(item);

            item = new TimeConuterItem("10_50", "10ms-50ms");
            Items.Add(item);

            item = new TimeConuterItem("50_100", "50ms-100ms");
            Items.Add(item);

            item = new TimeConuterItem("100_200", "100ms-200ms");
            Items.Add(item);

            item = new TimeConuterItem("200_500", "200ms-500ms");
            Items.Add(item);

            item = new TimeConuterItem("500_1000", "500ms-1s");
            Items.Add(item);

            item = new TimeConuterItem("1000_2000", "1s-2s");
            Items.Add(item);

            item = new TimeConuterItem("2000_5000", "2s-5s");
            Items.Add(item);

            item = new TimeConuterItem("5000", ">5s");
            Items.Add(item);
        }

        public void Print()
        {
            foreach (var item in Items)
            {
                if (item.Count == 0)
                    continue;
                var p = (int)(((double)item.Count / (double)Count) * 10000);
                if (p < 0)
                    p = 0;
                var pe = (double)p / 100;
                int pcount = (int)(pe / 5);
                Console.Write('|');
                string value = $"{item.DisplayName} ".PadLeft(19);
                Console.Write(value);
                Console.Write($"{item.Count:###,###,##0}  ".PadLeft(20));
                Console.Write("[");
                value = "".PadLeft(pcount, '=');
                value = value.PadRight(20, ' ');
                Console.Write(value);
                Console.Write("]");
                Console.Write($"{pe}%|".PadLeft(13));
              
                Console.WriteLine("");

            }
        }

        public void Add(long time)
        {
            System.Threading.Interlocked.Increment(ref _count);

            if (time < 1)
            {
                Items[0].Add();
            }
            else if (time < 5)
            {
                Items[1].Add();
            }
            else if (time < 10)
            {
                Items[2].Add();
            }
            else if (time >= 10 && time < 50)
            {
                Items[3].Add();
            }
            else if (time >= 50 && time < 100)
            {
                Items[4].Add();
            }
            else if (time >= 100 && time < 200)
            {
                Items[5].Add();
            }
            else if (time >= 200 && time < 500)
            {
                Items[6].Add();
            }
            else if (time >= 500 && time < 1000)
            {
                Items[7].Add();
            }
            else if (time >= 1000 && time < 2000)
            {
                Items[8].Add();
            }
            else if (time >= 2000 && time < 5000)
            {
                Items[9].Add();
            }
            else if (time >= 5000)
            {
                Items[10].Add();
            }
        }
        public class TimeConuterItem
        {
            public TimeConuterItem(string name, string displayName = null)
            {
                Name = name;
                if (displayName == null)
                    displayName = name;
                DisplayName = displayName;
            }

            public string DisplayName { get; set; }

            public string Name { get; set; }

            private int mCount;

            public int Count => mCount;

            public void Add()
            {
                System.Threading.Interlocked.Increment(ref mCount);
            }
        }
    }
}
