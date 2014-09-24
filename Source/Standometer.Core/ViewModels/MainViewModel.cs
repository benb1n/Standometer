using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Standometer.Core.Models;

namespace Standometer.Core.ViewModels
{
	public class MainViewModel : MvxViewModel
	{

		
		#region StatusEvents

		private ObservableCollection<StatusChangeEvent> statusEvents = new ObservableCollection<StatusChangeEvent>();
		public ObservableCollection<StatusChangeEvent> StatusEvents
		{
			get { return statusEvents; }
			set { statusEvents = value; RaisePropertyChanged(() => StatusEvents); }
		}

		#endregion
	}
}
