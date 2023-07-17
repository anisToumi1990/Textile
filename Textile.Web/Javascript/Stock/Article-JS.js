function DeleteArticle(id) {
    
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Article");
    $("#SelectedEntity").val("Article");
    $("#DeleteMessage").text("Voulez-vous supprimer cet article ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}
function OpenArticleEditor(id) {
    
    var reference = $("#Code").val();
    $.ajax({
        type: "GET",
        url: "/Stock/OpenArticleEditor",
        data: { ID: id, reference: reference },
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });

        },
        success: function (data) {
            document.getElementById("modalDialog").classList.add("modal-sm");
            $('#GenericModel').modal(
                {
                    backdrop: 'static',
                    keyboard: false
                }
            );
            $("#ModalTitle").text("Article");
            $("#ModalBody").html(data);
        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function AddOrUpdateArticle() {
    var code = $('#Code').val();
    var formData = $("#ParamForm").serializeArray();
    formData["code"] = code;
    var CheckValidityForm = $("#ParamForm")[0].checkValidity();
    if (CheckValidityForm == true) {
        event.preventDefault();
        $.ajax({
            type: 'POST',
            url: "/Stock/AddOrUpdateArticle",
            data: formData,
            encode: true,
            success: function (response) {
                
                //var res = JSON.parse(response);

                $('#GenericModel').modal('hide');
                $('#ArticleGrid').DataTable().ajax.reload(null, false);
                toastr.success('opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            },
        });
    }
}
function LoadArticleData(code) {
    var table = $('#ArticleGrid').DataTable({
        "order": [[1, "asc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Stock/GetAllArticle',
            "data": {
                "Code": code
            },
            dataSrc: ''
        },
        columns: [
            { "data": "id" },
            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var id = row.id;
                    var article = row.CodeArticle;
                    return "<div style='display:inline-flex'>" +
                        "<a data-toggle='tooltip' title='Modifier l&#96;article: " +
                        article +
                        "' onclick='OpenArticleEditor(" +
                        "\"" +
                        id +
                        "\"" +
                        ")' href='#' class='btn btn-warning btn-circle btn-sm'>" +
                        "<i class='fas fa-edit'></i> </a>" +
                        "<a data-toggle='tooltip' title='Afficher les détails de l&#96;article: " +
                        article +
                        "' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=Btn_Detail_click('" +
                        id +
                        "') href='#' class='btn btn-success btn-circle btn-sm'>" +
                        "<i class='fas fa-list'></i> </a>" +
                        "<a data-toggle='tooltip' title='Supprimer l&#96;article: " +
                        article +
                        "' onclick=DeleteArticle('" +
                        id +
                        "')  href='#' class='btn btn-danger btn-circle btn-sm'>" +
                        "<i class='fas fa-trash'></i> </a>" +
                        "</div>";
                }
            },
            { "data": "CodeArticle" },
            {
                "data": "Quantite",
                render: function (data, type, row) {
                    if (data) {
                       
                        return "<span class='badge badge-success' >" + data + " m </span>"

                    }
                    else {
                        return "<span class='badge badge-danger' >"+ 0 + " m </span>"
                      
                    }
                    
                }
            },
            {
                "data": "Couleur",
                render: function (data, type, row) {
                    if (data != null) {
                        return "<div style='background:" + data + "' class='cercle'></div>"
                    }
                    else {
                        return "";
                    }
                }

            },
            {
                "data": "DateCreation",
                render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            {
                "data": "DateModification",
                render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY");
                }
            }
        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }],
        "drawCallback": function (settings) {
            $('[data-toggle="tooltip"]').tooltip();
        }
    });

}
function Btn_Detail_click(myValue) {
    if (myValue != null) {
        window.location.href = "/Stock/ArticleDetails?Code=" + myValue;
    }
}