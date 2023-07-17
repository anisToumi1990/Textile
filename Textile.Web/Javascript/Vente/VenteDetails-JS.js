function LoadVenteDetailsData(code, status) {
    var table = $('#VenteDetailsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Vente/GetAllVenteDetails',
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
                    if (status == "Valide") {
                        return "<div style='display:inline-flex'>" +
                            "<a data-toggle='tooltip'  title='Modifier' style='cursor:pointer;' onclick='OpenVenteDetailsEditor(" + "\"" + id + "\"" + ")' href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i class='fas fa-edit'></i> </a>" +
                            "<a data-toggle='tooltip'  title='Supprimer' style='cursor:pointer;margin:0px 5px 0px 5px' onclick=DeleteVenteDetails('" + id + "')  href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                            "</div>";
                    }
                    else {
                        return "<div style='display:inline-flex'>" +
                            "<a data-toggle='tooltip'  title='Modifier' style='cursor:not-allowed;'  href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i class='fas fa-edit'></i> </a>" +
                            "<a data-toggle='tooltip'  title='Modifier' style='cursor:not-allowed;margin:0px 5px 0px 5px'   href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                            "</div>";
                    }
                   

                }
            },
            { "data": "CodeReference" },
            { "data": "Description" },
            
            {
                "data": "DateCreation",
                render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY");
                }
            }
            ,
            {
                "data": "Qte",
                render: function (data, type, row) {
                    return data + "m"
                }
            },
            {
                "data": "PrixUnitaire",
                render: function (data, type, row) {
                    
                    var numFormat = $.fn.dataTable.render.number(' ', ',', 3, '').display;
                    return numFormat(data ) + " DT";
                }
            },
            
            {
                "data": "_PrixTotal",   
                render: function (data, type, row) {
                    var numFormat = $.fn.dataTable.render.number(' ', ',', 3, '').display;
                    return numFormat(data) + " DT";
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
        },
        "footerCallback": function (row, data, start, end, display) {
            
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };
            totalR = api
                .column(7)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var numFormat = $.fn.dataTable.render.number(' ', ',', 3, '').display;

            $(api.column(7).footer()).html(
                numFormat(totalR) + ' DT'
                //+ ' ( ' + total + ' TND total)'
            );
        }
    });
}
function AddOrUpdateVenteDetail() {
    
    var formData = $("#ParamForm").serializeArray();
    var qte = formData[5].value;
    var qteDispo = $("#availableQte").val();
    if (parseFloat(qte) > parseFloat(qteDispo)) {
        $("#Qte").css("border-color", "red");
        toastr.error("Quantité doit être inférieure ou égale à la quantité disponible: " +
            qteDispo + (" m"));
        event.preventDefault();
    }
    else {
        $("#Qte").css("border-color", "green");
        var checkValidityForm = $("#ParamForm")[0].checkValidity();
        if (checkValidityForm === true) {
            event.preventDefault();
            $.ajax({
                type: 'POST',
                url: "/Vente/AddOrUpdateVenteDetail",
                data: formData,
                encode: true,
                success: function (response) {
                    $('#GenericModel').modal('hide');
                    $('#VenteDetailsGrid').DataTable().ajax.reload(null, false);
                    toastr.success('Détail vente ajouté avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                },
            });
        }
    }   
}
function OpenVenteDetailsEditor(id) {
    var vente = $("#Code").val();
    $.ajax({
        type: "GET",
        url: "/Vente/OpenVenteDetailsEditor",
        data: { ID: id, vente: vente },
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
            $("#ModalTitle").text("Vente-Detail");
            $("#ModalBody").html(data);
        },
        complete: function () {
            $.unblockUI();
        },
        failure: function () { }
    });
}
function DeleteVenteDetails(id) {
    $("#SelectedID").val(id);
    $("#DeleteItem").text("Vente-Détail");
    $("#SelectedEntity").val("Vente-Détail");
    $("#DeleteMessage").text("Voulez-vous supprimer ce détail ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}





