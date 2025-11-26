// --- Lógica da Página de Detalhes do Produto (Frontend do Cliente) ---
document.addEventListener('DOMContentLoaded', () => {
    const mainProductImage = document.getElementById('mainProductImage');

    // Se este elemento não existe, não estamos na página de detalhes, então paramos.
    if (!mainProductImage) return;

    const thumbnailList = document.getElementById('thumbnailList');
    const stockInfoDiv = document.getElementById('stockInfo');
    const addToCartBtn = document.getElementById('addToCartBtn');
    const buyNowBtn = document.getElementById('buyNowBtn');

    let selectedColorId = null;
    let selectedSizeId = null;

    // Função para trocar a imagem principal ao clicar na miniatura
    window.changeMainImage = function(thumbnailElement, imagePath) {
        mainProductImage.src = imagePath;
        // Remove a classe 'active' de todas as miniaturas e adiciona na clicada
        if (thumbnailList) {
            thumbnailList.querySelectorAll('.thumbnail-item').forEach(item => item.classList.remove('active'));
        }
        thumbnailElement.classList.add('active');
    }

    // Função para registrar a seleção de uma variante (cor ou tamanho)
    window.selectVariant = function(element, type) {
        const selectorId = `#${type}Selector`;
        const optionClass = `.${type}-option`;

        // Remove a seleção anterior visualmente
        document.querySelectorAll(`${selectorId} ${optionClass}`).forEach(el => el.classList.remove('selected', 'active'));
        element.classList.add('selected', 'active');

        // Atualiza os IDs selecionados e os textos na tela
        if (type === 'color') {
            selectedColorId = element.dataset.colorId;
            const colorNameEl = document.getElementById('selectedColorName');
            if(colorNameEl) colorNameEl.textContent = `(${element.dataset.colorName})`;
        } else if (type === 'size') {
            selectedSizeId = element.dataset.sizeId;
            const sizeNameEl = document.getElementById('selectedSizeName');
            if(sizeNameEl) sizeNameEl.textContent = `(${element.dataset.sizeName})`;
        }

        // Verifica o estoque com a nova combinação
        checkStock();
    }

    // Função para verificar e exibir o estoque
    function checkStock() {
        // A variável global 'stockData' deve ser definida na View (Details.cshtml) antes deste script
        if (selectedColorId && selectedSizeId && typeof stockData !== 'undefined') {
            const stockKey = `${selectedColorId}-${selectedSizeId}`;
            const quantity = stockData[stockKey];

            if (quantity !== undefined && quantity > 0) {
                stockInfoDiv.innerHTML = `<span class="text-success fw-bold">Em estoque (${quantity} disponíveis)</span>`;
                if(addToCartBtn) addToCartBtn.disabled = false;
                if(buyNowBtn) buyNowBtn.disabled = false;
            } else {
                stockInfoDiv.innerHTML = `<span class="text-danger">Indisponível</span>`;
                if(addToCartBtn) addToCartBtn.disabled = true;
                if(buyNowBtn) buyNowBtn.disabled = true;
            }
        } else {
            stockInfoDiv.innerHTML = 'Selecione cor e tamanho';
            if(addToCartBtn) addToCartBtn.disabled = true;
            if(buyNowBtn) buyNowBtn.disabled = true;
        }
    }

    // Executa uma vez ao carregar para validar estado inicial
    checkStock();
});