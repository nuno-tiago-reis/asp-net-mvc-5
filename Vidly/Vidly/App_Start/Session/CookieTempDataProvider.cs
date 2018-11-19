
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Vidly
{
	public sealed class CookieTempDataProvider : ITempDataProvider
	{
		/// <summary>
		/// The cookie key.
		/// </summary>
		private const string CookieKey = "VidlyCookie";

		/// <inheritdoc />
		public IDictionary<string, object> LoadTempData(ControllerContext context)
		{
			var cookie = context.HttpContext.Request.Cookies[CookieKey];
			if (cookie == null)
				return new Dictionary<string, object>();

			string json = Encoding.UTF8.GetString(Convert.FromBase64String(cookie.Value));

			var dictionary = JsonConvert.DeserializeObject<IDictionary<string, object>>
			(
				json,
				new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All,
					TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
				}
			);

			// Expire the cookie
			cookie.Expires = DateTime.Now.AddDays(-1d); 
			context.HttpContext.Response.SetCookie(cookie);

			return dictionary;
		}

		/// <inheritdoc />
		public void SaveTempData(ControllerContext context, IDictionary<string, object> values)
		{
			var cookie = context.HttpContext.Request.Cookies[CookieKey];
			if (cookie == null)
			{
				cookie = new HttpCookie(CookieKey);
				context.HttpContext.Response.Cookies.Add(cookie);
			}

			if (values == null || values.Count == 0)
			{
				// Expire the cookie
				cookie.Expires = DateTime.Now.AddDays(-1d);
				context.HttpContext.Response.SetCookie(cookie);

				return;
			}

			string json = JsonConvert.SerializeObject
			(
				values,
				new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All,
					TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
				}
			);

			string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

			context.HttpContext.Response.SetCookie(new HttpCookie(CookieKey, encoded));
		}
	}
}