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
    const imageInput = document.getElementById('productImagesInput');
    const previewContainer = document.getElementById('imagePreviewContainer');
    let imagePreviews = [];

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
    });

    function renderImagePreviews() {
        previewContainer.innerHTML = '';
        imagePreviews.forEach((preview, index) => {
            const div = document.createElement('div');
            div.classList.add('image-preview-container');
            div.setAttribute('draggable', 'true');
            div.dataset.index = index;

            const img = document.createElement('img');
            img.classList.add('image-preview');
            img.src = preview.dataUrl;

            div.appendChild(img);
            previewContainer.appendChild(div);
        });
        setupDragAndDrop();
    }

    function setupDragAndDrop() {
        const draggableImages = document.querySelectorAll('.image-preview-container');
        let draggedItem = null;

        draggableImages.forEach(item => {
            item.addEventListener('dragstart', (event) => {
                draggedItem = event.target.closest('.image-preview-container');
                event.dataTransfer.setData('text/plain', draggedItem.dataset.index);
                item.classList.add('dragging');
            });

            item.addEventListener('dragend', () => {
                if (draggedItem) {
                    draggedItem.classList.remove('dragging');
                    draggedItem = null;
                }
            });
        });

        previewContainer.addEventListener('dragover', (event) => {
            event.preventDefault();
        });

        previewContainer.addEventListener('drop', (event) => {
            event.preventDefault();
            const droppedIndex = parseInt(event.dataTransfer.getData('text/plain'));
            const targetItem = event.target.closest('.image-preview-container');

            if (draggedItem && targetItem && draggedItem !== targetItem) {
                const targetIndex = parseInt(targetItem.dataset.index);
                // Atualizar o array de imagePreviews
                const [removed] = imagePreviews.splice(droppedIndex, 1);
                imagePreviews.splice(targetIndex, 0, removed);
                renderImagePreviews(); // Re-renderizar a visualização
            }
        });
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
