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
        event.target.value = ''; // Reset input to allow re-selection
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
