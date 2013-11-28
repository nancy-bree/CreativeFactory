$(function () {
    $(".deleteBtn").click(function (e) {
        e.preventDefault();
        pid = $(this).parent().parent().parent().attr("id");
        $('#dialog-confirm').dialog('open');
    });
    $("#dialog-confirm").dialog({
        dialogClass: "alert alert-info",
        open: function (event) {
            $('.ui-dialog-buttonpane').find('button:contains("Cancel")').addClass('btn btn-primary btn-xs');
            $('.ui-dialog-buttonpane').find('button:contains("OK")').addClass('btn btn-primary btn-xs');
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").hide();
        },
        resizable: false,
        height: 100,
        modal: true,
        autoOpen: false,
        buttons: {
            "OK": function () {
                deleteRecord(pid);
                $("div#" + pid).fadeOut("slow").remove();
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    function deleteRecord(pid) {
        $.ajax({
            type: "POST",
            url: $(".deleteBtn").attr("href"),
            data: { id: pid }
        });
    };
});