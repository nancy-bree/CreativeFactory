$(function () {
    $(".deleteBtn").click(function (e) {
        e.preventDefault();
        pid = $(this).parent().parent().parent().parent().parent().attr("id");
        $('#dialog-confirm').dialog('open');
    });
    $("#dialog-confirm").dialog({
        open: function (event) {
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").hide();
        },
        resizable: false,
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