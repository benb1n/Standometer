using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Cirrious.MvvmCross.Wpf.Views;
using Microsoft.Win32;
using Standometer.Core.Models;

namespace Standometer.UI.WPF.Views
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : MvxWpfView
	{
		private const int standing_threshold = 100;

		public ObservableCollection<StatusChangeEvent> events = new ObservableCollection<StatusChangeEvent>();
		SerialPort serialPort;
		Status currentStatus = Status.Unknown;

		public MainView()
		{
			InitializeComponent();

			SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
			this.Loaded += MainWindow_Loaded;
			this.Unloaded += MainWindow_Unloaded;
		}

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Task.Factory.StartNew(this.MonitorDistance);
		}

		void MainWindow_Unloaded(object sender, RoutedEventArgs e)
		{
			if (serialPort != null)
			{
				serialPort.Close();
			}
		}

		void MonitorDistance()
		{
			serialPort = new SerialPort();
			serialPort.PortName = "COM9";
			serialPort.BaudRate = 9600;
			serialPort.Parity = Parity.None;
			serialPort.DataBits = 8;
			serialPort.ReadTimeout = 5000;

			serialPort.Open();

			bool _continue = true;

			while (_continue)
			{
				try
				{
					string message = serialPort.ReadLine();
					int distanceInCm = -1;
					if (int.TryParse(message, out distanceInCm))
					{
						if (distanceInCm >= standing_threshold && this.currentStatus != Status.Standing)
						{
							this.LogChange(this.currentStatus, Status.Standing);
							currentStatus = Status.Standing;
							this.ToggleArrowVisibility(currentStatus);
						}
						else if (distanceInCm < standing_threshold && this.currentStatus != Status.Sitting)
						{
							this.LogChange(this.currentStatus, Status.Sitting);
							currentStatus = Status.Sitting;
							this.ToggleArrowVisibility(currentStatus);
						}
						UpdateDistanceDisplay(distanceInCm.ToString());
					}
					else
					{
						UpdateDistanceDisplay("???");
					}
				}
				catch (TimeoutException)
				{
					UpdateDistanceDisplay("---");
				}
			}
		}

		void UpdateDistanceDisplay(string displayText)
		{
			Application.Current.Dispatcher.Invoke(
							DispatcherPriority.Send,
							(DispatcherOperationCallback)(arg =>
							{
								DistanceTextBlock.Text = displayText;
								return null;
							}), null);
		}

		void ToggleArrowVisibility(Status newStatus)
		{
			Application.Current.Dispatcher.Invoke(
							DispatcherPriority.Send,
							(DispatcherOperationCallback)(arg =>
							{
								if (newStatus == Status.Sitting)
								{
									this.UpArrow.Visibility = System.Windows.Visibility.Collapsed;
									this.DownArrow.Visibility = System.Windows.Visibility.Visible;
								}
								else if (newStatus == Status.Standing)
								{
									this.UpArrow.Visibility = System.Windows.Visibility.Visible;
									this.DownArrow.Visibility = System.Windows.Visibility.Collapsed;
								}
								return null;
							}), null);
		}

		void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			if (e.Reason == SessionSwitchReason.SessionLogon || e.Reason == SessionSwitchReason.SessionUnlock)
			{
				this.LogEvent(StartStop.Start, this.currentStatus);
			}
			else if (e.Reason == SessionSwitchReason.SessionLock || e.Reason == SessionSwitchReason.SessionLogoff)
			{
				this.LogEvent(StartStop.Stop, this.currentStatus);
			}
		}

		void LogEvent(StartStop startOrStop, Status status)
		{
			this.events.Add(new StatusChangeEvent(DateTime.Now, startOrStop, status));

			Console.WriteLine(string.Format("Logging event at {0} for {1} {2}", DateTime.Now, startOrStop, status));
		}

		void LogChange(Status oldStatus, Status newStatus)
		{
			this.LogEvent(StartStop.Stop, oldStatus);
			this.LogEvent(StartStop.Start, newStatus);
		}
	}
}
