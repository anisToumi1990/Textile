function LoadVenteData() {
    var table = $('#VenteGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Vente/GetAllVentes',
            dataSrc: ''
        },
        columns: [
            { "data": "id" },
            {
                "data": "",
                "render": function (data, type, row) {
                    var id = row.id;
                    if (row.Status === "Valide") {
                        return "<div style='display:inline-flex'>" +
                            "<a data-toggle='tooltip'  title='Modifier la facture' onclick='OpenVenteEditor(" +
                            "\"" +
                            id +
                            "\"" +
                            ")' href='#' class='btn btn-warning btn-circle btn-sm'>" +
                            "<i class='fas fa-edit'></i> </a>" +
                            "<a data-toggle='tooltip' title='Afficher la facture' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=Btn_Detail_click('" +
                            id +
                            "') href='#' class='btn btn-success btn-circle btn-sm'>" +
                            "<i class='fas fa-list'></i> </a>" +
                            "<a data-toggle='tooltip' title='Annuler la facture' onclick=AnnulerOuActiverVente('" +
                            id +
                            "')  href='#' class='btn btn-danger btn-circle btn-sm'>" +
                            "<i class='fas fa-times'></i> </a>" +
                            "</div>";
                    }
                    else {
                        return "<div style='display:inline-flex'>" +
                            "<a data-toggle='tooltip' title='Modifier la facture' onclick='OpenVenteEditor(" +
                            "\"" +
                            id +
                            "\"" +
                            ")' href='#' class='btn btn-warning btn-circle btn-sm'>" +
                            "<i class='fas fa-edit'></i> </a>" +
                            "<a data-toggle='tooltip' title='Afficher la facture' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=Btn_Detail_click('" +
                            id +
                            "') href='#' class='btn btn-success btn-circle btn-sm'>" +
                            "<i class='fas fa-list'></i> </a>" +
                            "<a data-toggle='tooltip' title='Activer la facture' onclick=AnnulerOuActiverVente('" +
                            id +
                            "')  href='#' class='btn btn-success btn-circle btn-sm'>" +
                            "<i class='fas fa-undo'></i> </a>" +
                            "</div>";
                    }
                   
                }
            },
            { "data": "Code" },
            { "data": "Type" },
            { "data": "NomClient" },            
            
            {
                "data": "DateCreation",
                render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            {
                "data": "Status",
                "render": function (data, type, row) {                    
                    if (row.Status == "Valide") {
                        return "<div style='display:inline-flex'>" +
                            "<span class='badge badge-success'>Valide</span></div>";
                    }
                    else {
                        return "<div style='display:inline-flex'>" +
                            "<span class='badge badge-danger'>Non Valide</span></div>";
                    }
                    
                }
            },
            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var id = row.id;
                    return "<div  style='display:inline-flex'>" + "<span data-toggle='tooltip' title='Imprimer la facture' style='cursor:pointer' onclick='Print(" + "\"" + id + "\"" + ")' class='badge badge-primary'><i class='fas fa-file-file-pdf'></i> Imprimer</span></div>"

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
function OpenVenteEditor(ID) {
    
    $.ajax({
        type: "GET",
        url: "/Vente/OpenVenteEditor",
        data: { ID: ID },
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });
        },
        success: function (data) {
            
            $('#GenericModel').modal(
                {
                    backdrop: 'static',
                    keyboard: false
                }
            );
            $("#ModalTitle").text("Vente");
            $("#ModalBody").html(data);
        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function AddOrUpdateVente() {
    var formData = $("#ParamForm").serializeArray();
    var CheckValidityForm = $("#ParamForm")[0].checkValidity();
    if (CheckValidityForm == true) {
        event.preventDefault();
        $.ajax({
            type: 'POST',
            url: "/Vente/AddOrUpdateVente",
            data: formData,
            encode: true,
            success: function (response) {
                $('#GenericModel').modal('hide');
                $('#VenteGrid').DataTable().ajax.reload(null, false);
                toastr.success('Article ajouté avec succès', 'Succès', { progressBar: true, showDuration: 100 });
            },
        });
    }
}
function Btn_Detail_click(MyValue) {
    if (MyValue != null) {
        window.open("/Vente/VenteDetails?Code=" + MyValue, '_blanck');
    }
}
function AnnulerOuActiverVente(id) {
    
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Vente");
    $("#SelectedEntity").val("Vente");
    $("#DeleteMessage").text("Voulez-vous annuler ou activer cette facture ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}
