function LoadhistoryCustomerData() {
    var table = $('#historyCustomerGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: '/Vente/GetAllCustomerSale',
            dataSrc: ''
        },
        columns: [
            { "data": "client" },
            { "data": "reference" },
            { "data": "quantite" },
        ],
        "drawCallback": function (settings) {
            $('[data-toggle="tooltip"]').tooltip();
        },
        drawCallback: function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            api
                .column(0, { page: 'current' })
                .data()
                .each(function (group, i) {
                    if (last !== group) {
                        $(rows)
                            .eq(i)
                            .before('<tr align=left style="background-color:antiquewhite" class="group"><td colspan="3">' + group + '</td></tr>');

                        last = group;
                    }
                });
        },
        "columnDefs": [
        {
            "targets": [0],
            "visible": false,
            "searchable": false
        }]
    });
}