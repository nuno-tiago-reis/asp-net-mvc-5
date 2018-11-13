﻿using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The verify two factor authentication code view model. TODO
	/// </summary>
	public class VerifyCodeViewModel
	{
		[Required]
		[Display(Name = "Provider")]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }

		[Required]
		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		[Required]
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}
}