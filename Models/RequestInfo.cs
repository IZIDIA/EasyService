using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService {
	public class RequestInfo {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("admin")]
		public string Admin { get; set; }

		[JsonProperty("from_pc")]
		public int FromPc { get; set; }

		[JsonProperty("mac")]
		public string Mac { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("ip_address")]
		public string IpAddress { get; set; }

		[JsonProperty("topic")]
		public string Topic { get; set; }

		[JsonProperty("inventory_number")]
		public string InventoryNumber { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("phone_call_number")]
		public string PhoneCallNumber { get; set; }

		[JsonProperty("solution_with_me")]
		public string SolutionWithMe { get; set; }

		[JsonProperty("problem_with_my_pc")]
		public int ProblemWithMyPc { get; set; }

		[JsonProperty("work_time")]
		public string WorkTime { get; set; }

		[JsonProperty("user_password")]
		public string UserPassword { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("photo")]
		public string Photo { get; set; }

		[JsonProperty("comments")]
		public string Coments { get; set; }

		[JsonProperty("beauty_closed_at")]
		public string ClosedAt { get; set; }

		[JsonProperty("beauty_created_at")]
		public string CreatedAt { get; set; }

		public string Icon { get; set; }

		public string IconColor { get; set; }

		public string IdName { get; set; }
	}
}
