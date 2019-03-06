/// <reference path="../../../Libraries/jquery/js/jquery.js" />

$.validator.setDefaults
	({
		ignore: []
	});

$.validator.addMethod("validCustomer", function ()
{
	return $('#customer-id').val() !== "";
});

$.validator.addMethod("atLeastOneMovie", function ()
{
	return $('#movie-id-0').val() != null;
});

$(document).ready(function ()
{
	var movieIDs = [];

	const movieSelectFunction = function (event, movie)
	{
		if ($.inArray(movie.id, movieIDs) === -1)
		{
			movieIDs.push(movie.id);

			const ul = $("#movie-list");

			var li = document.createElement("li");
			li.setAttribute("class", "list-group-item");

			const button = document.createElement("button");
			button.setAttribute("id", "movie-list-remove");
			button.setAttribute("type", "button");
			button.setAttribute("class", "close");
			button.innerHTML = "&times;";
			button.onclick = function ()
			{
				for (let j = 0; j < movieIDs.length; j++)
				{
					if (j > movieIDs.indexOf(movie.id))
						$(`#movie-id-${j}`).attr("id", `movie-id-${j - 1}`);
				}

				movieIDs.splice(movieIDs.indexOf(movie.id), 1);

				if (movieIDs.length === 0)
				{
					$("#movie-id-x").prop("disabled", false);
					$('#movie-list-empty').show();
				}

				li.remove();
			};

			const input = $("#movie-id-x").clone();
			input.attr("id", `movie-id-${movieIDs.length - 1}`);
			input.prop("disabled", false);
			input.val(movie.id);

			for (let i = 0; i < movieIDs.length; i++)
			{
				$(`#movie-id-${i}`).val(movieIDs[i]);
			}

			li.appendChild(input[0]);
			li.append(movie.name);
			li.append(button);

			ul.append(li);

			$("#movie-id-x").prop("disabled", true);
			$('#movie-list-empty').hide();
		}

		$('#movie').typeahead('val', '');

		$('#movie').valid();
		$('#movie-id-0').valid();
	};

	// Initialize the typeahead plugin
	typeAheadModule.initializeApiTypeAheadWithQuery('movie', '/api/movies?query=%QUERY', '%QUERY', 'movies', 'name', 3, 'movie', movieSelectFunction);

	const customerSelectFunction = function (event, customer)
	{
		$('#customer-id').val(customer.id);
		$('#customer-id').valid();
		$('#customer').valid();
	}

	// Initialize the typeahead plugin
	typeAheadModule.initializeApiTypeAheadWithQuery('customer', '/api/customers?query=%QUERY&includeOutOfStock=false', '%QUERY', 'customers', 'name', 3, 'customer', customerSelectFunction);
});