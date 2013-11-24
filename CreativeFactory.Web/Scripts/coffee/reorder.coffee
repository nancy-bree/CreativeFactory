$(document).ready ->
  $("#sortable").sortable update: (event, ui) ->
    url = $(this).data("url")
    newOrder = $(this).sortable("toArray").toString()
    $.get url,
      order: newOrder