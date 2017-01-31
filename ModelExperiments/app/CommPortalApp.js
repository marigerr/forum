var commPortalApp = angular.module('commPortalApp', ["ngRoute","ngAnimate", "ui.bootstrap"]);

commPortalApp.config(["$routeProvider", "$locationProvider",
    function($routeProvider, $locationProvider) {
    $routeProvider
        .when("/Home/Index", {
            templateUrl: "app/home/home.html",
            controller: "HomeController"
        })
        .when("/Forum/Forum", {
            templateUrl: "app/forum/forumList.html",
            controller: "ForumController"
        })
        .when("/ReadPostAndReplies/:PostId", {
            templateUrl: "app/forum/readPostPage.html",
            controller: "PostAndRepliesController"
        })
        .when("/PublicProfile/:UserName", {
            templateUrl: "app/publicUserProfile/publicProfilePage.html",
            controller: "PublicProfileController"
        })
        .when("/NewsFeed/NewsFeed", {
            templateUrl: "app/newsFeed/newsFeed.html",
            controller: "NewsFeedController"
        })
        .when("/ReadNewsItem/:ItemId", {
            templateUrl: "app/newsFeed/readNews.html",
            controller: "ReadNewsController"
        })
        .when("/Mail/Mail", {
            templateUrl: "app/mail/mail.html",
            controller: "MailController"
        })
        .when("/Admin/Admin", {
            templateUrl: "app/admin/adminIndex.html",
            controller: "AdminController"
        });
        //.otherwise({
        //    redirectTo: "",
        //    controller: "HomeController"
        //});
    $locationProvider.html5Mode(true);
}]);


commPortalApp.factory('JsonTime', [ function () {
    var convertFrom = function (objWithTimes) {
        myLength = objWithTimes.length;
        if (Object.prototype.toString.call(objWithTimes) === '[object Array]') {
            for (var i = 0; i < myLength; i++) {
                if (objWithTimes[i].TimeStamp) {
                    objWithTimes[i].TimeStamp = new Date(parseInt(objWithTimes[i].TimeStamp.replace('/Date(', '')));
                }
                if (objWithTimes[i].DateJoined) {
                    objWithTimes[i].DateJoined = new Date(parseInt(objWithTimes[i].DateJoined.replace('/Date(', '')));
                }
                if (objWithTimes[i].EventDateTime)
                {
                    objWithTimes[i].EventDateTime = new Date(parseInt(objWithTimes[i].EventDateTime.replace('/Date(', '')));
                }
            };
        } else {
            objWithTimes.TimeStamp = new Date(parseInt(objWithTimes.TimeStamp.replace('/Date(', '')));
            if (objWithTimes.EventDateTime)
            {
                objWithTimes.EventDateTime = new Date(parseInt(objWithTimes.EventDateTime.replace('/Date(', '')));
            }
        }
        return objWithTimes;
    };

    return {
        convertFrom: convertFrom
    };
}]);

commPortalApp.factory('Pagin', function () {
    return {
        vals : function() {
            return {
                currentPage : 1,
                itemsPerPage : 100,
                maxSize : 6
            };
        }
    }
});

commPortalApp.filter('pages', function () {
    return function (input, currentPage, pageSize) {
        if (angular.isArray(input)) {
            var start = (currentPage - 1) * pageSize;
            var end = currentPage * pageSize;
            return input.slice(start, end);
        }
    };
});


