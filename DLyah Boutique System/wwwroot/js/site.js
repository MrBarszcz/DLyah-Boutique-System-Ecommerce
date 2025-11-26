// Bloco jQuery para interações gerais do site (modais, dropdowns, menu)
// Este arquivo deve conter apenas scripts que afetam o layout global ou componentes compartilhados.

$(document).ready(function () {

    // --- LÓGICA DE EXCLUSÃO DE CATEGORIA (Modal) ---
    // Usado na Index de Categorias
    $('.btn-delete-category').click(function () {
        var categoryId = $(this).data('category-id');
        // Apenas abre o modal, a lógica de delete geralmente é um form dentro do modal ou outro script
        // Se você tiver lógica AJAX específica para categoria, ela ficaria aqui ou num arquivo category-index.js
        $('#modalDeleteCategory').modal('show');
    });

    // --- LÓGICA DE EXCLUSÃO DE BANNER (Modal + AJAX) ---
    // Usado na Index de Banners
    var bannerIdToDelete;

    // Quando o modal de confirmação abre
    $('#confirmDeleteModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        bannerIdToDelete = button.data('banner-id');
    });

    // Quando confirma a exclusão
    $('#confirmDeleteBtn').on('click', function () {
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: '/Banner/Delete/' + bannerIdToDelete,
            type: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            success: function (result) {
                console.log(result.message);
                // Remove a linha da tabela visualmente
                $('button[data-banner-id="' + bannerIdToDelete + '"]').closest('tr').fadeOut(500, function() {
                    $(this).remove();
                });
                $('#confirmDeleteModal').modal('hide');
            },
            error: function (xhr, status, error) {
                console.error("Erro ao excluir o banner:", error);
                alert("Ocorreu um erro ao tentar excluir o banner.");
                $('#confirmDeleteModal').modal('hide');
            }
        });
    });

    // --- COMPORTAMENTO DE UI GLOBAL ---

    // Impede que o menu dropdown feche ao clicar dentro dele
    // Útil para filtros ou formulários dentro de dropdowns
    $('.dropdown-menu').on('click', function (e) {
        e.stopPropagation();
    });
});