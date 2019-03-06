/// <reference path="../../../Libraries/jquery/js/jquery.js" />

$.validator.setDefaults
({
	ignore: []
});

$.validator.addMethod("validCustomer", function ()
{
	return $('#customer-id').val() !== "";
});

$.validator.addMethod("validMovie", function ()
{
	return $('#movie-id').val() !== "";
});

$(document).ready(function ()
{
	const movieSelectFunction = function (event, movie)
	{
		$('#movie-id').val(movie.id);
		$('#movie-id').valid();
		$('#movie').valid();
	}

	// Initialize the typeahead plugin
	typeAheadModule.initializeApiTypeAheadWithQuery('movie', '/api/movies?query=%QUERY', '%QUERY', 'movies', 'name', 3, 'movie', movieSelectFunction);

	const customerSelectFunction = function (event, customer)
	{
		$('#customer-id').val(customer.id);
		$('#customer-id').valid();
		$('#customer').valid();
	}

	// Initialize the typeahead plugin
	typeAheadModule.initializeApiTypeAheadWithQuery('customer', '/api/customers?query=%QUERY', '%QUERY', 'customers', 'name', 3, 'customer', customerSelectFunction);
});