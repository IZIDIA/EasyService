using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace EasyService.Models {
	class Options {
		[JsonProperty("company_name")]
		public string CompanyName { get; set; }

		[JsonProperty("welcome_text_app")]
		public string WelcomeTextApp { get; set; }
	}
}
