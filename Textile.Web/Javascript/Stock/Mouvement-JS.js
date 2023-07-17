function LoadMouvementData(code) {
    var table = $('#MouvementGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Stock/GetAllMouvement',
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
                    if (row.Mouvement == "Dechet") {
                        return "<div style='display:inline-flex'>" + "<span style='cursor:pointer' onclick='AnnulerDechet(" + "\"" + id + "\"" + ")' class='badge badge-danger'><i class='fas fa-undo'> Annuler Dechet</i></span></div>";

                    }
                    else {
                        return "";
                    }                    
                }
            },
            {
                "data": "Mouvement"
            },
            {
                "data": "Qte",
                render: function (data, type, row) {
                    var mvt = row.Mouvement;
                    if (data != null && mvt == "Dechet") {
                        return "<span class='badge badge-danger' >" + data + " ms </span>"
                    }
                    else if (data != null && mvt == "Vente") {
                        return "<span class='badge badge-success' >" + data + " ms </span>"
                    }
                    else {
                        return "<span class='badge badge-warning' >" + data + " m </span>"
                    }
                }
            },
            {
                "data": "DateCreation",
                render: function (data, type, row) {
                    return moment(data).format("DD /MM/YYYY");
                }
            },
            {
                "data": "Facture"
            },
            {
                "data": "Client"
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
function AnnulerDechet(id) {
    if (id != null) {
        $.ajax({
            type: 'POST',
            url: "/Stock/AnnulerDechet",
            data: { id: id },
            encode: true,
            success: function (response) {
                $('#MouvementGrid').DataTable().ajax.reload(null, false);
                toastr.success('', 'opération réussite', { progressBar: true, showDuration: 100 });
            },
        });
    }
   
}