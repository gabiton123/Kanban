using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanBoardWithSignalRAngularJSSol.Models
{
	public class SubTasks
	{
		public int Id{ get; set; }
		public int IdTask{ get; set; }
		public string Subtask { get; set; }
		public DateTime HoraInclusao { get; set; }
	}
}