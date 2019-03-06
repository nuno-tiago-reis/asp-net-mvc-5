/// <reference path="../../../Libraries/jquery/js/jquery.js" />

$(document).ready(function ()
{
	const columns =
	[
		{
			data: "id"
		},
		{
			data: "movie.name"
		},
		{
			data: "customer.name"
		},
		{
			render: function(data, type, rental) { return moment(rental.dateRented).format('YYYY-MM-DD HH:mm:ss'); }
		},
		{
			render: function(data, type, rental)
			{
				if (rental.dateReturned != null)
					return moment(rental.dateReturned).format('YYYY-MM-DD HH:mm:ss');

				return null;
			}
		},
		{
			render: function(data, type, rental)
			{
				if (rental.dateReturned == null)
					return `<button class='btn btn-primary js-return' data-rental-id='${rental.id}'>Return</button>`;
				else
					return `<button disabled='true' data-toggle='tooltip' title='The rental has been returned.' class='btn btn-default js-return' data-rental-id='${rental.id}'>Return</button>`;
			}
		},
		{
			render: function(data, type, rental) { return `<button class='btn btn-danger js-delete' data-rental-id='${rental.id}'>Delete</button>`; }
		}
	];

	// Initialize the table
	indexModule.initializeIndex('rental', 'Rental', '/api/rentals/', columns);

	// Initialize the return button
	$('#rentals').on('click', '.js-return', function ()
	{
		var button = $(this);

		bootbox.confirm('Are you sure you want to return this rental?', function (result)
		{
			if (result)
			{
				// ReSharper disable once StringLiteralTypo
				var dateReturned = moment().format('YYYY-MM-DDTHH:mm:ss');

				$.ajax
				({
					url: `/api/rentals/${button.attr('data-rental-id')}`,
					method: 'PUT',
					contentType: 'application/json; charset=utf-8',
					data: JSON.stringify({ id: button.attr('data-rental-id'), dateReturned: dateReturned }),

					success: function ()
					{
						button.prop('disabled', true);
						button.attr('data-toggle', 'tooltip');
						button.attr('title', 'The rental has been returned.');

						const row = table.row(button.parents('tr'));

						const data = row.data();
						data['dateReturned'] = dateReturned;

						alertModule.success('Rental successfully returned.');
						table.row(row).data(data).draw();
					},

					error: function (xhr)
					{
						const error = JSON.parse(xhr.responseText);
						alertModule.error(`An error occurred:${error.message}`);
					}
				});
			}
		});
	});
});