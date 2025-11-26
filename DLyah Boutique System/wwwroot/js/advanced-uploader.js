document.addEventListener("DOMContentLoaded", () => {

    // --- 1. Seletores e "Memória" ---
    const dropZone = document.getElementById("image-drop-zone");
    const fileInput = document.getElementById("file-input");
    const previewList = document.getElementById("image-preview-list");
    const previewTemplate = document.getElementById("image-preview-template");

    // Esta é a nossa "memória" (staging area). É aqui que guardamos os
    // arquivos na ordem correta antes de enviar o formulário.
    let fileBuffer = [];

    // --- 2. Iniciar o Drag-and-Drop (SortableJS) ---
    const sortable = new Sortable(previewList, {
        animation: 150,
        handle: ".drag-handle", // Só podemos arrastar pelo "handle"
        onEnd: (evt) => {
            // Quando terminamos de arrastar, precisamos reordenar nossa "memória" (fileBuffer)

            // Pega o arquivo que foi movido
            const movedFile = fileBuffer.splice(evt.oldDraggableIndex, 1)[0];
            // Insere ele na nova posição
            fileBuffer.splice(evt.newDraggableIndex, 0, movedFile);

            // Atualiza os IDs no HTML para manter a sincronia
            updatePreviewIds();
        }
    });

    // --- 3. Lógica de Upload (Clique e Arrastar) ---

    // Ativa o input de arquivo quando clicamos na "drop zone"
    dropZone.addEventListener("click", () => fileInput.click());

    // Eventos de "arrastar" para efeito visual
    dropZone.addEventListener("dragover", (e) => {
        e.preventDefault();
        dropZone.classList.add("drag-over");
    });
    dropZone.addEventListener("dragleave", () => dropZone.classList.remove("drag-over"));

    dropZone.addEventListener("drop", (e) => {
        e.preventDefault();
        dropZone.classList.remove("drag-over");
        // Pega os arquivos do "drop" e os envia para o manipulador
        handleFiles(e.dataTransfer.files);
    });

    // Pega os arquivos do "clique"
    fileInput.addEventListener("change", () => handleFiles(fileInput.files));

    // --- 4. Manipulador Principal de Arquivos ---
    function handleFiles(files) {
        for (const file of files) {
            if (!file.type.startsWith("image/")) continue; // Ignora se não for imagem

            const fileIndex = fileBuffer.push(file) - 1; // Adiciona na "memória" e pega o índice

            // Usa o FileReader para ler o arquivo e gerar uma pré-visualização
            const reader = new FileReader();
            reader.onload = (e) => {
                const previewClone = previewTemplate.content.cloneNode(true).firstElementChild;

                previewClone.setAttribute("data-id", fileIndex);
                previewClone.querySelector(".preview-thumbnail").src = e.target.result;
                previewClone.querySelector(".preview-name").textContent = file.name;

                // Lógica do botão Deletar
                previewClone.querySelector(".preview-delete-btn").addEventListener("click", () => {
                    const index = parseInt(previewClone.getAttribute("data-id"));

                    // Remove da "memória" (usando null para não bagunçar os índices)
                    // Uma solução mais robusta usaria um ID único
                    fileBuffer[index] = null;

                    // Remove do HTML
                    previewClone.remove();

                    // Re-compacta o array (remove os nulos) e atualiza os IDs
                    fileBuffer = fileBuffer.filter(f => f !== null);
                    updatePreviewIds();
                });

                previewList.appendChild(previewClone);
            };

            reader.readAsDataURL(file);
        }
    }

    // Função auxiliar para re-sincronizar o HTML com a "memória"
    function updatePreviewIds() {
        let i = 0;
        previewList.querySelectorAll(".preview-item").forEach(item => {
            item.setAttribute("data-id", i++);
        });
    }

    // --- 5. Interceptar o Envio do Formulário ---
    // Este é o passo mais importante!

    // CORREÇÃO 1: O ID do seu formulário é "productForm", e não "product-form"
    const form = document.getElementById("productForm");
    form.addEventListener("submit", async (e) => {
        e.preventDefault(); // Impede o envio normal do formulário

        const formData = new FormData(form); // Pega todos os outros campos (Nome, Preço, etc)

        // CORREÇÃO 2: O nome do campo deve ser "PImages" para bater com o ViewModel
        formData.delete("PImages");

        // Adiciona nossos arquivos da "memória" (agora na ordem correta!)
        fileBuffer.forEach((file) => {
            if(file) {
                // CORREÇÃO 3: O nome "PImages" é o que o C# vai procurar
                formData.append("PImages", file, file.name);
            }
        });

        // Envia o formulário manualmente com o fetch
        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
                // Não defina 'Content-Type', o navegador faz isso
            });

            if (response.ok) {
                // Deu certo! Redireciona para onde o C# mandou
                window.location.href = response.url;
            } else {
                // Trate o erro (ex: mostrar erro de validação)
                console.error("Falha no envio do formulário.");
            }
        } catch (error) {
            console.error("Erro de rede:", error);
        }
    });
});