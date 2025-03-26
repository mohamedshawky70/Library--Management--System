function ShowSuccessMessage(message='Success') { //Don't repeate your self (DRY)     // Success is default  
    Swal.fire({
        title: message,
        icon: "success",
      
    });
}
function ShowErrorMessage(message = 'Error') { //Don't repeate your self (DRY)     // Success is default  
    Swal.fire({
        title: message,
        icon: "error",
        
    });
}

$(document).ready(function () {
    
    var message = $("#SuccessSweetAlert2").text();
    if (message !== '') {                   // لو فيها قيمة في اللاي اوت اذن عدت علي اكشن الكريت
        ShowSuccessMessage(message);
    }
    var message = $("#ErrorSweetAlert2").text();
    if (message !== '') {                   // لو فيها قيمة في اللاي اوت اذن عدت علي اكشن الكريت
        ShowErrorMessage(message);
    }

    //handell UnLock
    $('.UnLock').on('click', function () {  //select btn toggle
        var btn = $(this);    //btn
        bootbox.confirm({
            message: "Are you sure that you need unlock this user?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    // console.log(data);
                    ShowSuccessMessage();         //SweetAlert
                    $.post({                      //or get
                        url: btn.data('url'),
                        /*success: function (UpdateDate) {
                            var status = btn.parents('tr').find('.ColorStatus');   // from parent get ColorStatus
                            var newStatus = status.text().trim() === 'Available' ? 'Deleted' : 'Available';
                            status.text(newStatus).toggleClass('badge text-bg-success badge text-bg-danger'); //add new text and change color

                            var update = btn.parents('tr').find('.LastUpdate');   // from parent get LastUpdate
                            var NewUpdate = update.text(UpdateDate);             //change dateTime
                        }*/

                    })
                }
            }
        });
    });
        //handel TinyMCE
        tinymce.init({
            selector: 'kt_docs_tinymce_basic',
            height: "445",
            plugins: [
                // Core editing features
                'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists', 'media', 'searchreplace', 'table', 'visualblocks', 'wordcount',
                // Your account includes a free trial of TinyMCE premium features
                // Try the most popular premium features until Jan 8, 2025:
                'checklist', 'mediaembed', 'casechange', 'export', 'formatpainter', 'pageembed', 'a11ychecker', 'tinymcespellchecker', 'permanentpen', 'powerpaste', 'advtable', 'advcode', 'editimage', 'advtemplate', 'ai', 'mentions', 'tinycomments', 'tableofcontents', 'footnotes', 'mergetags', 'autocorrect', 'typography', 'inlinecss', 'markdown', 'importword', 'exportword', 'exportpdf'
            ],
            toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
            tinycomments_mode: 'embedded',
            tinycomments_author: 'Author name',
            mergetags_list: [
                { value: 'First.Name', title: 'First Name' },
                { value: 'Email', title: 'Email' },
            ],
            ai_request: (request, respondWith) => respondWith.string(() => Promise.reject('See docs to implement AI Assistant')),


        });
    var options = { selector: "#kt_docs_tinymce_basic", height: "445" };

    if (KTThemeMode.getMode() === "dark") {
        options["skin"] = "oxide-dark";
        options["content_css"] = "dark";
    }

    tinymce.init(options);
        //handel select2
    $('.Js_select2').select2();

    //handel dataRangepicker .DataRange

    $('.DataRange').daterangepicker({
        autoApply: true,
        //maxDate: Date.to,
        "locale": {
            "format": "DD/MM/YYYY",
        },
        minDate: "01/01/2025",
        maxDate: new Date(),
        showDropdowns:true,
    });
    $('#SingleDataPicker').daterangepicker({
        autoApply: true,
        singleDatePicker: true,
        isInvalidDate:false,
        maxDate: new Date(),
        showDropdowns: true,
    });
});

