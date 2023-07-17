function LoadData() {
    var table = $('#StockGrid').DataTable({
        "order": [[0, "asc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Stock/GetAllOpenStock',
            dataSrc: ''
        },
        columns: [
            { "data": "id" },
            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var id = row.id;
                    return "<span style='cursor:pointer;' onclick='Mouvement(" + "\"" + id + "\"" + ")' class='badge badge-primary' >" + "<i class='fas fa-list'></i> Historiques" + " </span>"
}
            },
            {
                "data": "reference",
                "render": function (data, type, row) {
                    
                    var id = row.id;
                    return "<span style='font-weight:bold'>"+data+"</span>"
                }
            },
            {
                "data": "qte",
                render: function (data, type, row) {
                    if (data != null && data !== 0)  {
                        return "<span class='badge badge-success' >" + data + "m </span>"
                    }
                    else {
                        return "<span class='badge badge-danger' >" + 0 + "m </span>"
                    }
                }
            },
            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var id = row.id;
                    if (row.qte > 0) {
                        return "<span style='cursor:pointer;' onclick='Dechet(" + "\"" + id + "\"" + ")' class='badge badge-danger' >" + "<i class='fas fa-trash'></i> Dechet" + " </span>"
                    }
                    else {
                        return "";
                    }

                }
            },
        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
           }],
        
    });
}
function Mouvement(id ) {
    if (id != null) {
        window.location.href = "/Stock/Mouvement?Code=" + id;
    }
}
function Dechet(id) {
    $("#SelectedDechetID").val(id);
    $("#Qte").val();
    $.ajax({
        type: 'POST',
        url: "/Stock/GetQteToDechet",
        data: { id: id},
        encode: true,
        success: function (response) {
            $("#Qte").val(response);
            $("#confirm-dechet").modal(
                {
                    backdrop: 'static',
                    keyboard: false
                }
            );
        },
    });
   
}
function ConfirmDechet(id) {

    id = $("#SelectedDechetID").val();
    var qte = $("#Qte").val();
    $.ajax({
        type: 'POST',
        url: "/Stock/Dechet",
        data: { id: id, Qte: qte},
        encode: true,
        success: function (response) {
            $('#StockGrid').DataTable().ajax.reload(null, false);
            $("#confirm-dechet").modal("hide");
            toastr.success('', 'opération réussite', { progressBar: true, showDuration: 100 });
        }
    });
}