  $(".deleteBtn").click (e) ->
    e.preventDefault()
    pid = $(this).parent().parent().attr("id")
    $("#dialog-confirm").dialog "open"

  $("#dialog-confirm").dialog
    dialogClass: "alert alert-info"
    open: (event) ->
      $(".ui-dialog-buttonpane").find("button:contains(\"Cancel\")").addClass "btn btn-primary btn-xs"
      $(".ui-dialog-buttonpane").find("button:contains(\"Delete\")").addClass "btn btn-primary btn-xs"
      $(".ui-dialog-titlebar-close").hide()
      $(".ui-dialog-title").hide()

    resizable: false
    height: 100
    modal: true
    autoOpen: false
    buttons:
      Delete: ->
        deleteRecord pid
        $("div#" + pid).fadeOut "slow"
        $(this).dialog "close"

      Cancel: ->
        $(this).dialog "close"
        
        $ ->
  deleteRecord = (pid) ->
    $.ajax
      type: "POST"
      url: $(".deleteBtn").attr("href")
      data:
        id: pid