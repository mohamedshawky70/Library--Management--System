$(document).ready(function () {
    var book = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Search/Find?value=%QUERY', // اللي هتختاره من الانبوت بتاع السيرش هيتخزن في الكويري
            wildcard: '%QUERY'
        }
    });
    $('#Search').typeahead({
        minLength: 0,//بعد كام حرف ابدأ اظهر السيرش
        highlight: true
    },
    {
        name: 'books',
        limit:50,
        display: 'title', //اللي هيعرضه بعد ميعدي علي الاكشن
        source: book,
        templates: {
            empty: [
                '<div class="ms-3 fw-bold">',
                'No Book Founded!',
                '</div>'
            ].join('\n'),
        }
    });
}).on('typeahead:select', function (e,book) {
    window.location.replace(`/Search/Details?key=${book.key}`)
})