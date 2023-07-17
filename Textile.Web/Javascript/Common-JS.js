function ConfirmDelete() {
    
    $.blockUI({ message: 'Patientez un peu...' });
    var selectedId = $("#SelectedID").val();
    var entity = $("#SelectedEntity").val();
    var grid = "";
    var url = "";
    switch (entity) {
        case "Client":
            url = "/Administration/DeleteCustomer";
            grid = "ClientsGrid";
            break;
        case "Article Detail":
            url = "/Stock/DeleteArticleDetail";
            grid = "ArticleDetailsGrid";
            break;
        case "Article":
            url = "/Stock/DeleteArticle";
            grid = "ArticleGrid";
            break;
        case "Vente-Détail":
            url = "/Vente/DeleteVenteDetail";
            grid = "VenteDetailsGrid";
            break;
        case "User":
            url = "/Administration/DeleteUser";
            grid = "UsersGrid";
            break;
        case "Vente":
            url = "/Vente/AnnulerOuActiverFacture";
            grid = "VenteGrid";
            break;
        case "Reference":
            url = "/Stock/DeleteReference";
            grid = "ReferencesGrid";
            break;
        case "Paiement":
            url = "/Vente/DeletePayment";
            grid = "PaiementGrid";
            break;
        default:
    }
    
    if (url !== "") {
        $.ajax({
            url: url,
            data: { ID: selectedId },
            success: function (response) {
                $("#confirm-delete").modal("hide");
                if (response === "") {
                    toastr.success(response, 'opération réussite', { progressBar: true, showDuration: 100 });
                }
                else {
                    toastr.error(response, 'opération échouée', { progressBar: true, showDuration: 100 });
                }
                if (grid !== "") {
                    $('#' + grid + '').DataTable().ajax.reload(null, false);
                    if (entity === "Paiement") {
                        var id = $("#IDClient").val();
                        window.GetAmountByCustomer(id);
                    }
                }
            },
            complete: function () {
                $.unblockUI();
                return false;
            }
        });
    }
}
function Print(id) {
    window.open("/Vente/PrintFacture?ID=" + id, '_blank');
}
function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}
function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

    // get input value
    var inputVal = input.val();

    // don't validate empty input
    if (inputVal === "") { return; }

    // original length
    var originalLen = inputVal.length;

    // initial caret position
    var caretPos = input.prop("selectionStart");

    // check for decimal
    if (inputVal.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimalPos = inputVal.indexOf(".");

        // split number by decimal point
        var leftSide = inputVal.substring(0, decimalPos);
        var rightSide = inputVal.substring(decimalPos);

        // add commas to left side of number
        leftSide = formatNumber(leftSide);

        // validate right side
        rightSide = formatNumber(rightSide);

        // On blur make sure 2 numbers after decimal
        //if (blur === "blur") {
        //    right_side += "000";
        //}

        // Limit decimal to only 2 digits
        rightSide = rightSide.substring(0, 2);

        // join number by .
        inputVal = leftSide + "." + rightSide;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        inputVal = formatNumber(inputVal);
        inputVal = inputVal;

        // final formatting
        //if (blur === "blur") {
        //    input_val += ".000";
        //}
    }

    // send updated string to input
    input.val(inputVal);

    // put caret back in the right position
    var updatedLen = inputVal.length;
    caretPos = updatedLen - originalLen + caretPos;
    input[0].setSelectionRange(caretPos, caretPos);
}