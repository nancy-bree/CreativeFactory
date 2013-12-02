$ ->
  ownerId = $("#sortable").data("ownerid")
  currentUserId = $("#sortable").data("currentuserid")
  $(".line").each ->
    self = $(this)
    span = self.find(".vote-span").find("span")
    if ownerId is currentUserId or currentUserId is -1
      span.hide() #hide heart
    else
      $.ajax
        type: "POST"
        url: $("#sortable").data("hasvotedurl")
        data:
          userId: currentUserId
          itemId: self.attr("id")

        success: (data) ->
          if data.result #show empty heart
            span.removeClass "glyphicon-heart-empty"
            span.addClass "glyphicon-heart"



$ ->
  $.ajaxSetup
    url: $("#sortable").data("votelogic")
    type: "POST"
    cache: "false"

  $(".vote").click ->
    self = $(this)
    span = self.find("span")
    currentUserId = $("#sortable").data("currentuserid")
    parent = self.parent().parent().parent().parent()
    itemid = parent.attr("id")
    score = parent.find(".vote-score").data("score")
    if span.hasClass("glyphicon-heart") # if already voted
      $.ajax
        data:
          userId: currentUserId
          itemId: itemid
          action: "remove"

        success: ->
          parent.find(".vote-score").html(--score).data "score", score
          span.removeClass "glyphicon-heart"
          span.addClass "glyphicon-heart-empty"

    else
      $.ajax
        data:
          userId: currentUserId
          itemId: itemid
          action: "add"

        success: ->
          parent.find(".vote-score").html(++score).data "score", +score
          span.removeClass "glyphicon-heart-empty"
          span.addClass "glyphicon-heart"