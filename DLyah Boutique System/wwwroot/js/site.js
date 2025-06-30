// Bloco jQuery para interações gerais do site (modais, dropdowns, etc.)
$(document).ready(function () {
    $('.btn-delete-category').click(function () {
        var categoryId = $(this).data('category-id'); // Use .data() para ler atributos data-*
        console.log("Abrindo modal de exclusão para a categoria ID:", categoryId);
        $('#modalDeleteCategory').modal('show');
    });
    
    var bannerIdToDelete; // Variável para guardar o ID

    // Quando o modal estiver prestes a ser exibido...
    $('#confirmDeleteModal').on('show.bs.modal', function (event) {
        // Pega o botão que acionou o modal
        var button = $(event.relatedTarget);

        // Extrai o ID do banner do atributo data-banner-id
        bannerIdToDelete = button.data('banner-id');
    });

    $('#confirmDeleteBtn').on('click', function () {
        // Pega o token AntiForgery do formulário (se houver um no seu _Layout ou na página)
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: '/Banner/Delete/' + bannerIdToDelete, // Monta a URL para a action de exclusão
            type: 'POST',
            headers: {
                // Envia o token de verificação no cabeçalho da requisição
                'RequestVerificationToken': token
            },
            success: function (result) {
                // Se a exclusão no servidor for bem-sucedida
                console.log(result.message);

                // Remove a linha da tabela da tela sem precisar recarregar a página
                $('button[data-banner-id="' + bannerIdToDelete + '"]').closest('tr').fadeOut(500, function() {
                    $(this).remove();
                });

                // Fecha o modal
                $('#confirmDeleteModal').modal('hide');

                // Opcional: Exibir uma notificação de sucesso (toast)
                // Ex: showSuccessToast("Banner excluído com sucesso!");
            },
            error: function (xhr, status, error) {
                // Se ocorrer um erro no servidor
                console.error("Erro ao excluir o banner:", error);
                alert("Ocorreu um erro ao tentar excluir o banner.");
                $('#confirmDeleteModal').modal('hide');
            }
        });
    });

    // Impede que o menu dropdown feche ao clicar dentro dele
    $('.dropdown-menu').on('click', function (e) {
        e.stopPropagation();
    });
});

document.addEventListener('DOMContentLoaded', () => {

    // --- Lógica da Página de Detalhes do Produto ---

    const mainProductImage = document.getElementById('mainProductImage');

    // VERIFICAÇÃO: Se este elemento não existe, não estamos na página de detalhes.
    // O script para aqui para não dar erro em outras páginas.
    if (!mainProductImage) {
        return;
    }

    // Se estamos na página de detalhes, seleciona os outros elementos
    const thumbnailList = document.getElementById('thumbnailList');
    const colorSelector = document.getElementById('colorSelector');
    const sizeSelector = document.getElementById('sizeSelector');
    const stockInfoDiv = document.getElementById('stockInfo');
    const addToCartBtn = document.getElementById('addToCartBtn');
    const buyNowBtn = document.getElementById('buyNowBtn');

    let selectedColorId = null;
    let selectedSizeId = null;

    // Função para trocar a imagem principal ao clicar na miniatura
    window.changeMainImage = function(thumbnailElement, imagePath) {
        mainProductImage.src = imagePath;
        thumbnailList.querySelectorAll('.thumbnail-item').forEach(item => item.classList.remove('active'));
        thumbnailElement.classList.add('active');
    }

    // Função para registrar a seleção de uma variante (cor ou tamanho)
    window.selectVariant = function(element, type) {
        const selectorId = `#${type}Selector`;
        const optionClass = `.${type}-option`;

        document.querySelectorAll(`${selectorId} ${optionClass}`).forEach(el => el.classList.remove('selected', 'active'));
        element.classList.add('selected', 'active');

        if (type === 'color') {
            selectedColorId = element.dataset.colorId;
            document.getElementById('selectedColorName').textContent = `(${element.dataset.colorName})`;
        } else if (type === 'size') {
            selectedSizeId = element.dataset.sizeId;
            document.getElementById('selectedSizeName').textContent = `(${element.dataset.sizeName})`;
        }

        checkStock();
    }

    // Função para verificar e exibir o estoque
    function checkStock() {
        // Só continua se ambos, cor e tamanho, estiverem selecionados
        if (selectedColorId && selectedSizeId) {
            const stockKey = `${selectedColorId}-${selectedSizeId}`;

            // A variável 'stockData' foi definida no Details.cshtml
            const quantity = stockData[stockKey];

            if (quantity !== undefined && quantity > 0) {
                stockInfoDiv.innerHTML = `<span class="text-success fw-bold">Em estoque (${quantity} disponíveis)</span>`;
                addToCartBtn.disabled = false;
                buyNowBtn.disabled = false;
            } else {
                stockInfoDiv.innerHTML = `<span class="text-danger">Indisponível</span>`;
                addToCartBtn.disabled = true;
                buyNowBtn.disabled = true;
            }
        } else {
            stockInfoDiv.innerHTML = 'Selecione cor e tamanho';
            addToCartBtn.disabled = true;
            buyNowBtn.disabled = true;
        }
    }

    // Executa a verificação de estoque uma vez no carregamento da página, caso algum item já venha selecionado
    checkStock();
});


// Bloco de JavaScript puro (Vanilla JS) para a lógica dos formulários de Produto (Cadastro e Edição)
document.addEventListener('DOMContentLoaded', () => {

    // --- 1. SELEÇÃO DE ELEMENTOS ---
    const productForm = document.getElementById('productForm');

    // Se não estiver em uma página com o formulário, o script para aqui.
    if (!productForm) {
        return;
    }

    // Seleciona os elementos corretos, seja da página de Register ou Edit
    const imageInput = document.getElementById('productImagesInput') || document.getElementById('newImagesInput');
    const previewContainer = document.getElementById('imagePreviewContainer') || document.getElementById('newImagePreviewContainer');

    const colorCheckboxes = document.querySelectorAll('input[name="SelectedColors"]');
    const sizeCheckboxes = document.querySelectorAll('input[name="SelectedSizes"]');
    const stockInputsContainer = document.getElementById('stockInputsContainer');

    let accumulatedFiles = []; // Array para armazenar NOVAS imagens que o usuário seleciona


    // --- 2. LÓGICA DE UPLOAD DE IMAGENS (COM DRAG-AND-DROP CORRIGIDO) ---
    if (imageInput && previewContainer) {
        imageInput.addEventListener('change', (event) => {
            const newFiles = Array.from(event.target.files);

            newFiles.forEach(file => {
                if (file.type.startsWith('image/') && !accumulatedFiles.some(f => f.name === file.name && f.size === file.size)) {
                    accumulatedFiles.push(file);
                }
            });

            renderImagePreviews();
            event.target.value = '';
        });
    }

    function renderImagePreviews() {
        if (!previewContainer) return;

        previewContainer.style.display = accumulatedFiles.length > 0 ? 'flex' : 'none';
        previewContainer.innerHTML = '';

        accumulatedFiles.forEach((file, index) => {
            const reader = new FileReader();
            reader.onload = (e_reader) => {
                const itemWrapper = document.createElement('div');
                itemWrapper.className = 'image-preview-item';
                itemWrapper.setAttribute('draggable', 'true');
                itemWrapper.dataset.index = index;

                const imageContainer = document.createElement('div');
                imageContainer.className = 'image-container';

                const img = document.createElement('img');
                img.className = 'image-preview';
                img.src = e_reader.target.result;
                img.alt = file.name;

                const removeOverlay = document.createElement('div');
                removeOverlay.className = 'remove-overlay';
                removeOverlay.title = 'Remover Imagem';
                removeOverlay.onclick = () => {
                    accumulatedFiles.splice(index, 1);
                    renderImagePreviews();
                };

                const removeIcon = document.createElement('span');
                removeIcon.className = 'remove-icon';
                removeIcon.innerHTML = '&times;';

                const fileNameSpan = document.createElement('span');
                fileNameSpan.className = 'image-name';
                fileNameSpan.textContent = file.name.length > 15 ? file.name.substring(0, 12) + '...' : file.name;

                removeOverlay.appendChild(removeIcon);
                imageContainer.appendChild(img);
                imageContainer.appendChild(removeOverlay);
                itemWrapper.appendChild(imageContainer);
                itemWrapper.appendChild(fileNameSpan);
                previewContainer.appendChild(itemWrapper);
            };
            reader.readAsDataURL(file);
        });

        setupDragAndDrop();
    }

    function setupDragAndDrop() {
        const draggableItems = document.querySelectorAll('.image-preview-item');
        let draggedItem = null;

        draggableItems.forEach(item => {
            item.addEventListener('dragstart', () => {
                draggedItem = item;
                setTimeout(() => item.classList.add('dragging'), 0);
            });
            item.addEventListener('dragend', () => {
                draggedItem?.classList.remove('dragging');
            });
        });

        if(previewContainer) {
            previewContainer.addEventListener('dragover', e => {
                e.preventDefault();
                const afterElement = getDragAfterElement(previewContainer, e.clientX);
                if (draggedItem) {
                    if (afterElement == null) {
                        previewContainer.appendChild(draggedItem);
                    } else {
                        previewContainer.insertBefore(draggedItem, afterElement);
                    }
                }
            });

            previewContainer.addEventListener('drop', e => {
                e.preventDefault();
                if (!draggedItem) return;

                const originalIndex = parseInt(draggedItem.dataset.index);
                const allItems = [...previewContainer.querySelectorAll('.image-preview-item')];
                const newIndex = allItems.indexOf(draggedItem);

                draggedItem.classList.remove('dragging');

                const [removed] = accumulatedFiles.splice(originalIndex, 1);
                accumulatedFiles.splice(newIndex, 0, removed);

                renderImagePreviews();
            });
        }
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


    // --- 3. LÓGICA DE GERAÇÃO DE ESTOQUE (INTELIGENTE) ---
    function generateStockInputs() {
        if (!stockInputsContainer) return;
        stockInputsContainer.innerHTML = '';

        const selectedColors = Array.from(colorCheckboxes)
            .filter(c => c.checked)
            .map(c => ({ id: parseInt(c.value, 10), name: getColorName(c.value), hex: getColorHex(c.value) }));

        const selectedSizes = Array.from(sizeCheckboxes)
            .filter(s => s.checked)
            .map(s => ({ id: parseInt(s.value, 10), name: getSizeName(s.value) }));

        if (selectedColors.length > 0 && selectedSizes.length > 0) {
            let stockIndex = 0;
            const stockHeader = document.createElement('h6');
            stockHeader.textContent = "Defina as quantidades:";
            stockInputsContainer.appendChild(stockHeader);

            selectedColors.forEach(color => {
                selectedSizes.forEach(size => {
                    const existingStockItem = typeof initialStockData !== 'undefined'
                        ? initialStockData.find(s => s.colorId === color.id && s.sizeId === size.id)
                        : null;

                    const stockId = existingStockItem ? existingStockItem.stockId : 0;
                    const quantity = existingStockItem ? existingStockItem.stockQuantity : 0;
                    const stockItemName = `Stock[${stockIndex}]`;

                    const inputGroup = document.createElement('div');
                    inputGroup.className = 'form-group row mb-2 align-items-center';

                    const label = document.createElement('label');
                    label.className = 'col-sm-4 col-form-label';
                    label.innerHTML = `<div class="color-box d-inline-block me-2" style="background-color:${color.hex}"></div> ${color.name} / ${size.name}`;

                    const inputDiv = document.createElement('div');
                    inputDiv.className = 'col-sm-8';

                    inputDiv.innerHTML = `
                        <input type="number" min="0" class="form-control" name="${stockItemName}.StockQuantity" value="${quantity}">
                        <input type="hidden" name="${stockItemName}.StockId" value="${stockId}">
                        <input type="hidden" name="${stockItemName}.ColorId" value="${color.id}">
                        <input type="hidden" name="${stockItemName}.SizeId" value="${size.id}">
                    `;

                    inputGroup.appendChild(label);
                    inputGroup.appendChild(inputDiv);
                    stockInputsContainer.appendChild(inputGroup);
                    stockIndex++;
                });
            });
        }
    }

    function getColorName(colorId) {
        const label = document.querySelector(`label[for="color_${colorId}"]`);
        return label ? label.textContent.trim() : `Cor ID ${colorId}`;
    }
    function getColorHex(colorId) {
        const label = document.querySelector(`label[for="color_${colorId}"]`);
        const colorBox = label ? label.querySelector('.color-box') : null;
        return colorBox ? colorBox.style.backgroundColor : '#ccc';
    }
    function getSizeName(sizeId) {
        const label = document.querySelector(`label[for="size_${sizeId}"]`);
        return label ? label.textContent.trim() : `Tamanho ID ${sizeId}`;
    }

    if (colorCheckboxes.length > 0) {
        colorCheckboxes.forEach(checkbox => checkbox.addEventListener('change', generateStockInputs));
        sizeCheckboxes.forEach(checkbox => checkbox.addEventListener('change', generateStockInputs));
        generateStockInputs();
    }


    // --- 4. LÓGICA DE SUBMISSÃO AJAX ---
    productForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const formData = new FormData(productForm);
        const formAction = productForm.action.toLowerCase();

        let imageKey = formAction.includes('/edit') ? 'NewImages' : 'PImages';
        formData.delete(imageKey);
        accumulatedFiles.forEach(file => formData.append(imageKey, file, file.name));

        const submitButton = productForm.querySelector('button[type="submit"]');
        if (submitButton) {
            submitButton.disabled = true;
            submitButton.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Enviando...';
        }

        try {
            const response = await fetch(productForm.action, { method: 'POST', body: formData });

            if (response.ok) {
                const result = await response.json();
                if (result.success && result.redirectToUrl) {
                    window.location.href = result.redirectToUrl;
                } else {
                    alert("Produto salvo com sucesso, mas houve um problema no redirecionamento ou não foi especificado.");
                    productForm.reset();
                    accumulatedFiles = [];
                    renderImagePreviews();
                }
            } else {
                const errorResult = await response.json();
                let errorMessage = errorResult.message || "Por favor, corrija os erros abaixo.";
                if (errorResult.errors) {
                    for (const key in errorResult.errors) {
                        errorMessage += `\n- ${errorResult.errors[key].join(', ')}`;
                    }
                }
                alert(errorMessage);
                console.error('Erro de validação ou do servidor:', errorResult);
            }
        } catch (error) {
            alert('Erro de rede. Verifique sua conexão e tente novamente.');
            console.error('Erro de rede:', error);
        } finally {
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.innerHTML = formAction.includes('/edit') ? 'Salvar Alterações' : 'Enviar';
            }
        }
    });
});