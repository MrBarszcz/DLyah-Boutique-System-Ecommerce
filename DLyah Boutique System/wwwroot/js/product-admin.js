// Conteúdo para o arquivo: wwwroot/js/product-admin.js

$(document).ready(function () {
    console.log("Script 'product-admin.js' carregado.");

    var productIdToDelete; // Variável para guardar o ID do produto

    // Quando o modal de exclusão de PRODUTO estiver abrindo...
    $('#confirmDeleteProductModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        productIdToDelete = button.data('product-id');
    });

    // Quando o botão de confirmação do modal de PRODUTO for clicado...
    $('#confirmDeleteProductBtn').on('click', function () {
        // Pega o token de segurança da página
        var token = $('input[name="__RequestVerificationToken"]').val();

        if (productIdToDelete && token) {
            $.ajax({
                url: '/Product/Delete/' + productIdToDelete,
                type: 'POST',
                headers: {
                    'RequestVerificationToken': token
                },
                success: function (result) {
                    if (result.success) {
                        // Remove a linha da tabela da tela, com um efeito suave
                        $('button[data-product-id="' + productIdToDelete + '"]').closest('tr').fadeOut(500, function() {
                            $(this).remove();
                        });
                        // Fecha o modal
                        $('#confirmDeleteProductModal').modal('hide');
                    } else {
                        alert(result.message || "Não foi possível excluir o produto.");
                        $('#confirmDeleteProductModal').modal('hide');
                    }
                },
                error: function () {
                    alert("Ocorreu um erro de comunicação ao tentar excluir o produto.");
                    $('#confirmDeleteProductModal').modal('hide');
                }
            });
        }
    });
});