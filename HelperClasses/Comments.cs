using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EasyService.HelperClasses {
	public partial class Comments {
		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Time")]
		public string Time { get; set; }

		[JsonProperty("Message")]
		public string Message { get; set; }
	}
}
