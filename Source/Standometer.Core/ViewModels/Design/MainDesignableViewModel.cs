using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standometer.Core.ViewModels.Design
{
	public class MainDesignableViewModel : MainViewModel
	{
		public MainDesignableViewModel()
		{
			this.StatusEvents.Add(new Models.StatusChangeEvent(DateTime.Now.AddHours(-8), Models.StartStop.Start, Models.Status.Standing));
			this.StatusEvents.Add(new Models.StatusChangeEvent(DateTime.Now.AddHours(-5), Models.StartStop.Stop, Models.Status.Standing));
		}
	}
}
