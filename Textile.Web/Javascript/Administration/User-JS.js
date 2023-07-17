//User
function LoadUserData() {
    
    var table = $('#UsersGrid').DataTable({
        "order": [[3, "asc"]],
        "autoWidth": true,
        responsive: true,
        colReorder: true,
        ajax: {
            url: "/Administration/GetAllUsers",
            dataSrc: ''
        },
        columns: [
            { "data": "ID" },
            {
                "data": "",
                "render": function (data, type, row) {
                    
                    var UserID = row.ID;
                    return "<div style='display:inline-flex'>" +
                        "<a onclick='OpenUserEditor(" + "\"" + UserID + "\"" + ")' href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i class='fas fa-edit'></i> </a>" +
                        "<a style='cursor:pointer;margin:0px 5px 0px 5px' onclick=OpenUserRolesEditor('" + UserID + "') href='#' class='btn btn-success btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                        "<a  onclick=DeleteUser('" + UserID + "')  href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                        "</div>"
                }
            },
            { "data": "UserName" },
            {
                "data": "",
                "render": function (data, type, row) {
                    return "<label>" + row.FirstName + " " + row.LastName +"</label>"
                }
            },
            { "data": "Email" },
            { "data": "Phone" },
            {
                "data": "IsActive",
                render: function (data, type, row) {
                    var id = row.ID;
                    if (data === true) {
                        return "<div style='display:inline-flex'>" +
                            "<i  onclick=EnableUser('" +
                            id +
                            "') style='color:green;cursor:pointer' class='fas fa-toggle-on fa-2x' aria-hidden='true'></i></div>";

                    } else {
                        return "<div style='display:inline-flex'><i onclick=EnableUser('" +
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
            }],
        "fnRowCallback": function (row, aData) {
            
            if (aData.IsActive == false) {

                $(row).css({ "background-color": "#f080807a" });
            }
        }
    });

}
function OpenUserEditor(ID) {
    
    $.ajax({
        type: "GET",
        url: "/Administration/OpenUserEditor",
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
            $("#ModalTitle").text("Utilisateur");
            $("#ModalBody").html(data);

        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function DeleteUser(ID) {
    $("#SelectedID").val(ID);
    $("#DeleteItem").text("User");
    $("#SelectedEntity").val("User");
    $("#DeleteMessage").text("Voulez-vous supprimer cet utilisateur ?");
    $("#confirm-delete").modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}
function ChangeUserRole(user) {
    var roleId = user.split(',')[0];
    var userId = user.split(',')[1];
    var isChecked = $("#Checkbox_" + roleId).is(":checked");
    $.ajax({
        type: "GET",
        url: "/Administration/ChangeUserRole",
        data: { userId: userId, roleId: roleId, isChecked: isChecked },
        success: function (data) {
            toastr.success("opération réussite");
        },
    });
}
function OpenUserRolesEditor(ID) {
    $.ajax({
        type: "GET",
        url: "/Administration/OpenUserRolesEditor",
        data: { ID: ID },
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });
        },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Roles");
            $("#ModalBody").html(data);

        },
        complete: function () {
            $.unblockUI();
        },
        failure: function (response) { }
    });
}
function EnableUser(id) {
    $.ajax({
        type: "GET",
        url: "/Administration/EnableOrDisableUser",
        data: { id: id },
        beforeSend: function () {
        },
        success: function (data) {
            $("#UsersGrid").DataTable().ajax.reload(null, false);
            toastr.success("opération réussite");
        },
        complete: function () {
        },
        failure: function (response) {
            toastr.error("opération échouée");
        }
    });

}
function AddOrUpdateUser() {
    var userName = $("#UserName").val();
    var selectedUser = $("#ID").val();
    $.ajax({
        type: "GET",
        url: "/Administration/CheckUserName",
        data: { id: selectedUser, userName: userName },
        beforeSend: function () {
        },
        success: function (data) {
           
            if (data == "True") {
                var formData = $("#ParamForm").serializeArray();
                var CheckValidityForm = $("#ParamForm")[0].checkValidity();
                if (CheckValidityForm == true) {
                    event.preventDefault();
                    $.ajax({
                        type: 'POST',
                        url: "/Administration/AddOrUpdateUser",
                        data: formData,
                        encode: true,
                        success: function (response) {
                            $('#GenericModel').modal('hide');
                            $('#UsersGrid').DataTable().ajax.reload(null, false);
                            toastr.success('opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
                        },
                    });
                }
            }
            else {
                $("#UserName").css("border-color", "red");
                toastr.warning("Nom d'utilisateur existe déjà");
                event.preventDefault();
            }
        },
        complete: function () {
        },
        failure: function (response) {
            toastr.error("opération échouée");
        }
    });
    event.preventDefault();
}