using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Timers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PomodoroApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        PomodoroTimer pomodoroTimer=new PomodoroTimer();

        public MainWindow()
        {   
           
            InitializeComponent();
            rb_work.IsChecked = true;
            tb_timer.DataContext = pomodoroTimer;
            this.Topmost = true;
  
        }


        private void bt_start_Click(object sender, RoutedEventArgs e)
        {
            StartOrStopTimer();
            StartButton_ChangeIcon();
           

        }

        private void bt_reset_Click(object sender, RoutedEventArgs e)
        {
            pomodoroTimer.resetTimer((bool)rb_work.IsChecked);
            bt_start.Content = FindResource("start");
        }

        private void bt_edit_Click(object sender, RoutedEventArgs e)
        {
            Boolean editMode = bt_edit.Content == FindResource("edit");
            Boolean InputIsValid = inputIsValid();

            EditButton_ChangeIcon(editMode, InputIsValid);
            EnabaleOrDisable_Interval(editMode, InputIsValid);
            saveInput(editMode, InputIsValid);
        }

        private void rb_work_Checked(object sender, RoutedEventArgs e)
        {
            load_workInterval();
        }

        private void rb_break_Checked(object sender, RoutedEventArgs e)
        {
            load_breakInterval();
        }

        private void StartButton_ChangeIcon()
        {
            if (bt_start.Content == FindResource("start")) bt_start.Content = FindResource("stop");
            else bt_start.Content = FindResource("start");

        }
        private void StartOrStopTimer()
        {
            if (bt_start.Content == FindResource("start")) pomodoroTimer.startTimer();
            else pomodoroTimer.stopTimer();

        }

        private void EditButton_ChangeIcon(Boolean editMode, Boolean inputIsValid)
        {
            if (editMode) bt_edit.Content = FindResource("save");
            else if (inputIsValid) bt_edit.Content = FindResource("edit");        
   
        }

        private void EnabaleOrDisable_Interval(Boolean editMode, Boolean inputIsValid)
        {
            if (editMode) enable_Inteval(); 
            else if (inputIsValid) disable_Interval();

        }

        private void saveInput(Boolean editMode, Boolean inputIsValid)
        {
            if (inputIsValid)
            {                
                save_UI_Interval();      
                lb_invalid.Visibility = Visibility.Hidden;
            }
            else lb_invalid.Visibility = Visibility.Visible;
        }

        private void enable_Inteval()
        {
            bt_hour.IsEnabled = true;
            bt_min.IsEnabled = true;
            bt_sec.IsEnabled = true;
        }

        private void disable_Interval()
        {
            bt_hour.IsEnabled = false;
            bt_min.IsEnabled = false;
            bt_sec.IsEnabled = false;
        }

        private Boolean inputIsValid()
        {        
            Regex NumberTo60 = new Regex("^([0-5]?[0-9]|60)$");
            Boolean secIsValid = NumberTo60.IsMatch(bt_sec.Text);
            Boolean minIsValid = NumberTo60.IsMatch(bt_min.Text);
            Boolean hourIsValid = NumberTo60.IsMatch(bt_hour.Text);

            Boolean UI_inputIsValid = secIsValid && minIsValid && hourIsValid;

            return UI_inputIsValid;
        }

        private void save_UI_Interval()
        {
           if((bool)rb_work.IsChecked) pomodoroTimer.workInterval = UI_Interval();
           else pomodoroTimer.breakInterval = UI_Interval();
        }

        private TimeSpan UI_Interval()
        {
           return new TimeSpan(Convert.ToInt32(bt_hour.Text), Convert.ToInt32(bt_min.Text), Convert.ToInt32(bt_sec.Text));
        }

        private void load_breakInterval()
        {
            bt_sec.Text = pomodoroTimer.breakInterval.Seconds.ToString();
            bt_min.Text = pomodoroTimer.breakInterval.Minutes.ToString();
            bt_hour.Text = pomodoroTimer.breakInterval.Hours.ToString();
        }

        private void load_workInterval()
        {
            bt_sec.Text = pomodoroTimer.workInterval.Seconds.ToString();
            bt_min.Text = pomodoroTimer.workInterval.Minutes.ToString();
            bt_hour.Text = pomodoroTimer.workInterval.Hours.ToString();
        }


        private void bt_minimize_Click(object sender, RoutedEventArgs e)
        {
           WindowState = WindowState.Minimized;
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
