    $(function () {
    // Reference the auto-generated proxy for the hub.  
    var chat = $.connection.authorisationHub;
    // Create a function that the hub can call back to refresh page
    chat.client.refresh = function (name) {
        //reload page
        location.reload();
    };
    // Start the connection.
    $.connection.hub.start().done(function () { });
    
});