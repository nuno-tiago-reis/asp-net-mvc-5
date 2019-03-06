/// <reference path="../../Libraries/jquery/js/jquery.js" />
/// <reference path="../../Libraries/alertify/build/alertify.js" />

/**
 * General configurations.
 */
$(document).ready(function()
{
	const message = $('.alert-message').text();
	const messageType = $('.alert-message-type').text();

	alertModule.alert(messageType, message);
});

/**
 * Provides functionality to show toast notification messages.
 *
 * Exposes three methods:
 * - success
 * - warning
 * - error
 */
var alertModule = (function()
{
	'use strict';

	// Initialize alertify
	alertify.set('notifier','position', 'top-right');

	/**
	 * [PUBLIC]
	 *
	 * Creates a toast notification.
	 *
	 * @param {any} type
	 * @param {any} message
	 */
	function alert(type, message)
	{
		if (type === 'success')
		{
			success(message);
		}
		if (type === 'warning')
		{
			warning(message);
		}
		if (type === 'error')
		{
			error(message);
		}
	}

	/**
	 * [PUBLIC]
	 *
	 * Creates a success toast notification.
	 *
	 * @param {any} message
	 */
	function success(message)
	{
		alertify.success(message);
	}

	/**
	 * [PUBLIC]
	 *
	 * Creates a warning toast notification.
	 *
	 * @param {any} message
	 */
	function warning(message)
	{
		alertify.warning(message);
	}

	/**
	 * [PUBLIC]
	 *
	 * Creates an error toast notification.
	 *
	 * @param {any} message
	 */
	function error(message)
	{
		alertify.error(message);
	}

	return {
		alert: alert,
		success: success,
		warning: warning,
		error: error
	};
}());