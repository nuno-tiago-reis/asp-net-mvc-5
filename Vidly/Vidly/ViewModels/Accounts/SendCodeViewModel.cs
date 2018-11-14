using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The send two factor authentication code view model.
	/// </summary>
	public class SendCodeViewModel
	{
		[Required]
		[Display(Name = "Provider")]
		public string SelectedProvider { get; set; }

		[Display(Name = "Providers")]
		public ICollection<SelectListItem> Providers { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		[Display(Name = "Return URL")]
		public string ReturnUrl { get; set; }
	}
}