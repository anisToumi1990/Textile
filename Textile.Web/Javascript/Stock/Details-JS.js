function DeleteArticleDetails(id) {
    
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Article Detail");
    $("#SelectedEntity").val("Article Detail");
    $("#DeleteMessage").text("Voulez-vous supprimer ce detail ?");
    $("#confirm-delete").modal(
        {
            backdrop: "static",
            keyboard: false
        }
    );
}
function OpenArticleDetailEditor(id) {
    
    var article = $("#codeArticle").val();
    $.ajax({
        type: "GET",
        url: "/Stock/OpenArticleDetailEditor",
        data: { ID: id, article: article },
        beforeSend: function () {
            $.blockUI({ message: "Patientez un peu..." });

        },
        success: function (data) {
           
            document.getElementById("modalDialog").classList.add("modal-sm");
            $("#GenericModel").modal(
                {
                    backdrop: "static",
                    keyboard: false
                }
            );
            $("#ModalTitle").text("Article Detail");
            $("#ModalBody").html(data);

        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function AddOrUpdateArticleDetail() {
    var codeArticle = $("#codeArticle").val();
    var formData = $("#ParamForm").serializeArray();
    formData["codeArticle"] = codeArticle;
    var checkValidityForm = $("#ParamForm")[0].checkValidity();
    if (checkValidityForm === true) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Stock/AddOrUpdateArticleDetail",
            data: formData,
            encode: true,
            success: function (response) {
                
                //var res = JSON.parse(response);
                $("#GenericModel").modal("hide");
                $("#ArticleDetailsGrid").DataTable().ajax.reload(null, false);
                toastr.success("opération réussite", "Succès", { progressBar: true, showDuration: 100 });
                GetQte(codeArticle);
            },
        });
    }
}
function LoadArticleDetailsData(code) {
    
    var table = $("#ArticleDetailsGrid").DataTable({
       "order": [[9, "desc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: "/Stock/GetAllArticleDetails",
            "data": {
                "Code": code
            },
            dataSrc: ""
        },
        columns: [
            { "data": "id" },

            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var id = row.id;
                    if (row.Disponible === true) {
                        return "<div style='display:inline-flex'>" +
                            "<a data-toggle='tooltip' title='Modifier' onclick='OpenArticleDetailEditor(" +
                            "\"" +
                            id +
                            "\"" +
                            ")' href='#' class='btn btn-warning btn-circle btn-sm'>" +
                            "<i class='fas fa-edit'></i> </a>" +
                            // "<a onclick=Btn_Detail_click('" + id + "') href='#' class='btn btn-success btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                            "<a data-toggle='tooltip' title='Supprimer' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=DeleteArticleDetails('" +
                            id +
                            "')  href='#' class='btn btn-danger btn-circle btn-sm'>" +
                            "<i class='fas fa-trash'></i> </a>" +
                            "</div>";
                    }
                    else {
                        return "";
                    }
                 }
            },
          //  { "data": "Code" },
            { "data": "Designation" },
            {
                "data": "Qte",
                render: function (data, type, row) {
                    if (data) {

                        return "<span class='badge badge-success' >" + data + "m </span>"

                    }
                    else {
                        return "<span class='badge badge-danger' >" + 0 + "m </span>"

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
            },
         
            {
                "data": "",
                "render": function (data, type, row) {
                    var id = row.id;
                    var img;
                    if (row.imgURL !== "" && row.imgURL != null) {
                        img = "../Images/" + row.imgURL;
                        return "<img style='width: 50px;height:31px'  onclick='UploadImgUrl(" + "\"" + id + "\"" + ")' src='" + img + "'></img>"
                    }
                    else {
                        img = "../Images/ImageEmpty.png";
                        return "<img style='width: 50px;height:31px'  onclick='UploadImgUrl(" + "\"" + id + "\"" + ")' src='" + img + "'></img>"
                    }

                }
            },
            {
                "data": "Disponible",
                render: function (data, type, row) {
                    if (data === true) {
                        return '<span class="badge badge-success">' +
                            '<i class="fa fa-check" aria-hidden="true"></i>' +
                            "</span>";
                    }
                    else {
                        return '<span class="badge badge-danger">' +
                            '<i class="fa fa-window-close" aria-hidden="true"></i>' +
                            "</span>";
                    }
                }
            },
            {
                "data": "",
                "render": function (data, type, row) {
                    var id = row.id;
                    var disponible = row.Disponible;
                    if (disponible) {
                        return "<span data-toggle='tooltip' title='Déplacer vers le stock ouvert' style='cursor:pointer' onclick='Deplacer(" + "\"" + id + "\"" + ")' class='badge badge-primary' >" + "<i class='fa fa-paper-plane' aria-hidden='true'></i>" + "</span>"
                    }
                    else {
                        return "";
                    }

                }
            },
            {
                "data": "Disponible"
            }
        ],
        "columnDefs": [
            {
                "targets": [0,9],
                "visible": false,
                "searchable": false
            }],
       "drawCallback": function (settings) {
           $('[data-toggle="tooltip"]').tooltip();
       }
    });

}
function UploadImgUrl(id) {
    $.ajax({
        type: "GET",
        url: "/Stock/OpenImageUploader",
        data: { id: id },
        beforeSend: function () {
            $.blockUI({ message: "Patientez un peu..." });
        },
        success: function (data) {
            
            $("#GenericModel").modal(
                {
                    backdrop: "static",
                    keyboard: false
                }
            );
            $("#ModalTitle").text("Image");
            $("#ModalBody").html(data);


        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function Deplacer(id) {
    
    var codeArticle = $("#codeArticle").val();
    $.ajax({
        type: "POST",
        url: "/Stock/DeplacerVersStockOuvert",
        data: {id:id},
        encode: true,
        success: function (response) {
            $("#ArticleDetailsGrid").DataTable().ajax.reload(null, false);
            GetQte()
            toastr.success("", "opération réussite", { progressBar: true, showDuration: 100 });
            GetQte(codeArticle);
        },
    });
}
function GetQte(article) {
    $.ajax({
        type: "POST",
        url: "/Stock/GetQte",
        data: { article: article },
        encode: true,
        success: function (response) {
            $("#metres").text(response.Qte);
            $("#Rouloux").text(response.Rouloux);
        },
    });
}