using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The send two factor authentication code view model. TODO
	/// </summary>
	public class SendCodeViewModel
	{
		[Display(Name = "Provider")]
		public string SelectedProvider { get; set; }

		[Display(Name = "Providers")]
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}
}