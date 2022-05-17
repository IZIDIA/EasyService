using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.HelperClasses {
	public class PersonalData {
		[JsonProperty("FirstName")]
		public string FirstName { get; set; }

		[JsonProperty("LastName")]
		public string LastName { get; set; }

		[JsonProperty("Email")]
		public string Email { get; set; }

		[JsonProperty("Location")]
		public string Location { get; set; }

		[JsonProperty("Phone")]
		public string Phone { get; set; }
	}
}
