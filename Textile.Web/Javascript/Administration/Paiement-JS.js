
function DeleteCustomer(id) {
    
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Client");
    $("#SelectedEntity").val("Client");
    $("#DeleteMessage").text("Voulez-vous supprimer ce client ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}
function OpenPaymentEditor(id) {
    
    var client = $("#IDClient").val();
    $.ajax({
        type: "GET",
        url: "/Administration/OpenPaymentEditor",
        data: { ID: id, client: client },
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
            $("#ModalTitle").text("Paiement");
            $("#ModalBody").html(data);

        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function AddOrUpdatePayment() {
    var formData = $("#ParamForm").serializeArray();
    var client = formData[1].value;
    var checkValidityForm = $("#ParamForm")[0].checkValidity();
    if (checkValidityForm === true) {
        event.preventDefault();
        $.ajax({
            type: 'POST',
            url: "/Administration/AddOrUpdatePayment",
            data: formData,
            encode: true,
            success: function (response) {
                $('#GenericModel').modal('hide');
                $('#PaiementGrid').DataTable().ajax.reload(null, false);
                toastr.success('opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
                GetAmountByCustomer(client);
            },
        });
    }
}
function LoadPaymentData(id) {
    
    var table = $('#PaiementGrid').DataTable({
        "order": [[1, "asc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: "/Administration/GetAllPayment",
            "data": {
                "id": id
            },
            dataSrc: ''
        },
        columns: [
            { "data": "id" },

            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var id = row.id;

                    return "<div style='display:inline-flex'>" +
                        "<a onclick='OpenPaymentEditor(" + "\"" + id + "\"" + ")' href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i class='fas fa-edit'></i> </a>" +
                        // "<a onclick=Btn_Detail_click('" + id + "') href='#' class='btn btn-success btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                        "<a style='cursor:pointer;margin:0px 5px 0px 5px' onclick=DeletePayment('" + id + "')  href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-money-bill-alt'></i> </a>" +
                        "</div>"
                }
            },
            {
                "data": "Montant",
                render: function (data, type, row) {
                    var numFormat = $.fn.dataTable.render.number(' ', ',', 3, '').display;
                    return numFormat(data) + " DT";
                }
            },
            { "data": "TypePaiement" },
            {
                "data": "DatePaiement",
                render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            {
                "data": "DateEcheance",
                render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },



        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }]
    });
}
function LoadSaleDataByCustomer(id) {
    
    var table = $('#VenteGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Vente/GetAllVentesByClient',
            "data": {
                "id": id
            },
            dataSrc: ''
        },
        columns: [
            { "data": "id" },
         
            { "data": "Code" },

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
                        return "<div style='display:inline-flex'>" + "<span class='badge badge-success'>Valide</span></div>"
                    }
                    else {
                        return "<div style='display:inline-flex'>" + "<span class='badge badge-danger'>Non Valide</span></div>"
                    }

                }
            },
            {
                "data": "TotalAmount",
                render: function (data, type, row) {
                    if(data != ""){
                        var numFormat = $.fn.dataTable.render.number(' ', ',', 3, '').display;
                        return numFormat(data) + " DT";
                    }
                    return ""
                }
            },
            {
                "data": "",
                "render": function (data, type, row) {

                    var id = row.id;

                    return "<div style='display:inline-flex'>" +
                        "<a data-toggle='tooltip'  title='Afficher' onclick='ShowInvoice(" +
                        "\"" +
                        id +
                        "\"" +
                        ")' href='#' class='btn btn-info btn-circle btn-sm'>" +
                        "<i class='fas fa-eye'></i> </a>" +
                        "<a data-toggle='tooltip'  title='Imprimer' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=Print('" +
                        id +
                        "')  href='#' class='btn btn-primary btn-circle btn-sm'>" +
                        "<i class='fas fa-print'></i> </a>" +
                        "</div>";
                }
            },
        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }], "drawCallback": function (settings) {
            $('[data-toggle="tooltip"]').tooltip();
        }

    });
}
function ShowInvoice(id) {
    window.open("/Vente/VenteDetails?Code=" + id);
}
function update(value) {
    
    $.ajax({
        type: "GET",
        url: "/Administration/EnableOrDisableCustomer",
        data: { id: value },
        beforeSend: function () {

        },
        success: function (data) {
            $('#ClientsGrid').DataTable().ajax.reload(null, false);
        },
        complete: function () {

        },
        failure: function (response) { }
    });
}
function GetAmountByCustomer(id) {
    $.ajax({
        type: "GET",
        url: "/Vente/GetAmountByCustomer",
        data: { id: id },
        beforeSend: function () {

        },
        success: function (data) {
           
            var payrollAmount = Number(data.payrollAmount).toFixed(3).replace(/\d(?=(\d{3})+\.)/g, '$&,'); // 12,345.67
            $("#payrollAmount").text(payrollAmount);
            var receivedAmount = Number(data.receivedAmount).toFixed(3).replace(/\d(?=(\d{3})+\.)/g, '$&,'); // 12,345.67
            $("#receivedAmount").text(receivedAmount);
           var rest = Number(data.payrollAmount - data.receivedAmount).toFixed(3).replace(/\d(?=(\d{3})+\.)/g, '$&,')
            $("#restAmount").text(rest);
            var element = document.getElementById("restAmount");
            if (data.payrollAmount - data.receivedAmount < 0) {
                element.style.color = "red";
            }
            else {
                element.style.color = "green";
            }
            
        },
        complete: function () {

        },
        failure: function (response) { }
    });
}
function DeletePayment(id) {
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Paiement");
    $("#SelectedEntity").val("Paiement");
    $("#DeleteMessage").text("Voulez-vous supprimer ce paiement ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}