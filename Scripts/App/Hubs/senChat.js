/*
 * Author: Valerio Gheri
 * Date: 22/07/2012
 * Description: senChat namespace js file and viewmodels declaration
 */

// Namespace
var senChat = {};

// Models

senChat.chatMessage = function (userid,sender, content, dateSent,avatar) {
    var self = this;
    self.UserId = userid;
    self.UserName = sender;
    self.content = content;
    self.Avatar = avatar;
    if (dateSent != null) {
        self.timestamp = dateSent;
    }
}

senChat.sendToGroup = function (groupname,sender, content, dateSent) {
    var self = this;
    self.UserName = sender;
    self.content = content;
    self.groupname = groupname;
    if (dateSent != null) {
        self.timestamp = dateSent;
    }
}


senChat.user = function (username, userid, avatar, lastactive, senuserid) {
    var self = this;
    self.UserName = username;
    self.UserId = userid;
    self.Avatar = avatar;
    self.LastActive = lastactive;
    self.SenUserId = senuserid;
}

// ViewModels

senChat.chatViewModel = function () {
    var self = this;
    self.messages = ko.observableArray();
}

senChat.connectedUsersViewModel = function () {
    var self = this;
    self.contacts = ko.observableArray();
    self.customRemove = function (userToRemove) {
        var userIdToRemove = userToRemove.id;
        self.contacts.remove(function (item) {
            return item.id === userIdToRemove;
        });
    }
}

// IE doesn't parse IS8601 formatted dates, so I had to find this function to parse it
// (URL http://dansnetwork.com/javascript-iso8601rfc3339-date-parser/ )
Date.prototype.setISO8601 = function (dString) {

    var regexp = /(\d\d\d\d)(-)?(\d\d)(-)?(\d\d)(T)?(\d\d)(:)?(\d\d)(:)?(\d\d)(\.\d+)?(Z|([+-])(\d\d)(:)?(\d\d))/;

    if (dString.toString().match(new RegExp(regexp))) {
        var d = dString.match(new RegExp(regexp));
        var offset = 0;

        this.setUTCDate(1);
        this.setUTCFullYear(parseInt(d[1], 10));
        this.setUTCMonth(parseInt(d[3], 10) - 1);
        this.setUTCDate(parseInt(d[5], 10));
        this.setUTCHours(parseInt(d[7], 10));
        this.setUTCMinutes(parseInt(d[9], 10));
        this.setUTCSeconds(parseInt(d[11], 10));
        if (d[12])
            this.setUTCMilliseconds(parseFloat(d[12]) * 1000);
        else
            this.setUTCMilliseconds(0);
        if (d[13] != 'Z') {
            offset = (d[15] * 60) + parseInt(d[17], 10);
            offset *= ((d[14] == '-') ? -1 : 1);
            this.setTime(this.getTime() - offset * 60 * 1000);
        }
    }
    else {
        this.setTime(Date.parse(dString));
    }
    return this;
};