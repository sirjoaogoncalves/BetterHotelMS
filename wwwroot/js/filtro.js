
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
