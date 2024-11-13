using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace PomodoroApp
{


    public class PomodoroTimer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TimeSpan time 
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _breakInterval = new TimeSpan(0, 10, 00);

        public TimeSpan breakInterval
        {
            get { return _breakInterval; }
            set
            {
                if (breakInterval > new TimeSpan(0, 0, 0)) _breakInterval = value;
            }

        }

        private TimeSpan _workInterval=new TimeSpan(01,30,0);

        public TimeSpan workInterval
        {
            get { return _workInterval; }
            set
            {
                if(workInterval>new TimeSpan(0,0,0)) _workInterval = value;        
            }

        }

        private TimeSpan _time= new TimeSpan(01, 30, 0);



        private Stopwatch _stopwatch = new Stopwatch();

        private Boolean workChecked = true;

        public PomodoroTimer()
        {
            TimerScheduler();
        }
               

        private bool TimerEnd()
        {
            return time <= new TimeSpan(0, 0, 0);
        }

        public void startTimer()
        {
            _stopwatch.Start();            
        }

        public void stopTimer()
        {
            _stopwatch.Stop();
        }

        public void resetTimer(Boolean WorkChecked)
        {
            workChecked = WorkChecked;
            _stopwatch.Reset();
        }

        private void TimerScheduler()
        {
            new Task(() => {

                while (true)
                {
                    Thread.Sleep(300);
                    SetTime();
                    if(TimerEnd()) start_NextInterval();
                }

            }).Start();
        }

        private void SetTime()
        {

            if (workChecked) time = workInterval - _stopwatch.Elapsed;
            else time = breakInterval - _stopwatch.Elapsed;
        }

        private void start_NextInterval()
        {
            resetTimer(!workChecked);
            SetTime();
            startTimer();
        }
       

    }
}
