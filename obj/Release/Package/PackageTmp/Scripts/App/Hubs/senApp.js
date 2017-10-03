/*
 * Author: Valerio Gheri
 * Date: 22/07/2012
 * Description: senChat namespace js file and viewmodels declaration
 */

// Namespace
var senChat = {};

// Models

senChat.chatMessage = function (sender, content, dateSent,avatar) {
    var self = this;
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


senChat.user = function (username, userid, avatar,lastactive) {
    var self = this;
    self.UserName = username;
    self.Id = userid;
    self.Avatar = avatar;
    self.LastActive = lastactive;
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

