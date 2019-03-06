/// <reference path="../../Libraries/jquery/js/jquery.js" />
/// <reference path="../../Libraries/typeahead/js/bloodhound.js" />
/// <reference path="../../Libraries/typeahead/js/typeahead.jquery.js" />

/**
 * Provides functionality to handle typeahead inputs
 *
 * Exposes one method:
 * - initializeTypeAhead
 */
var typeAheadModule = (function()
{
	'use strict';

	/**
	 * [PUBLIC]
	 *
	 * Initializes the typeahead input.
	 *
	 * @param {any} entityName
	 * @param {any} entityApiQuery
	 * @param {any} entityApiQueryField
	 * @param {any} datumName
	 * @param {any} datumInputName
	 * @param {any} minimumLength
	 * @param {any} inputIdentifier
	 * @param {any} inputSelectFunction
	 */
	function initializeApiTypeAhead(entityName, entityApiQuery, entityApiQueryField, datumName, datumInputName, minimumLength, inputIdentifier, inputSelectFunction)
	{
		const bloodhound = new Bloodhound
		({
			datumTokenizer: Bloodhound.tokenizers.obj.whitespace(datumInputName),
			queryTokenizer: Bloodhound.tokenizers.whitespace,
			remote:
			{
				url: entityApiQuery,
				wildcard: entityApiQueryField
			}
		});

		$(`#${inputIdentifier}`).typeahead
		(
			{
				hint: true,
				highlight: true,
				minLength: minimumLength,
				displayKey: datumInputName,
				matcher: function(result) { return result; }
			},
			{
				name: datumName,
				display: datumInputName,
				source: bloodhound
			}
		)
		.on("typeahead:select", inputSelectFunction);
	}

	return {
		initializeApiTypeAheadWithQuery: initializeApiTypeAhead
	};
}());