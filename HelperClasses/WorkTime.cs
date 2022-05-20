using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.HelperClasses {
	public partial class WorkTime {
		[JsonProperty("friday")]
		public Day Friday { get; set; }

		[JsonProperty("monday")]
		public Day Monday { get; set; }

		[JsonProperty("sunday")]
		public Day Sunday { get; set; }

		[JsonProperty("tuesday")]
		public Day Tuesday { get; set; }

		[JsonProperty("saturday")]
		public Day Saturday { get; set; }

		[JsonProperty("thursday")]
		public Day Thursday { get; set; }

		[JsonProperty("wednesday")]
		public Day Wednesday { get; set; }
	}

	public partial class Day {
		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("from")]
		public string From { get; set; }
	}
}
