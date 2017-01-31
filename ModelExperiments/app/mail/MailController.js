commPortalApp.controller("MailController",
    ["$http", "$scope", "$location", "$routeParams", "JsonTime", "Pagin", "$uibModal", "$log",
    function ($http, $scope, $location, $routeParams, JsonTime, Pagin, $uibModal ,$log) {

        $scope.oneAtATime = true;

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        var onListComplete = function (response) {
            var data = response.data
            $scope.ForumList = data;
            $scope.totalItems = $scope.ForumList.length;
            JsonTime.convertFrom($scope.ForumList);
            for (var i = 0; i < $scope.ForumList.length; i++) {
                if ($scope.ForumList[i].IsRead == true) {
                    $scope.ForumList[i].textColor = "purple";
                }
                else {
                    $scope.ForumList[i].textColor = "blue";
                }
            }
        };

        $http.get('/Mail/ReadPrivateMessage/')
             .then(onListComplete, onError);

        $scope.showUser = function (UserName) {
            $location.path('/PublicProfile/' + UserName);
        };

        $scope.replyToPM = function (PostId) {
            $http.get('/Mail/MarkReadPM/', { params: { postId: PostId } })
            .then(onListComplete, onError);
        };

        $scope.markAsRead = function (PostId) {
            $http.get('/Mail/MarkReadPM/', { params: { postId: PostId } })
            .then(onListComplete, onError);
        };

        $scope.markAsUnRead = function (PostId) {
            $http.get('/Mail/MarkUnReadPM/', { params: { postId: PostId } })
            .then(onListComplete, onError);
        };

        $scope.deletePM = function (PostId) {
            $http.get('/Mail/DeletePM/', { params: { postId: PostId } })
            .then(onListComplete, onError);
        };

        $scope.pageVals = Pagin.vals();

        $scope.showForm = function (forumPost) {
            $scope.message = "Show Form Button Clicked";
            console.log($scope.message);

            var modalInstance = $uibModal.open({
                templateUrl: '../app/mail/modalPmForm.html',
                controller: ModalInstanceCtrl,
                scope: $scope,
                resolve: {
                    userForm: function () {
                        return $scope.userForm;
                    },
                    originalPost: function () {
                        return forumPost;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                $scope.selected = selectedItem;
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

    }]);

var ModalInstanceCtrl = function ($scope, $http, $uibModalInstance, userForm, originalPost, $log) {
    $scope.newPM = {}
    $scope.submitForm = function () {
        if ($scope.form.userForm.$valid) {
            console.log('user form is in scope');
            $uibModalInstance.close('closed');
        } else {
            console.log('userform is not in scope');
        }
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    var onPMComplete = function () {
        $scope.PmSent = "Your private message was sent successfully";
    };

    var onError = function (reason) {
        $scope.error = "There was an error";
    };

    $scope.sendPM = function (newPM) {
        newPM.PmToUserName = originalPost.UserName;
        newPM.Title = "RE: " + originalPost.Title;
        $http.post('/Forum/CreateOrUpdatePM/', newPM)
            .then(onPMComplete, onError);
        $uibModalInstance.close();
    }
};