$(document).ready(function () {
    //Variables
    var proxy = $.connection.groupUpdates;
    var connected = false;
    var groupName;

    //Logging
    //$.connection.hub.logging = true;

    //Server side functions

    //Client side functions
    proxy.client.updateConnectionCount = function (x) {
        $("#connections").text(x);
    }

    proxy.client.broadcastMessage = function (from, grp, msg) {
        $("ul").append("<li><b>" + from + "</b> -> <b>" + grp + "</b> : " + msg + "</li>");
    }

    proxy.client.addGroupToList = function (grp) {
        if ($("select").index("<option value='" + grp + "'>" + grp + "</option>") == -1) {
            $("select").append("<option value='" + grp + "'>" + grp + "</option>");
        }
    }

    proxy.client.deleteAllGroups = function () {
        $("#groups option").each(function () {
            $(this).remove();
        });
    }

    //Event handlers
    $("button#send-message").click(function () {
        var group;
        var message;

        group = $("#groups").val();
        message = $("input#message-content").val();

        proxy.server.sendGroupMessage(groupName, message, group);

        $("input#message-content").val("");
        $("input#message-content").focus();
    });

    $("button#send-data").click(function () {
        proxy.server.sendGroupsData();
    });

    // Connection
    $.connection.hub.start()
        .done(function () {
            groupName = prompt("Choose a group name", "");

            if (groupName != null && groupName != "") {
                proxy.server.updateGroupName(groupName);
                $("#yourGroupName").text(groupName);
                connected = true;
            }
            else {
                var randName = "AnonGroup" + Math.floor(Math.random() * 1000000).toString();

                proxy.server.updateGroupName(randName);
                $("#yourGroupName").text(randName);
                connected = true;
            }
    });
});