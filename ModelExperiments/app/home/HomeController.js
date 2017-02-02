commPortalApp.controller("HomeController",
    ["$http", "$scope", "$location", "JsonTime", "HomePagin",
    function ($http, $scope, $location, JsonTime, HomePagin) {

        $scope.dataLoaded = false;

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        var onNewsListComplete = function (response) {
            var data = response.data
            $scope.NewsItemList = data.NewsItems;
            $scope.totalNewsItems = $scope.NewsItemList.length;
            JsonTime.convertFrom($scope.NewsItemList);
            $scope.IsAdminOrMod = data.IsAdminOrMod;
            $scope.newsLoaded = true;
            checkDataLoaded();
        };  

        $http.get('/NewsFeed/NewsFeedIndex/')
            .then(onNewsListComplete, onError);

        var onListComplete = function (response) {
            var data = response.data
            $scope.ForumList = data.ForumPosts;
            $scope.totalItems = $scope.ForumList.length;
            $scope.LoggedInUserStatus = data.LoggedInUserStatus;
            $scope.LoggedInUser = data.LoggedInUser
            JsonTime.convertFrom($scope.ForumList);
            $scope.forumLoaded = true;
            checkDataLoaded();
        };

        $http.get('/Forum/ForumList/')  
            .then(onListComplete, onError);

        var onMailListComplete = function (response) {
            $scope.TotalUnread = 0;
            $scope.UnreadClass = "";
            var data = response.data
            $scope.MailList = data;
            $scope.totalMailItems = $scope.MailList.length;
            JsonTime.convertFrom($scope.MailList);
            for (var i = 0; i < $scope.MailList.length; i++) {
                if ($scope.MailList[i].IsRead == true) {
                    $scope.MailList[i].textColor = "purple";
                }
                else {
                    $scope.MailList[i].textColor = "blue";
                    $scope.TotalUnread ++; 
                }
            }
            if($scope.TotalUnread > 0)
                $scope.unreadMail = true;
            
            $scope.mailLoaded = true;
            checkDataLoaded();
        };

        $http.get('/Mail/ReadPrivateMessage/')
             .then(onMailListComplete, onError);

        $scope.NewsPageVals = HomePagin.vals();

        $scope.pageVals = HomePagin.vals();

        $scope.MailPageVals = HomePagin.vals();

        $scope.readPost = function (PostId) {
            $location.path('/ReadPostAndReplies/' + PostId);
        };    

        $scope.readNews = function (ItemId) {
            $location.path('/ReadNewsItem/' + ItemId);
        };  

        $scope.readMail = function() {
            $location.path('/Mail/Mail');
        }

        $scope.showUser = function (UserName) {
            $location.path('/PublicProfile/' + UserName);
        };

        function checkDataLoaded() {
            if ($scope.mailLoaded && $scope.forumLoaded && $scope.newsLoaded)
                $scope.dataLoaded = true;
        }                        
    }]);
