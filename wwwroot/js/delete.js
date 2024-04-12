function confirmDelete() {
    $('#confirmModal').modal('show');
    event.preventDefault();

    // Capture the form element
    var form = document.querySelector('.action-buttons');

    $('#confirmButton').click(function() {
        form.submit(); // Submit the form
    });

    $('#cancelButton').click(function() {
        $('#confirmModal').modal('hide');
    });
}
