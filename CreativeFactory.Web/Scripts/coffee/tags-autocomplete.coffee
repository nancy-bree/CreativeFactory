$ ->
  split = (val) ->
    val.split /,\s*/
  extractLast = (term) ->
    split(term).pop()
  availableTags = []
  $.getJSON $("#tagsUrl").val(), (data) ->
    i = 0

    while i < data.length
      availableTags[i] = data[i]
      i++

  
  # don't navigate away from the field on tab when selecting an item
  $("#Tags").bind("keydown", (event) ->
    event.preventDefault()  if event.keyCode is $.ui.keyCode.TAB and $(this).data("ui-autocomplete").menu.active
  ).autocomplete
    minLength: 0
    source: (request, response) ->
      response $.ui.autocomplete.filter(availableTags, extractLast(request.term))

    search: ->
      
      # custom minLength
      term = extractLast(@value)
      false  if term.length < 2

    focus: ->
      
      # prevent value inserted on focus
      false

    select: (event, ui) ->
      terms = split(@value)
      
      # remove the current input
      terms.pop()
      
      # add the selected item
      terms.push ui.item.value
      
      # add placeholder to get the comma-and-space at the end
      terms.push ""
      @value = terms.join(", ")
      false