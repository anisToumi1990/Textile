function DeleteReference(id) {
    
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Reference");
    $("#SelectedEntity").val("Reference");
    $("#DeleteMessage").text("Voulez-vous supprimer cette référence ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}
function OpenReferenceEditor(id) {
    
    $.ajax({
        type: "GET",
        url: "/Stock/OpenReferenceEditor",
        data: { ID: id },
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
            $("#ModalTitle").text("Reference");
            $("#ModalBody").html(data);

        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function AddOrUpdateReference() {
    var formData = $("#ParamForm").serializeArray();
    var checkValidityForm = $("#ParamForm")[0].checkValidity();
    if (checkValidityForm === true) {
        event.preventDefault();
        $.ajax({
            type: 'POST',
            url: "/Stock/AddOrUpdateReference",
            data: formData,
            encode: true,
            success: function (response) {
                
                //var res = JSON.parse(response);

                $('#GenericModel').modal('hide');
                $('#ReferencesGrid').DataTable().ajax.reload(null, false);
                toastr.success('opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            }
        });
    }
}
function LoadReferenceData() {
    
    var table = $('#ReferencesGrid').DataTable({
        "order": [[1, "asc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: "/Stock/GetAllReferences",
            dataSrc: ''
        },
        columns: [
            { "data": "id" },

            {
                "data": "",
                "render": function (data, type, row) {
                    var id = row.id;
                    var Libelle = row.Libellé;
                    return "<div style='display:inline-flex'>" +
                        "<a  data-toggle='tooltip' title='Modifier la référence: " + Libelle+"' onclick='OpenReferenceEditor(" + "\"" + id + "\"" + ")' href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i  class='fas fa-edit'></i> </a>" +
                        "<a data-toggle='tooltip' title='Afficher les articles de la référence: " + Libelle +"' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=Btn_Detail_click('" + id + "') href='#' class='btn btn-success btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                        "<a data-toggle='tooltip' title='Supprimer la référence: " + Libelle +"' onclick=DeleteReference('" + id + "')  href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                        "</div>"
                }
            },
            { "data": "Libellé" },
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
        window.location.href = "/Stock/Article?Code=" + myValue;
    }
}