//Clients

function GetPayment(id) {
    if (id != null) {
        window.open("/Administration/Payment?id=" + id, "_blanck");
    }
}

function OpenClientsEditor(ID) {
    $.ajax({
        type: "GET",
        url: "/Administration/OpenClientsEditor",
        data: { ID: ID },
        beforeSend: function() {
            $.blockUI({ message: 'Patientez un peu...' });

        },
        success: function(data) {
            $("#GenericModel").modal(
                {
                    backdrop: "static",
                    keyboard: false
                }
            );
            $("#ModalTitle").text("Client");
            $("#ModalBody").html(data);

        },
        complete: function() {
            $.unblockUI();
        },
        failure: function(response) {}
    });
}

function AddOrUpdateCustomer() {
    var formData = $("#ParamForm").serializeArray();
    var CheckValidityForm = $("#ParamForm")[0].checkValidity();
    if (CheckValidityForm == true) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Administration/AddOrUpdateCustomer",
            data: formData,
            encode: true,
            success: function(response) {

                //var res = JSON.parse(response);

                $("#GenericModel").modal("hide");
                $("#ClientsGrid").DataTable().ajax.reload(null, false);
                toastr.success("opération  reussite", "Succès", { progressBar: true, showDuration: 100 });
            },
        });
    }
}

function LoadClientData() {

    var table = $("#ClientsGrid").DataTable({
        "order": [[1, "asc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: "/Administration/GetAllClients",
            dataSrc: ""
        },
        columns: [
            { "data": "idClient" },
            {
                "data": "",
                "render": function(data, type, row) {

                    var clientId = row.idClient;

                    return "<div style='display:inline-flex'>" +
                        "<a data-toggle='tooltip'  title='Modifier le client' onclick='OpenClientsEditor(" +
                        "\"" +
                        clientId +
                        "\"" +
                        ")' href='#' class='btn btn-warning btn-circle btn-sm'>" +
                        "<i class='fas fa-edit'></i> </a>" +
                        // "<a onclick=Btn_Detail_click('" + id + "') href='#' class='btn btn-success btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                        "<a data-toggle='tooltip'  title='Afficher le paiement' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=GetPayment('" +
                        clientId +
                        "')  href='#' class='btn btn-primary btn-circle btn-sm'>" +
                        "<i class='fas fa-money-bill-alt'></i> </a>" +
                        "</div>";


                }
            },
            { "data": "Nom" },
            { "data": "Adresse" },
            { "data": "NumeroTelephoe" },
            {
                "data": "Active",
                render: function(data, type, row) {
                    var id = row.idClient;
                    if (data === true) {
                        return "<div style='display:inline-flex'>" +
                            "<i  onclick=EnableCustomer('" +
                            id +
                            "') style='color:green;cursor:pointer' class='fas fa-toggle-on fa-2x' aria-hidden='true'></i></div>";

                    } else {
                        return "<div style='display:inline-flex'><i onclick=update('" +
                            id +
                            "') style='color:red;cursor:pointer' class='fas fa-toggle-off fa-2x' aria-hidden='true'></i></div>";

                    }
                }
            }
        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }
        ], "drawCallback": function (settings) {
            $('[data-toggle="tooltip"]').tooltip();
        }
    });

}

function EnableCustomer(value) {

    $.ajax({
        type: "GET",
        url: "/Administration/EnableOrDisableCustomer",
        data: { id: value },
        beforeSend: function() {

        },
        success: function(data) {
            $("#ClientsGrid").DataTable().ajax.reload(null, false);
            toastr.success("opération  reussite");
        },
        complete: function() {

        },
        failure: function(response) {}
    });
}