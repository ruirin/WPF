using LiveCharts.Wpf;
using LiveCharts;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using LiveCharts.Geared;
using System.Timers;
using System.Threading.Tasks;
using System;

namespace LiveChartTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch sw = new Stopwatch();
        long minTime = long.MaxValue;
        long maxTime = long.MinValue;
        long? preTime;

        int dataTime = 10;
        List<CartesianChart> LiveChartList = new List<CartesianChart>();

        List<List<ConcurrentQueue<int>>> ChartQueueList = new List<List<ConcurrentQueue<int>>>();
        List<List<double>> ChartCountList = new List<List<double>>()
        {
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
            new List<double>{0,0,0,0,0},
        };
        public MainWindow()
        {
            InitializeComponent();

            LiveChartList.Add(liveChart1);
            LiveChartList.Add(liveChart2);
            LiveChartList.Add(liveChart3);
            LiveChartList.Add(liveChart4);
            LiveChartList.Add(liveChart5);
            LiveChartList.Add(liveChart6);
            LiveChartList.Add(liveChart7);
            LiveChartList.Add(liveChart8);

            foreach(var liveChart in LiveChartList)
            {
                // 차트 딩 라인수.
                int lineCount = 5;

                SeriesCollection SeriesCollection = new SeriesCollection();

                // 차트에 라인 생성.
                for (int i = 0; i < lineCount; i++)
                    SeriesCollection.Add(new GLineSeries { Values = new GearedValues<int>().WithQuality(Quality.High) });

                liveChart.Series = SeriesCollection;
            }

            timer.Interval = dataTime;
            timer.Elapsed += Timer_Elapsed;

            CreateQueue();
        }

        Timer timer = new System.Timers.Timer();

        int dataCount = 0;
        int queueData;
        bool oneOrTwo = true;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            dataCount++;

            if (dataCount >= 1000)
                timer.Stop();

            if (oneOrTwo)
            {
                queueData = 1;
                oneOrTwo = false;
            }
            else
            {
                queueData = 5;
                oneOrTwo = true;
            }

            foreach (var chartQueue in ChartQueueList)
            {
                foreach (var queue in chartQueue)
                {
                    queue.Enqueue(queueData);
                    queueData++;
                }
                queueData -= 5;

            }
        }

        private void CreateQueue()
        {
            var Chart1Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart1Queue);

            var Chart2Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart2Queue);

            var Chart3Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart3Queue);

            var Chart4Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart4Queue);

            var Chart5Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart5Queue);

            var Chart6Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart6Queue);

            var Chart7Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart7Queue);

            var Chart8Queue = new List<ConcurrentQueue<int>> {
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>(),
                          new ConcurrentQueue<int>()};
            ChartQueueList.Add(Chart8Queue);

        }

        private async void QueueDataToGraph(object sender, RoutedEventArgs e)
        {
            sw.Start();
            timer.Start();
            bool test = true;
            await Task.Run(() =>
            {
                while (test)
                {
                    if(ChartCountList[7][4] == 1000)
                    {
                        test = false;
                    }

                    for (int i = 0; i < LiveChartList.Count; i++)
                    {
                        for (int j = 0; j < ChartQueueList[i].Count; j++)
                        {
                            var count = ChartQueueList[i][j].Count;
                            for (int z = 0; z < count; z++)
                            {
                                if (ChartQueueList[i][j].TryDequeue(out int value))
                                {
                                    LiveChartList[i].Series[j].Values.Add(value);
                                    ChartCountList[i][j]++;
                                }
                            }
                        }
                    }

                    long time = sw.ElapsedTicks;
                    if (preTime != null)
                    {
                        long diff = time - preTime.Value;
                        if (diff > maxTime)
                            maxTime = diff;

                        if (diff < minTime)
                            minTime = diff;
                    }

                    preTime = time;
                }
            });

            sw.Stop();

            MessageBox.Show($"Max:{maxTime}, Min: {minTime}, Average: {sw.ElapsedTicks / 1000}, Total: {sw.ElapsedTicks}");
        }

        // 8 Chart, 5 Series, Loop 1000count.
        private async void LoopTest(object sender, RoutedEventArgs e)
        {
            var taskCount = 1000;

            sw.Start();


            #region Task
            // 에러발생 : 호출 스레드가 잠금을 가지고 있지 않습니다. (해결책 : Dispatcher.Invoke 사용)
            await Task.Run(() =>
            {
                for (int i = 0; i < taskCount; i++)
                {
                    foreach(var liveChart in LiveChartList)
                    {
                        var value = 1;
                        foreach (var series in liveChart.Series)
                        {
                            series.Values.Add(value);
                            value++;
                        }
                    }

                    long time = sw.ElapsedTicks;
                    if (preTime != null)
                    {
                        long diff = time - preTime.Value;
                        if (diff > maxTime)
                            maxTime = diff;

                        if (diff < minTime)
                            minTime = diff;
                    }

                    preTime = time;
                }

                sw.Stop();
                MessageBox.Show($"Max:{maxTime}, Min: {minTime}, Average: {sw.ElapsedTicks / taskCount}, Total: {sw.ElapsedTicks}");
            });
            #endregion
        }

        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            base.OnClosed(e);
        }
    }
}
