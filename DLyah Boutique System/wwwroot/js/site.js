$(document).ready(function () {
    $('.btn-total-products').click(function () {
        $('#modalProductCategory').modal('show');
    });
    
    $('.btn-delete-category').click(function () {
        var categoryId = $(this).attr('category-id'); // Get the category ID when clicking the button

        // $.ajax({ // AJAX request that returns a PartialView with the information to delete the category
        //     type: "GET",
        //     url: "/ControlPanel/KillCategory/" + categoryId, 
        //     success: function(result){
        //         $("#KillConfirmationCategory").html(result);
        //         $('#modalDeleteCategory').modal('show');
        //     }
        // });

        $('#modalDeleteCategory').modal('show');
        
    })
    
    $('.dropdown-menu').on('click', function (e) {
        e.stopPropagation();
    })
})

document.addEventListener('DOMContentLoaded', () => {
    const productForm = document.getElementById('productForm');
    const imageInput = document.getElementById('productImagesInput');
    const previewContainer = document.getElementById('imagePreviewContainer');
    
    let accumulatedFiles = [];
    
    if (imageInput) {
        imageInput.addEventListener('change', (event) => {
            const newFiles = Array.from(event.target.files);
            
            newFiles.forEach(file => {
                if (file.type.startsWith('image/')) {
                    if (!accumulatedFiles.some(f => f.name === file.name && f.size === file.size)) {
                        accumulatedFiles.push(file);
                    }
                }
            });
            
            renderImagePreviews();
            
            event.target.value = '';
            
        });
    }
    
    // --- anterior --- //
    /*let imagePreviews = [];

    imageInput.addEventListener('change', (event) => {
        const files = event.target.files;
        for (const file of files) {
            if (file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    imagePreviews.push({ name: file.name, dataUrl: e.target.result });
                    renderImagePreviews();
                };
                reader.readAsDataURL(file);
            }
        }
    });*/

    function renderImagePreviews() {
        if (accumulatedFiles.length > 0) {
            // Se houver arquivos, torna o container visível usando o display 'flex' que usamos para o layout.
            previewContainer.style.display = 'flex';
        } else {
            // Se não houver arquivos (lista vazia), oculta o container.
            previewContainer.style.display = 'none';
        }
        
        previewContainer.innerHTML = ''; // Limpa as prévias existentes

        accumulatedFiles.forEach((file, index) => {
            const reader = new FileReader();

            reader.onload = (e_reader) => {
                // Cria o container principal do item (será o elemento arrastável)
                const itemWrapper = document.createElement('div');
                itemWrapper.classList.add('image-preview-item');
                itemWrapper.setAttribute('draggable', 'true');
                itemWrapper.dataset.index = index; // Guarda o índice para o drag-and-drop

                // Cria o container para a imagem e a sobreposição
                const imageContainer = document.createElement('div');
                imageContainer.classList.add('image-container');

                // Cria a imagem
                const img = document.createElement('img');
                img.classList.add('image-preview');
                img.src = e_reader.target.result;
                img.alt = file.name;

                // Cria a sobreposição de remoção
                const removeOverlay = document.createElement('div');
                removeOverlay.classList.add('remove-overlay');
                removeOverlay.onclick = () => {
                    accumulatedFiles.splice(index, 1); // Remove o arquivo do array
                    renderImagePreviews(); // Re-renderiza tudo
                };

                // Cria o ícone 'X'
                const removeIcon = document.createElement('span');
                removeIcon.classList.add('remove-icon');
                removeIcon.innerHTML = '&times;'; // '&times;' é a entidade HTML para o 'X'

                // Cria o elemento para o nome do arquivo
                const fileName = document.createElement('span');
                fileName.classList.add('image-name');
                const maxLength = 15; // Máximo de caracteres para o nome do arquivo
                fileName.textContent = file.name.length > maxLength ? file.name.substring(0, maxLength - 3) + '...' : file.name;

                // Monta a estrutura
                removeOverlay.appendChild(removeIcon);
                imageContainer.appendChild(img);
                imageContainer.appendChild(removeOverlay);
                itemWrapper.appendChild(imageContainer);
                itemWrapper.appendChild(fileName);

                // Adiciona o item completo ao container principal
                previewContainer.appendChild(itemWrapper);
            };

            reader.readAsDataURL(file);
        });

        // Chama a função de drag and drop após renderizar
        setupDragAndDrop();
    }

    if (productForm) {
        productForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            // Opcional: Adicionar validação do lado do cliente aqui antes de prosseguir
            // if (typeof $ !== 'undefined' && $(productForm).valid && !$(productForm).valid()) {
            //     // Se estiver usando jQuery Validation e o formulário for inválido
            //     console.log("Formulário inválido de acordo com jQuery Validation.");
            //     return; // Não prossegue com o envio AJAX
            // }
            
            const formData = new FormData(productForm);
            
            formData.delete('PImages')

            if (accumulatedFiles.length === 0) {
                // Adiciona um alerta ou feedback para o usuário se nenhuma imagem for selecionada (opcional)
                // console.log("Nenhuma imagem selecionada para upload.");
                // Se as imagens não forem obrigatórias, você pode prosseguir.
                // Se forem, você pode mostrar um erro e retornar.
            }
            
            accumulatedFiles.forEach(file => {
                formData.append('PImages', file, file.name);
            });

            const submitButton = productForm.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.disabled = true;
                submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Enviando...';
            }

            try {
                const response = await fetch(productForm.action, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        // O 'Content-Type' para FormData é definido automaticamente pelo navegador
                        // 'X-Requested-With': 'XMLHttpRequest' // Útil se o controller verificar isso
                        // Adicione o token anti-falsificação se você o estiver usando
                        // Para o token [ValidateAntiForgeryToken], você precisa pegá-lo da página e enviar no header
                        // ou como um campo no FormData. Exemplo de pegar o token (se ele estiver em um input hidden):
                        // const requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');
                        // if (requestVerificationToken) {
                        //     formData.append('__RequestVerificationToken', requestVerificationToken.value);
                        // }
                        // Se você não adicionar o token e tiver [ValidateAntiForgeryToken] no controller, dará erro 400.
                        // Para simplificar este exemplo inicial, vou omitir o token, mas é crucial para produção.
                    }
                });
                
                if (submitButton) {
                    submitButton.disabled = false;
                    submitButton.innerHTML = 'Enviar';
                }
                
                if (response.ok) {
                    const result = await response.json(); // Supondo que o controller retorne JSON
                    if (result.success && result.redirectToUrl) {
                        window.location.href = result.redirectToUrl; // Redireciona em caso de sucesso
                    } else if (result.success) {
                        alert("Produto cadastrado com sucesso!"); // Ou outra mensagem
                        productForm.reset(); // Limpa o formulário
                        accumulatedFiles = []; // Limpa os arquivos acumulados
                        renderImagePreviews(); // Limpa as prévias
                    } else {
                        // Lida com erros de validação do servidor retornados no JSON
                        if (result.errors) {
                            let errorMessage = "Por favor, corrija os seguintes erros:\n";
                            for (const key in result.errors) {
                                errorMessage += `${key}: ${result.errors[key].join(', ')}\n`;
                            }
                            alert(errorMessage);
                        } else {
                            alert("Ocorreu um erro ao processar sua solicitação.");
                        }
                    }
                } else {
                    // Erro HTTP (4xx, 5xx)
                    const errorText = await response.text(); // Tenta pegar mais detalhes do erro
                    console.error('Erro no envio do formulário:', response.status, response.statusText, errorText);
                    alert(`Erro ao enviar o formulário: ${response.status} ${response.statusText}. Detalhes no console.`);
                }
            } catch (error) {
                if (submitButton) { // Reabilita o botão em caso de erro de rede, etc.
                    submitButton.disabled = false;
                    submitButton.innerHTML = 'Enviar';
                }
                console.error('Erro de rede ou JavaScript ao enviar o formulário:', error);
                alert('Erro de rede ou script ao tentar enviar o formulário.');
            }
        });
    }

    function setupDragAndDrop() {
        // Agora o elemento arrastável é '.image-preview-item'
        const draggableItems = document.querySelectorAll('.image-preview-item');
        let draggedItem = null;

        draggableItems.forEach(item => {
            item.addEventListener('dragstart', (event) => {
                draggedItem = item;
                event.dataTransfer.setData('text/plain', item.dataset.index);
                // Adiciona um timeout para que o navegador "tire um print" do elemento antes de aplicar o estilo
                setTimeout(() => {
                    item.classList.add('dragging');
                }, 0);
            });

            item.addEventListener('dragend', () => {
                if (draggedItem) {
                    draggedItem.classList.remove('dragging');
                }
            });
        });

        previewContainer.addEventListener('dragover', (event) => {
            event.preventDefault(); // Necessário para permitir o 'drop'
            const afterElement = getDragAfterElement(previewContainer, event.clientX);
            const dragging = document.querySelector('.dragging');
            if (dragging) {
                if (afterElement == null) {
                    previewContainer.appendChild(dragging);
                } else {
                    previewContainer.insertBefore(dragging, afterElement);
                }
            }
        });

        previewContainer.addEventListener('drop', (event) => {
            event.preventDefault();
            const droppedIndex = parseInt(event.dataTransfer.getData('text/plain'));
            const draggingItem = document.querySelector('.dragging');
            if (!draggingItem) return;

            // Encontra a nova posição do elemento no DOM
            const allItems = Array.from(previewContainer.querySelectorAll('.image-preview-item:not(.dragging)'));
            let newIndex = allItems.findIndex(item => item === getDragAfterElement(previewContainer, event.clientX));
            if (newIndex === -1) {
                newIndex = allItems.length;
            }

            // Atualiza a ordem no array `accumulatedFiles`
            const [removed] = accumulatedFiles.splice(droppedIndex, 1);
            accumulatedFiles.splice(newIndex, 0, removed);

            // Remove a classe de arrastar para que a re-renderização completa funcione
            draggingItem.classList.remove('dragging');

            // Re-renderiza tudo para garantir que os índices e os botões de remover estejam corretos
            renderImagePreviews();
        });
    }

    function getDragAfterElement(container, x) {
        const draggableElements = [...container.querySelectorAll('.image-preview-item:not(.dragging)')];

        return draggableElements.reduce((closest, child) => {
            const box = child.getBoundingClientRect();
            const offset = x - box.left - box.width / 2;
            if (offset < 0 && offset > closest.offset) {
                return { offset: offset, element: child };
            } else {
                return closest;
            }
        }, { offset: Number.NEGATIVE_INFINITY }).element;
    }

    // Initial render (if needed)
    if (/* Check if ViewBag.ImagePreviews has initial data */ false) {
        // Adapt based on how initial previews are passed
    }
});

document.addEventListener('DOMContentLoaded', () => {
    const colorCheckboxes = document.querySelectorAll('input[name="SelectedColors"]');
    const sizeCheckboxes = document.querySelectorAll('input[name="SelectedSizes"]');
    const stockInputsContainer = document.getElementById('stockInputsContainer');

    function generateStockInputs() {
        stockInputsContainer.innerHTML = ''; // Limpa os inputs existentes

        const selectedColors = Array.from(colorCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        const selectedSizes = Array.from(sizeCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        if (selectedColors.length > 0 && selectedSizes.length > 0) {
            let stockIndex = 0;
            selectedColors.forEach(colorId => {
                const colorName = getColorName(colorId);
                selectedSizes.forEach(sizeId => {
                    const sizeName = getSizeName(sizeId);

                    const inputGroup = document.createElement('div');
                    inputGroup.classList.add('form-group', 'row', 'mb-2');

                    const label = document.createElement('label');
                    label.classList.add('col-sm-4', 'col-form-label');
                    label.textContent = `Estoque para Cor: ${colorName}, Tamanho: ${sizeName}`;

                    const inputDiv = document.createElement('div');
                    inputDiv.classList.add('col-sm-8');

                    const input = document.createElement('input');
                    input.type = 'number';
                    input.classList.add('form-control');
                    input.name = `Stock[${stockIndex}].StockQuantity`; // Bind para Stock[i].StockQuantity
                    input.value = 0;

                    const hiddenColorId = document.createElement('input');
                    hiddenColorId.type = 'hidden';
                    hiddenColorId.name = `Stock[${stockIndex}].ColorId`;     // Bind para Stock[i].ColorId
                    hiddenColorId.value = colorId;

                    const hiddenSizeId = document.createElement('input');
                    hiddenSizeId.type = 'hidden';
                    hiddenSizeId.name = `Stock[${stockIndex}].SizeId`;      // Bind para Stock[i].SizeId
                    hiddenSizeId.value = sizeId;

                    inputDiv.appendChild(input);
                    inputGroup.appendChild(label);
                    inputGroup.appendChild(inputDiv);
                    inputGroup.appendChild(hiddenColorId);
                    inputGroup.appendChild(hiddenSizeId);
                    stockInputsContainer.appendChild(inputGroup);

                    stockIndex++;
                });
            });
        }
    }

    // Adiciona listeners de mudança aos checkboxes de cores e tamanhos
    colorCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', generateStockInputs);
    });

    sizeCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', generateStockInputs);
    });

    // Função auxiliar (você precisará implementar isso para obter os nomes)
    function getColorName(colorId) {
        // Pode buscar do ViewBag ou de um array JavaScript preenchido
        const colorElement = Array.from(document.querySelectorAll('input[name="SelectedColors"][value="' + colorId + '"]'))
            .find(checkbox => checkbox.checked);
        return colorElement ? colorElement.nextElementSibling.textContent.trim() : `Cor ID ${colorId}`;
    }

    function getSizeName(sizeId) {
        const sizeElement = Array.from(document.querySelectorAll('input[name="SelectedSizes"][value="' + sizeId + '"]'))
            .find(checkbox => checkbox.checked);
        return sizeElement ? sizeElement.nextElementSibling.textContent.trim() : `Tamanho ID ${sizeId}`;
    }
});
