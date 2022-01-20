using Arction.Wpf.Charting;
using Arction.Wpf.Charting.SeriesXY;
using Arction.Wpf.Charting.Views.ViewXY;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LightningChartTest
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
        List<LightningChart> LightningChartList = new List<LightningChart>();

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

            LightningChartList.Add(lightningChart1);
            LightningChartList.Add(lightningChart2);
            LightningChartList.Add(lightningChart3);
            LightningChartList.Add(lightningChart4);
            LightningChartList.Add(lightningChart5);
            LightningChartList.Add(lightningChart6);
            LightningChartList.Add(lightningChart7);
            LightningChartList.Add(lightningChart8);

            foreach (var lightningChart in LightningChartList)
            {
                // chart view.
                var view = lightningChart.ViewXY;
                // X축.
                var xAxes = view.XAxes[0];
                // Y축.
                var yAxes = view.YAxes[0];

                // 스크롤 여부 & 창에 보여지는 범위 설정.
                xAxes.ScrollMode = XAxisScrollMode.Scrolling;
                xAxes.SetRange(0, 50);

                // 차트 딩 라인수.
                int lineCount = 5;

                // 차트에 라인 생성.
                for (int i = 0; i < lineCount; i++)
                    view.PointLineSeries.Add(new PointLineSeries(view, xAxes, yAxes));
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

        private void QueueDataToGraph(object sender, RoutedEventArgs e)
        {
            sw.Start();
            
            // 어디에서 호출 되는지 모르겠음..
            CompositionTarget.Rendering += AddDatToChart_Queue;

            timer.Start();
        }

        private void AddDatToChart_Queue(object sender, EventArgs e)
        {
            for (int i = 0; i < LightningChartList.Count; i++)
            {
                LightningChartList[i].BeginUpdate();

                for (int j = 0; j < ChartQueueList[i].Count; j++)
                {
                    var count = ChartQueueList[i][j].Count;
                    SeriesPoint[] series = new SeriesPoint[count];
                    for (int z = 0; z < count; z++)
                    {
                        if (ChartQueueList[i][j].TryDequeue(out int value))
                        {
                            series[z] = new SeriesPoint(ChartCountList[i][j]++, value);
                        }
                    }
                    LightningChartList[i].ViewXY.PointLineSeries[j].AddPoints(series, false);
                    LightningChartList[i].ViewXY.XAxes[0].ScrollPosition = ChartCountList[i][j];
                }

                LightningChartList[i].EndUpdate();
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

            if (ChartCountList[7][4] == 1000)
            {
                sw.Stop();
                CompositionTarget.Rendering -= AddDatToChart_Queue;
                MessageBox.Show($"Max:{maxTime}, Min: {minTime}, Average: {sw.ElapsedTicks / 1000}, Total: {sw.ElapsedTicks}");

            }
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
                    foreach (var lightningChart in LightningChartList)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            lightningChart.BeginUpdate();
                        }));

                        var value = 1;
                        foreach (var series in lightningChart.ViewXY.PointLineSeries)
                        {
                            series.AddPoints(new SeriesPoint[] { new SeriesPoint { X = taskCount, Y = value } }, false);
                            value++;
                        }

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            lightningChart.EndUpdate();
                        }));
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

            #region CompositonTarget.Rendering
            //CompositionTarget.Rendering += AddDatToChart;
            #endregion
        }

        int eventCount = 0;
        private void AddDatToChart(object sender, EventArgs e)
        {
            foreach (var lightningChart in LightningChartList)
            {
                lightningChart.BeginUpdate();

                var value = 1;

                foreach (var series in lightningChart.ViewXY.PointLineSeries)
                {
                    series.AddPoints(new SeriesPoint[] { new SeriesPoint { X = eventCount, Y = value } }, false);
                    value++;
                }

                lightningChart.EndUpdate();
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

            eventCount++;
            if (eventCount == 1000)
            {
                sw.Stop();
                CompositionTarget.Rendering -= AddDatToChart;
                MessageBox.Show($"Max:{maxTime}, Min: {minTime}, Average: {sw.ElapsedTicks / eventCount}, Total: {sw.ElapsedTicks}");

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            base.OnClosed(e);
        }
    }
}
