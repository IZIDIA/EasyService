using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasyService {
	public class RequestInfo {
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("admin")]
		public string Admin { get; set; }

		[JsonPropertyName("from_pc")]
		public int FromPc { get; set; }

		[JsonPropertyName("mac")]
		public string Mac { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("email")]
		public string Email { get; set; }

		[JsonPropertyName("ip_address")]
		public string IpAddress { get; set; }

		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		[JsonPropertyName("inventory_number")]
		public string InventoryNumber { get; set; }

		[JsonPropertyName("location")]
		public string Location { get; set; }

		[JsonPropertyName("phone_call_number")]
		public string PhoneCallNumber { get; set; }

		[JsonPropertyName("solution_with_me")]
		public string SolutionWithMe { get; set; }

		[JsonPropertyName("problem_with_my_pc")]
		public int ProblemWithMyPc { get; set; }

		[JsonPropertyName("work_time")]
		public string WorkTime { get; set; }

		[JsonPropertyName("user_password")]
		public string UserPassword { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("photo")]
		public string Photo { get; set; }

		[JsonPropertyName("comments")]
		public string Coments { get; set; }

		[JsonPropertyName("beauty_closed_at")]
		public string ClosedAt { get; set; }

		[JsonPropertyName("beauty_created_at")]
		public string CreatedAt { get; set; }

		public string Icon { get; set; }

		public string IconColor { get; set; }

		public string IdName { get; set; }
	}
}
