document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener('hide.bs.modal', function (event) {
        if (document.activeElement) {
            document.activeElement.blur();
        }
    });
});

// Class definition
var KTDatatablesExample = function () {
    // Shared variables
    var table;
    var datatable;

    // Private functions
    var initDatatable = function () {
        
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'order': [],
            'pageLength': 10,
        });
    }
    //Start iterate to enclude all columns whithout action
    var header = $('th');  // اللي هلف عليه
    var includeCols = [];  //array
    $.each(header,function (i) {
        var cols = $(this);
        if (!cols.hasClass('Action')) {
            includeCols.push(i);
        }
    })
    //End iterate to enclude all columns whithout action
    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = '';
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns:includeCols
                    }
                    
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: includeCols
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: includeCols
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: includeCols
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

/*     Public methods*/
    return {
        init: function () {
            table = document.querySelector('#kt_datatable_example');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();



////////////////////////////////////////////////////////////////////////////
$(document).ready(function () {
    /*    $('table').DataTable();*/
    $('.Js-StatusToggle').on('click', function () {  //select btn toggle
        var btn = $(this);    //btn
        
        bootbox.confirm({
            message: "Are you sure that you need to toggle this item status?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {
                   // console.log(data);
                    ShowSuccessMessage();         //SweetAlert
                    $.post({                      //or get
                        url: btn.data('url'),
                        success: function (UpdateDate) {
                            var status = btn.parents('tr').find('.ColorStatus');   // from parent get ColorStatus
                            var newStatus = status.text().trim() === 'Available' ? 'Deleted' : 'Available';
                            status.text(newStatus).toggleClass('badge text-bg-success badge text-bg-danger'); //add new text and change color

                            var update = btn.parents('tr').find('.LastUpdate');   // from parent get LastUpdate
                            var NewUpdate = update.text(UpdateDate);             //change dateTime
                        }

                    })
                }
            }
        });





    });
    // On document ready in the last

    KTUtil.onDOMContentLoaded(function () {
        KTDatatablesExample.init();

    });
});
