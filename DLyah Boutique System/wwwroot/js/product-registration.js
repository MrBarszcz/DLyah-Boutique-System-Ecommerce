/**
 * product-registration.js
 * Gerencia o refresh dinâmico de dropdowns (Categorias, Cores, Tamanhos)
 * e a geração automática da grade de estoque baseada nas seleções.
 */

document.addEventListener("DOMContentLoaded", () => {
    // Inicializa os ouvintes de evento para o estoque na carga da página
    attachStockListeners();

    // Se já houver itens marcados (ex: validação falhou e a página recarregou),
    // regenera os inputs de estoque imediatamente.
    regenerateStockIfNeeded();
});

function regenerateStockIfNeeded() {
    if(document.querySelectorAll('input[name="SelectedColors"]:checked').length > 0 ||
        document.querySelectorAll('input[name="SelectedSizes"]:checked').length > 0) {
        generateStockInputs();
    }
}

// --- 1. LÓGICA DE REFRESH DINÂMICO (AJAX) ---

/**
 * Atualiza as opções de um dropdown específico via AJAX sem recarregar a página.
 * @param {string} type - O tipo de dado a atualizar: 'Categories', 'Colors' ou 'Sizes'.
 */
async function refreshData(type) {
    let url = '';
    let listId = '';
    let inputName = '';
    let cssClass = ''; // Classe usada para identificar inputs que afetam o estoque

    // Configuração baseada no tipo solicitado
    if (type === 'Categories') {
        url = '/Product/GetCategoriesJson';
        listId = 'listCategories';
        inputName = 'SelectedCategories';
    } else if (type === 'Colors') {
        url = '/Product/GetColorsJson';
        listId = 'listColors';
        inputName = 'SelectedColors';
        cssClass = 'stock-trigger-color';
    } else if (type === 'Sizes') {
        url = '/Product/GetSizesJson';
        listId = 'listSizes';
        inputName = 'SelectedSizes';
        cssClass = 'stock-trigger-size';
    }

    const listElement = document.getElementById(listId);
    if (!listElement) {
        console.error(`Elemento com ID '${listId}' não encontrado.`);
        return;
    }

    // Adiciona um indicador visual de carregamento
    const originalOpacity = listElement.style.opacity;
    listElement.style.opacity = '0.5';
    listElement.style.pointerEvents = 'none';

    try {
        // 1. Salvar o que já estava marcado (para não perder seleção ao atualizar)
        const checkedValues = Array.from(listElement.querySelectorAll('input:checked')).map(cb => cb.value);

        // 2. Buscar dados novos no servidor
        const response = await fetch(url);
        if (!response.ok) throw new Error(`Erro na requisição: ${response.statusText}`);

        const data = await response.json();

        // 3. Reconstruir o HTML da lista
        let itemsHtml = '';

        data.forEach(item => {
            // Verifica se o item estava marcado antes do refresh
            const isChecked = checkedValues.includes(item.id.toString()) ? 'checked' : '';

            let labelContent = item.name;

            // Se for cor, adiciona o quadradinho colorido (swatch)
            if (type === 'Colors' && item.hex) {
                labelContent = `
                    <div class="color-box d-inline-block me-2 border rounded-circle align-middle" 
                          style="width:16px; height:16px; background-color:${item.hex};">
                    </div>${item.name}`;
            }

            // Monta o HTML do item da lista
            // Note o uso de 'data-name': crucial para o gerador de estoque saber o nome da cor/tamanho
            itemsHtml += `
                <li>
                    <div class="form-check ms-3">
                        <input type="checkbox" 
                               class="form-check-input ${cssClass}" 
                               name="${inputName}" 
                               value="${item.id}" 
                               id="${type.toLowerCase()}_${item.id}" 
                               data-name="${item.name}" 
                               ${isChecked} />
                        <label class="form-check-label" for="${type.toLowerCase()}_${item.id}">
                            ${labelContent}
                        </label>
                    </div>
                </li>
            `;
        });

        // Determina a URL para o botão "Cadastrar Novo"
        let registerController = type === 'Categories' ? 'Category' : (type === 'Colors' ? 'Color' : 'Size');

        // Adiciona o divisor e o link de cadastro no final da lista
        let registerLink = `
            <li><hr class="dropdown-divider"></li>
            <li>
                <a class="dropdown-item text-primary text-center" href="/${registerController}/Register" target="_blank">
                    + Nova Categoria
                </a>
            </li>
        `;
        // Ajuste fino para texto
        if(type !== 'Categories') registerLink = registerLink.replace("Nova Categoria", "Novo Item");

        listElement.innerHTML = itemsHtml + registerLink;

        // IMPORTANTE: Como destruímos e recriamos os checkboxes, perdemos os eventos "onchange".
        // Precisamos re-anexar os ouvintes para que a lógica de estoque continue funcionando.
        if (type === 'Colors' || type === 'Sizes') {
            attachStockListeners();

            // Se novos itens foram carregados e marcados (preservação de estado), regenera o estoque
            regenerateStockIfNeeded();
        }

    } catch (error) {
        console.error("Erro ao atualizar lista via AJAX:", error);
        alert("Não foi possível atualizar a lista. Verifique sua conexão ou tente novamente.");
    } finally {
        // Restaura a aparência da lista
        listElement.style.opacity = originalOpacity || '1';
        listElement.style.pointerEvents = 'auto';
    }
}


// --- 2. LÓGICA DE ESTOQUE DINÂMICO (USANDO TEMPLATE) ---

/**
 * Anexa o evento 'change' a todos os checkboxes de Cor e Tamanho.
 * Deve ser chamado no início e sempre que a lista for reconstruída (após refresh).
 */
function attachStockListeners() {
    // Seleciona todos os inputs que afetam o estoque
    const triggers = document.querySelectorAll('.stock-trigger-color, .stock-trigger-size');

    triggers.forEach(input => {
        // Remove listener anterior para evitar duplicidade (boa prática)
        input.removeEventListener('change', generateStockInputs);
        // Adiciona o listener
        input.addEventListener('change', generateStockInputs);
    });
}

/**
 * Gera a grade de inputs de estoque (combinação de Cores x Tamanhos selecionados).
 * Usa o <template> definido na View para criar o HTML.
 */
function generateStockInputs() {
    const container = document.getElementById('stockInputsContainer');
    const template = document.getElementById('stockInputTemplate');

    if (!container || !template) return;

    // Pega apenas os itens MARCADOS
    const selectedColors = Array.from(document.querySelectorAll('input[name="SelectedColors"]:checked'));
    const selectedSizes = Array.from(document.querySelectorAll('input[name="SelectedSizes"]:checked'));

    // 1. Salvar valores atuais para não perder o que o usuário já digitou ao regenerar
    const currentValues = {};
    container.querySelectorAll('input[type="number"]').forEach(input => {
        const key = input.dataset.key;
        if (key) currentValues[key] = input.value;
    });

    // Limpa o container visualmente
    container.innerHTML = '';

    // Se não houver seleção completa, mostra mensagem ou esconde
    if (selectedColors.length === 0 || selectedSizes.length === 0) {
        container.innerHTML = `
            <div class="col-12">
                <div class="alert alert-light text-center border w-100 mb-0 text-muted small" role="alert">
                    Selecione pelo menos uma <strong>Cor</strong> e um <strong>Tamanho</strong> para definir o estoque.
                </div>
            </div>`;
        return;
    }

    let index = 0; // Índice para o Model Binding do ASP.NET (Stock[0], Stock[1]...)

    // Cria o HTML para cada combinação
    selectedColors.forEach(color => {
        selectedSizes.forEach(size => {
            const colorName = color.getAttribute('data-name') || "Cor";
            const sizeName = size.getAttribute('data-name') || "Tam";
            const colorId = color.value;
            const sizeId = size.value;

            // Tenta recuperar o Hex da cor visualmente (do elemento irmão label > .color-box)
            let colorHex = '#ddd';
            const label = color.nextElementSibling; // O <label> vem logo depois do <input>
            if (label) {
                const colorBox = label.querySelector('.color-box');
                if (colorBox) {
                    colorHex = colorBox.style.backgroundColor;
                }
            }

            // Chave única para recuperar valor digitado anteriormente
            const uniqueKey = `c${colorId}_s${sizeId}`;
            const savedValue = currentValues[uniqueKey] || 0;

            // --- CLONAGEM E PREENCHIMENTO DO TEMPLATE ---
            const clone = template.content.cloneNode(true);

            // 1. Atualiza a cor visual (bolinha)
            const colorPreview = clone.querySelector('.stock-color-preview');
            if(colorPreview) colorPreview.style.backgroundColor = colorHex;

            // 2. Atualiza os textos
            clone.querySelector('.stock-label-color').textContent = colorName;
            clone.querySelector('.stock-label-size').textContent = `Tamanho: ${sizeName}`;

            // 3. Configura os Inputs Ocultos (IDs)
            const inputColorId = clone.querySelector('.stock-input-color-id');
            inputColorId.name = `Stock[${index}].ColorId`;
            inputColorId.value = colorId;

            const inputSizeId = clone.querySelector('.stock-input-size-id');
            inputSizeId.name = `Stock[${index}].SizeId`;
            inputSizeId.value = sizeId;

            // 4. Configura o Input de Quantidade
            const inputQty = clone.querySelector('.stock-input-qty');
            inputQty.name = `Stock[${index}].StockQuantity`;
            inputQty.value = savedValue;
            inputQty.dataset.key = uniqueKey; // Para persistência

            // Adiciona ao container
            container.appendChild(clone);
            index++;
        });
    });
}