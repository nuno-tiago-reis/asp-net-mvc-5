/// <reference path='../../Libraries/jquery/js/jquery.js' />
/// <reference path='../../Libraries/bootbox/js/bootbox.js' />
/// <reference path='../../Libraries/alertify/build/alertify.js' />

/**
 * Provides functionality to handle index pages.
 *
 * Exposes one method:
 * - initializeIndex
 */
var indexModule = (function()
{
	'use strict';

	/**
	 * [PUBLIC]
	 *
	 * Initializes the table and the delete buttons.
	 *
	 * @param {any} entityName
	 * @param {any} capitalizedEntityName
	 * @param {any} entityApi
	 * @param {any} entityColumns
	 */
	function initializeIndex(entityName, capitalizedEntityName, entityApi, entityColumns, completeFunction = null)
	{
		const entityTable = $(`#${entityName}s`);

		var table = entityTable.DataTable
		({
			ajax:
			{
				url: entityApi,
				dataSrc: ''
			},
			columns: entityColumns,
			'initComplete': completeFunction,
			'drawCallback': function ()
			{
				$('[data-toggle="tooltip"]').tooltip();
			},
		});

		entityTable.on('click', '.js-delete', function ()
		{
			var button = $(this);
			var buttonDataId = button.attr(`data-${entityName}-id`);

			bootbox.confirm
			({
				title: (`Delete ${capitalizedEntityName}`),
				message: (`Are you sure you want to delete this ${entityName}?`),
				buttons:
				{
					confirm:
					{
						label: 'Delete',
						className: 'btn-danger mr-1'
					},
					cancel:
					{
						label: 'Cancel',
						className: 'btn-primary ml-1'
					}
				},
				callback: function(result)
				{
					if (result)
					{
						$.ajax
						({
							url: `${entityApi}${buttonDataId}`,
							method: 'DELETE',

							success: function ()
							{
								table.row(button.parents('tr')).remove().draw();
								alertModule.success(`${capitalizedEntityName} successfully deleted.`);
							},

							error: function(xhr)
							{
								const error = JSON.parse(xhr.responseText);
								alertModule.error(`An error occurred:${error.message}`);
							}
						});
					}
				}
			});
		});
	}

	return {
		initializeIndex: initializeIndex
	};
}());