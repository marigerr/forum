commPortalApp.controller("ReadNewsController",
    ["$http", "$scope", "$location", "$routeParams", "JsonTime",
    function ($http, $scope, $location, $routeParams, JsonTime) {

        var onRepliesComplete = function (response) {
            $scope.NewsItem = response.data
            JsonTime.convertFrom($scope.NewsItem);

        };

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        $http.get('/NewsFeed/GetNewsItem/',
                    { params: { itemId: $routeParams.ItemId } })
             .then(onRepliesComplete, onError);


        $scope.backToNewsFeed = function () {
            $location.path('/NewsFeed/NewsFeed');
        };

    }]);