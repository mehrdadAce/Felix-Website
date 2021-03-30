var bedragSlider = document.getElementById('bedrag-slider');
noUiSlider.create(bedragSlider, {
    start: [250, 750],
    connect: true,
    range: {
        'min': 0,
        'max': 1000
    }
});
var kmSlider = document.getElementById('km-slider');
noUiSlider.create(kmSlider, {
    start: [250, 750],
    connect: true,
    range: {
        'min': 0,
        'max': 1000
    }
});