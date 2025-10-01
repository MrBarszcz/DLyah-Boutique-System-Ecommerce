function initializeBannerPlacementForm(pageConfig, currentPlacementsCount) {
    // Mapeamento dos elementos
    const bannerSelect = document.getElementById('banner-select');
    const positionSelect = document.getElementById('position-select');
    const displayTypeContainer = document.getElementById('display-type-container');
    const radioSingle = document.getElementById('display-type-single');
    const radioCarousel = document.getElementById('display-type-carousel');
    const radioMosaic = document.getElementById('display-type-mosaic');

    // Adiciona um listener para a seleção de banners
    bannerSelect.addEventListener('change', updateFormState);
    // Também chama a função quando a posição muda
    positionSelect.addEventListener('change', updateFormState);

    function updateFormState() {
        // Pega a quantidade de banners selecionados no formulário
        const selectedBannersCount = Array.from(bannerSelect.selectedOptions).length;
        const selectedPosition = positionSelect.value;

        // Esconde as opções de layout por padrão
        displayTypeContainer.style.display = 'none';

        // Só continua se uma posição for selecionada
        if (!selectedPosition) return;

        // Mostra as opções de layout
        displayTypeContainer.style.display = 'block';

        // Lógica para habilitar/desabilitar os radio buttons
        if (selectedBannersCount === 1 && currentPlacementsCount === 0) {
            // Caso 1: Nenhum banner na página E só 1 banner selecionado
            radioSingle.disabled = false;
            radioSingle.checked = true;
            radioCarousel.disabled = true;
            radioMosaic.disabled = true;
        } else if (selectedBannersCount > 1) {
            // Caso 2: Mais de 1 banner selecionado
            radioSingle.disabled = true;
            radioCarousel.disabled = false;
            radioMosaic.disabled = false;
            // Sugere Mosaico como padrão se não estiver checado
            if (!radioCarousel.checked && !radioMosaic.checked) {
                radioMosaic.checked = true;
            }
        } else if (selectedBannersCount === 0) {
            // Caso 3: Nenhum banner selecionado ainda
            radioSingle.disabled = true;
            radioCarousel.disabled = true;
            radioMosaic.disabled = true;
        } else {
            // Outros casos (ex: 1 banner selecionado mas já existem outros na página)
            radioSingle.disabled = false;
            radioCarousel.disabled = false;
            radioMosaic.disabled = false;
        }

        // Lógica do mosaico cheio (ex: 4 banners) - adicione a sua regra aqui
        const MOSAIC_LIMIT = 4;
        if(currentPlacementsCount >= MOSAIC_LIMIT) {
            // radioMosaic.disabled = true; // Descomente se quiser desabilitar o mosaico
        }
    }

    // Roda a função uma vez no carregamento da página para definir o estado inicial
    updateFormState();
}