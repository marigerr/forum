commPortalApp.controller("NewsFeedController",
    ["$http", "$scope", "$location", "$routeParams", "JsonTime", "Pagin",
    function ($http, $scope, $location, $routeParams, JsonTime, Pagin) {


        $scope.newNews = {};
        $scope.newNews.EventDate = new Date();
        $scope.newNews.EventTime = new Date();
        $scope.popup = {
            opened: false
        };

        $scope.open= function () {
            $scope.popup.opened = true;
        };

        $scope.dateOptions = {
            maxDate: new Date(2020, 5, 22),
            minDate: new Date(),
            startingDay: 0
        };


        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        var onListComplete = function (response) {
            var data = response.data
            $scope.NewsItemList = data.NewsItems;
            $scope.totalItems = $scope.NewsItemList.length;
            JsonTime.convertFrom($scope.NewsItemList);
            $scope.IsAdminOrMod = data.IsAdminOrMod;
        };

        $http.get('/NewsFeed/NewsFeedIndex/')
            .then(onListComplete, onError);

        $scope.showHideNewsForm = function () {
            $scope.newNews = {};
            if ($scope.createEventForm) {
                $scope.createEventForm = false;
            };
            $scope.createNewsForm = !$scope.createNewsForm;
            $scope.newNews.IsEvent = false;
        };
        $scope.showHideEventForm = function () {
            $scope.newNews = {};
            if ($scope.createNewsForm) {
                $scope.createNewsForm = false;
            }
            $scope.createEventForm = !$scope.createEventForm;
            $scope.newNews.IsEvent = true;
        };
        
        $scope.createOrUpdateNewsItem = function (newNews) {
            newNews.JSEventDateTime = new Date(newNews.EventDate.getFullYear(), newNews.EventDate.getMonth(), newNews.EventDate.getDate(),
               newNews.EventTime.getHours(), newNews.EventTime.getMinutes(), newNews.EventTime.getSeconds());

            $scope.createNewsForm = false;
            $scope.createEventForm = false;
            $http.post('/NewsFeed/CreateOrUpdateNewsItem/', newNews)
                .then(onListComplete, onError);
        };

        $scope.editItem = function (newsItem) {
            if (newsItem.IsEvent) {
                $scope.newNews = newsItem;
                $scope.newNews.EventDate = new Date((newsItem.EventDateTime).getTime());
                $scope.newNews.EventTime = new Date((newsItem.EventDateTime).getTime());
                $scope.createEventForm = true;
            } else {
                $scope.createNewsForm = true;
                $scope.newNews = newsItem;
            }
        };

        $scope.readNews = function (ItemId) {
            $location.path('/ReadNewsItem/' + ItemId);
        };

        $scope.deleteItem = function (ItemId) {
            var dataToSend = { itemId: ItemId };
            $http.post('/NewsFeed/DeleteItem/', dataToSend)
                .then(onListComplete, onError);
        };


        $scope.pageVals = Pagin.vals();

    }]);