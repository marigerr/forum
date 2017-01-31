commPortalApp.controller("ForumController",
    ["$http", "$scope", "$location", "JsonTime", "Pagin",
    function ($http, $scope, $location, JsonTime, Pagin) {

        $scope.newPost = {};
        $scope.formShowHide = {};

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        var onPostComplete = function (response) {
            $scope.newPost = response.data;
        };

        var onListComplete = function (response) {
            var data = response.data
            $scope.ForumList = data.ForumPosts;
            $scope.totalItems = $scope.ForumList.length;
            $scope.LoggedInUserStatus = data.LoggedInUserStatus;
            $scope.LoggedInUser = data.LoggedInUser
            JsonTime.convertFrom($scope.ForumList);
            $scope.newPost.Title = "";
            $scope.newPost.PostContent = "";
        };

        $http.get('/Forum/ForumList/')
            .then(onListComplete, onError);

        $scope.readPost = function (PostId) {
            $location.path('/ReadPostAndReplies/' + PostId);
        };

        $scope.showUser = function (UserName) {
            $location.path('/PublicProfile/' + UserName);
        };

        $scope.createOrUpdatePost = function (newPost) {
            $scope.createForm = false;
            $http.post('/Forum/CreateOrUpdatePost/', newPost)
                .then(onListComplete, onError);
        };

        $scope.deletePost = function (PostId) {
            $http.get('/Forum/DeletePost/', { params: { postId: PostId } })
            .then(onListComplete, onError);
        };

        $scope.editPost = function (PostId) {
            $scope.createForm = true
            $http.get('/Forum/GetForumPostToEdit/', { params: { postId: PostId } })
            .then(onPostComplete, onError);
        };

        $scope.closeCreateForm = function () {
            //$scope.newPost.PostId = 0;
            $scope.newPost.Title = "";
            $scope.newPost.PostContent = "";
            $scope.createForm = false;
            $scope.formShowHide.privateMess = false;
            //$scope.newPM.PostId = 0;
            //$scope.newPM.Title = "";
            //$scope.newPM.PostContent = "";
        };

        $scope.pageVals = Pagin.vals();

    }]);