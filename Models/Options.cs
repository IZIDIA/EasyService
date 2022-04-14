using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasyService.Models {
	class Options {
		[JsonPropertyName("company_name")]
		public string CompanyName { get; set; }

		[JsonPropertyName("welcome_text_app")]
		public string WelcomeTextApp { get; set; }
	}
}
