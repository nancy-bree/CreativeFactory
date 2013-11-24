$(function () {
    $(".deleteBtn").click(function (e) {
        e.preventDefault();
        pid = $(this).parent().attr("id");
        username = $(this).parent().data("username");
        $('#dialog-confirm').dialog('open');
    });
    $("#dialog-confirm").dialog({
        dialogClass: "alert alert-info",
        open: function (event) {
            $('.ui-dialog-buttonpane').find('button:contains("Cancel")').addClass('btn btn-primary btn-xs');
            $('.ui-dialog-buttonpane').find('button:contains("Delete")').addClass('btn btn-primary btn-xs');
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").hide();
        },
        resizable: false,
        height: 100,
        modal: true,
        autoOpen: false,
        buttons: {
            "Delete": function () {
                deleteRecord(username);
                $("td#" + pid).parent().fadeOut("slow").remove();
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
            data: { username: username }
        });
    };
});