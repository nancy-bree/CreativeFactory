$(function () {
    $(".deleteBtn").click(function (e) {
        e.preventDefault();
        pid = $(this).parent().parent().attr("id");
        username = $(this).parent().parent().data("username");
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