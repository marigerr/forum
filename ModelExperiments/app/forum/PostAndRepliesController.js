commPortalApp.controller("PostAndRepliesController",
    ["$http", "$scope", "$location", "$routeParams", "JsonTime",
    function ($http, $scope, $location, $routeParams, JsonTime) {

        $scope.newReply = {};
        $scope.formShowHide = {};
        $scope.formShowHide.replyForm = false;

        var onRepliesComplete = function (response) {
            var replyData = response.data
            $scope.forumPost = replyData.forumPost;
            $scope.ForumPostReplies = replyData.ForumPostReplies;
            $scope.LoggedInUser = replyData.LoggedInUser;
            $scope.LoggedInUserStatus = replyData.LoggedInUserStatus;
            JsonTime.convertFrom($scope.ForumPostReplies);
            $scope.newReply.PostContent = "";
            $scope.newReply.ParentPostId = "";
            //$scope.formShowHide.replyForm = false;
        };

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        $http.get('/Forum/ReadPostAndReplies/',
                    { params: { postId: $routeParams.PostId } })
             .then(onRepliesComplete, onError);


        $scope.quotePost = function() {
            var text = "";
            if (window.getSelection) {
                text = window.getSelection().toString();
            } else if (document.selection && document.selection.type != "Control") {
                text = document.selection.createRange().text;
            }
            alert(text);
            if (text!="") {
                $scope.newReply.PostContent = $scope.forumPost.User.UserName + " said "+ '"' + text + '"';
            }
            $scope.formShowHide.replyForm = true
        }

        $scope.closeCreateForm = function () {
            //$scope.newPost.PostId = 0;
            $scope.newReply.Title = "";
            $scope.newReply.PostContent = "";
            //$scope.createForm = false;
            $scope.formShowHide.replyForm = false;
            //$scope.newPM.PostId = 0;
            //$scope.newPM.Title = "";
            //$scope.newPM.PostContent = "";
        };

        $scope.replyPost = function (newReply) {
            newReply.ParentPostId = $scope.forumPost.PostId;
            newReply.Title = "RE: " + $scope.forumPost.Title;
            $scope.formShowHide.replyForm = false;
            $http.post('/Forum/ReplyPost/', newReply)
            .then(onRepliesComplete, onError);
        };

        $scope.readPost = function (PostId) {
            $location.path('/ReadPostAndReplies/' + PostId);
        };
        $scope.backToForum = function () {
            $location.path('/Forum/Forum');
        };

        $scope.showUser = function (UserName) {
            $location.path('/PublicProfile/' + UserName);
        };
}]);