var commPortalApp = angular.module('commPortalApp', ["ngRoute","ngAnimate", "ui.bootstrap"]);

commPortalApp.config(["$routeProvider", "$locationProvider",
    function($routeProvider, $locationProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "app/home/home.html",
            controller: "HomeController"
        })
        //.when("/Home/Home", {
        //  templateUrl: "app/home/home.html",
        //  controller: "HomeController"
        //})
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


commPortalApp.controller("MainController",
    ["$http", "$scope", "$location", '$interval',
    function ($http, $scope, $location, $interval) {
         $interval(function () {
            var onError = function (reason) {
                $scope.error = "There was an error";
            };
            var onListComplete = function(response) {
                $scope.hasNewMail = response.data;
                <!--$rootScope.$broadcast('emailStatus', hasNewMail);-->
            }
            $http.get('/Mail/CheckMail/')
                 .then(onListComplete, onError);
        }, 5000);

}]);

/* <!--commPortalApp.run(['CheckMail', function(CheckMail) {
        alert(CheckMail());
    }]); --> */


/* <!-- commPortalApp.factory('CheckMail', ['$rootScope','$interval', '$http', function ($rootScope, $interval, $http){
    
    var hasNewMail;
    function start(){
        $interval(function () {
            var onError = function (reason) {
                $rootScope.error = "There was an error";
            };
            var onListComplete = function(response) {
                hasNewMail = response.data;
                $rootScope.$broadcast('emailStatus', hasNewMail);
            }
            $http.get('/Mail/CheckMail/')
                 .then(onListComplete, onError);
        }, 2000);
      }
    

    return hasNewMail;
}]);   --> */

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
                itemsPerPage : 25,
                maxSize : 5
            };
        }
    }
});

commPortalApp.factory('HomePagin', function () {
    return {
        vals : function() {
            return {
                currentPage : 1,
                itemsPerPage : 5,
                maxSize : 3
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


