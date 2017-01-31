commPortalApp.controller("PublicProfileController",
    ["$http", "$scope", "$location", "$routeParams", "JsonTime", "Pagin",
    function ($http, $scope, $location, $routeParams, JsonTime, Pagin) {

        $scope.newPM = {};
        $scope.formShowHide = {};

        var onListComplete = function (response) {
            var data = response.data
            $scope.ForumList = data.ForumPosts;
            $scope.totalItems = $scope.ForumList.length;
            $scope.LoggedInUser = data.LoggedInUser;
            $scope.ProfileUserName = data.ProfileUser;
            $scope.DateJoined = data.ProfileUserDateJoined;
            $scope.DateJoined = new Date(parseInt($scope.DateJoined.replace('/Date(', '')));
            JsonTime.convertFrom($scope.ForumList);
        };

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        $http.get('/Forum/GetUserPosts/',
                { params: { userName: $routeParams.UserName } })
             .then(onListComplete, onError);

        $scope.backToForum = function () {
            $location.path('/Forum/Forum');
        };

        $scope.formShow = function() {
            $scope.newPM.Title = "";
            $scope.newPM.PostContent = "";
            $scope.formShowHide.privateMess = true;
        }

        $scope.readPost = function (PostId) {
            $location.path('/ReadPostAndReplies/' + PostId);
        };

        var onPMComplete = function () {
            $scope.PmSent = "Your private message was sent successfully";
        };
        
        $scope.createOrUpdatePM = function (newPM) {
            newPM.PmToUserName = $scope.ProfileUserName;
            $scope.formShowHide.privateMess = false;

            $http.post('/Forum/CreateOrUpdatePM/', newPM)
                .then(onPMComplete, onError);
        };

        //$scope.deletePost = function (PostId) {
        //    $http.get('/Forum/DeletePost/', { params: { postId: PostId } })
        //    .then(onListComplete, onError);
        //};

        //$scope.editPost = function (PostId) {
        //    $scope.createForm = true
        //    $http.get('/Forum/GetForumPostToEdit/', { params: { postId: PostId } })
        //    .then(onPostComplete, onError);
        //};
    
        
        $scope.pageVals = Pagin.vals();

    }]);

