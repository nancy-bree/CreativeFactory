$ ->
  resetPassword = (username, email) ->
    $.ajax
      type: "POST"
      url: $(".resetPassword").attr("href")
      data:
        username: username
        emailAddress: email

      success: ->
        $("#pswd-reset-modal").dialog("open").dialog "close"

  $(".resetPassword").click (e) ->
    e.preventDefault()
    username = $(this).parent().data("username")
    email = $(this).parent().data("email")
    resetPassword username, email

  $("#pswd-reset-modal").dialog
    dialogClass: "alert alert-info"
    open: (event) ->
      $(".ui-dialog-titlebar-close").hide()
      $(".ui-dialog-title").hide()

    resizable: false
    height: 100
    modal: true
    autoOpen: false
    hide:
      effect: "fadeOut"
      duration: 2000