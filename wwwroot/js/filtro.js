
document.getElementById('filterType').addEventListener('change', function() {
    var selectedOption = this.value;
    if (selectedOption === 'Funcionario') {
        document.getElementById('funcionarioFilter').style.display = 'block';
        document.getElementById('quartoFilter').style.display = 'none';
    } else if (selectedOption === 'Quarto') {
        document.getElementById('funcionarioFilter').style.display = 'none';
        document.getElementById('quartoFilter').style.display = 'block';
    }
});
$(function() {
    $("#clientName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: '@Url.Action("AutocompleteCliente", "Filtro")',
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function(data) {
                    response($.map(data, function(item) {
                        return { label: item.label, value: item.value };
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            $("#clientName").val(ui.item.value);
            return false;
        }
    });
});