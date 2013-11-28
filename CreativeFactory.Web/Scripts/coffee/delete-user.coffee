$ ->
  deleteRecord = (pid) ->
    $.ajax
      type: "POST"
      url: $(".deleteBtn").attr("href")
      data:
        username: username

  $(".deleteBtn").click (e) ->
    e.preventDefault()
	exports = this
    exports.pid = $(this).parent().parent().attr("id")
    exports.username = $(this).parent().parent().data("username")
    $("#dialog-confirm").dialog "open"

  $("#dialog-confirm").dialog
    dialogClass: "alert alert-info"
    open: (event) ->
      $(".ui-dialog-buttonpane").find("button:contains(\"Cancel\")").addClass "btn btn-primary btn-xs"
      $(".ui-dialog-buttonpane").find("button:contains(\"OK\")").addClass "btn btn-primary btn-xs"
      $(".ui-dialog-titlebar-close").hide()
      $(".ui-dialog-title").hide()

    resizable: false
    height: 100
    modal: true
    autoOpen: false
    buttons:
      OK: ->
        deleteRecord username
        $("td#" + pid).parent().fadeOut("slow").remove()
        $(this).dialog "close"

      Cancel: ->
        $(this).dialog "close"