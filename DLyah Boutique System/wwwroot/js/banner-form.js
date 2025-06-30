// Este script lida com a pré-visualização de UMA ÚNICA imagem

document.addEventListener('DOMContentLoaded', function () {
    // Procura pelo input de arquivo e pelo container da imagem
    const imageUploadInput = document.getElementById('ImageUpload');
    const imagePreview = document.getElementById('imagePreview');

    // Se ambos os elementos existirem na página, ativa a lógica
    if (imageUploadInput && imagePreview) {

        imageUploadInput.addEventListener('change', function (event) {
            // Pega o primeiro (e único) arquivo que foi selecionado
            const file = event.target.files[0];

            if (file) {
                // Se um arquivo foi selecionado, usa o FileReader para lê-lo
                const reader = new FileReader();

                reader.onload = function(e) {
                    // Quando o arquivo for lido, define o 'src' da tag <img> com o resultado
                    imagePreview.src = e.target.result;
                    // Garante que a imagem de pré-visualização esteja visível
                    imagePreview.style.display = 'block';
                };

                // Lê o arquivo como uma Data URL (uma representação da imagem em texto)
                reader.readAsDataURL(file);
            }
        });
    }
});