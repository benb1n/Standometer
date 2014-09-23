using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standometer.UI.WPF
{
	class StatusChangeEvent
	{
		public StatusChangeEvent(DateTime eventDate, StartStop startOrStop, Status status)
		{
			this.EventDate = eventDate;
			this.StartOrStop = startOrStop;
			this.Status = status;
		}

		public DateTime EventDate { get; private set; }
		public StartStop StartOrStop { get; private set; }
		public Status Status { get; private set; }
	}

	enum StartStop
	{
		Start,
		Stop
	}

	enum Status
	{
		Unknown,
		Standing,
		Sitting
	}
}
