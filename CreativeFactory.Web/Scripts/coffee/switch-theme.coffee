$("link#site-theme").attr "href", $.cookie("css")  if $.cookie("css")
$(document).ready ->
  $("a.theme").click ->
    $("link#site-theme").attr "href", $(this).attr("rel")
    $.cookie "css", $(this).attr("rel"),
      expires: 10
      path: "/"

    false