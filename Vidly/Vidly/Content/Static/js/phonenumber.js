/// <reference path="../../Libraries/jquery/js/jquery.js" />
/// <reference path="../../Libraries/intl-tel-input/js/intlTelInput-jquery.js" />

/**
 * Provides functionality to handle phone number inputs
 *
 * Exposes one method:
 * - initializePhoneNumberInput
 */
var phoneNumberModule = (function()
{
	'use strict';

	/**
	 * [PUBLIC]
	 *
	 * Initializes the phone number input.
	 *
	 * @param {any} phoneNumberName
	 */
	function initializePhoneNumber(phoneNumberName)
	{
		const phoneNumber = document.querySelector(`#${phoneNumberName}`);

		window.intlTelInput
		(
			phoneNumber,
			{
				initialCountry: "auto",
				geoIpLookup: function(callback)
				{
					jQuery.get("https://ipinfo.io", function() {}, "jsonp").always(function(response)
					{
						const countryCode = (response && response.country) ? response.country : "pt";
						callback(countryCode);
					});
				},
				utilsScript: "../../Content/Libraries/intl-tel-input/js/utils.js"
			}
		);
	}

	/**
	 * [PUBLIC]
	 *
	 * Initializes the phone number input.
	 *
	 * @param {any} phoneNumberName
	 * @param {any} hiddenPhoneNumberName
	 */
	function initializePhoneNumberInput(phoneNumberName, hiddenPhoneNumberName)
	{
		const phoneNumber = document.querySelector(`#${phoneNumberName}`);

		window.intlTelInput
		(
			phoneNumber,
			{
				initialCountry: "auto",
				hiddenInput: `${hiddenPhoneNumberName}`,
				geoIpLookup: function(callback)
				{
					jQuery.get("https://ipinfo.io", function() {}, "jsonp").always(function(response)
					{
						const countryCode = (response && response.country) ? response.country : "pt";
						callback(countryCode);
					});
				},
				utilsScript: "../../Content/Libraries/intl-tel-input/js/utils.js"
			}
		);
	}
	
	return {
		initializePhoneNumber: initializePhoneNumber,
		initializePhoneNumberInput: initializePhoneNumberInput
	};
}());