$(function () {
    $(".adminBtn").click(function (e) {
        e.preventDefault();
        self = $(this)
        pid = $(this).parent().attr("id");
        username = $(this).parent().data("username");
        if ($(this).hasClass("excludeFromAdmins")) {
            $.ajax({
                type: "POST",
                url: $(".adminBtn").attr("href"),
                data: { username: username, act: "exclude" },
                success: function () {
                    self.removeClass("excludeFromAdmins")
                    .removeClass("btn-danger")
                    .addClass("addToAdmins")
                    .addClass("btn-success")
                    .text(self.data("inverse"));
                }
            });
        }
        else {
            $.ajax({
                type: "POST",
                url: $(".adminBtn").attr("href"),
                data: { username: username, act: "add" },
                success: function () {
                    self.removeClass("addToAdmins")
                    .removeClass("btn-success")
                    .addClass("excludeFromAdmins")
                    .addClass("btn-danger")
                    .text(self.data("inverse"));
                }
            });
        }
    });
});