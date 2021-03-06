(function( $ ) {
    var timeout = new Array();
    
    $.fn.autosave = function(options, callback) {
        return this.each(function(index) {
            
            var settings = $.extend(true, { 
                    'delay' : 3000,
                    'url' : '',
                    beforeSave : function() { },
                    afterSave : function() { }
                }, options);

            var input = $(this);
            var id = input.attr('id');
            var token = input.data("token");
            var title = $("form").find("#Title").val();

            input.bind('keyup', function(){
                clearTimeout(timeout[id]);

                timeout[id] = setTimeout(function(){

                    settings.beforeSave.call(this);

                    $.post(settings.url, {content: input.val(), token: token, title: title}, function(){
                        settings.afterSave.call(this);
                    });

                }, settings.delay); 
            });
            
        });
    };
    
})( jQuery );