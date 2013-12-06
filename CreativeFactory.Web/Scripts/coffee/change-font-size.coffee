$(document).ready ->
  $("div#body-content").css "font-size", $.cookie("font-size") + "px"  if $.cookie("font-size")
  originalSize = $("div").css("font-size")
  
  # reset
  $(".resetMe").click ->
    $("div#body-content").css "font-size", originalSize
    $.cookie "font-size", originalSize,
      expires: 10
      path: "/"


  
  # Increase Font Size
  $(".increase").click ->
    currentSize = $("div").css("font-size")
    currentSize = Math.round(parseFloat(currentSize) * 2)
    $("div#body-content").css "font-size", currentSize
    $.cookie "font-size", currentSize,
      expires: 10
      path: "/"

    false

  
  # Decrease Font Size
  $(".decrease").click ->
    currentFontSize = $("div").css("font-size")
    currentSize = $("div").css("font-size")
    currentSize = Math.round(parseFloat(currentSize) * 0.8)
    $("div#body-content").css "font-size", currentSize
    $.cookie "font-size", currentSize,
      expires: 10
      path: "/"

    false