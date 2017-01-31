commPortalApp.controller("AdminController",
    ["$http", "$scope", "$location", "$routeParams", "JsonTime", "Pagin",
    function ($http, $scope, $location, $routeParams, JsonTime, Pagin) {

        var onError = function (reason) {
            $scope.error = "There was an error";
        };

        Array.prototype.diff = function (a) {
            return this.filter(function (i) { return a.indexOf(i) < 0; });
        };

        var onListComplete = function (response) {
            var data = response.data
            $scope.currentList = data;
            JsonTime.convertFrom($scope.currentList);
            $scope.totalItems = $scope.currentList.length;
            var rolesArray = ["Moderator", "Admin", "User"];
            for (var i = $scope.currentList.length - 1; i >=0; i--) {
                $scope.currentList[i].availRoles = rolesArray.diff($scope.currentList[i].RoleList);
                for (var j = $scope.currentList[i].RoleList.length - 1; j >= 0; j--) {
                    if ($scope.currentList[i].RoleList[j] === "User") {
                        $scope.currentList[i].RoleList.splice(j, 1);
                    }
                }
            }
        };

        $http.get('/Admin/GetList/', { params: { roleString: "User" } })
                .then(onListComplete, onError);
        $scope.listName = "User";


        $scope.showList = function (RoleString)
        {
            $http.get('/Admin/GetList/', { params: { roleString: RoleString } })
                .then(onListComplete, onError);
            $scope.listName = RoleString;
        };

        $scope.showDeactivated = function () {
            $http.get('/Admin/GetDeactivated/')
                .then(onListComplete, onError);
            $scope.listName = "Deactivated";
        };

        $scope.removeRole = function (userName, role) {
            var dataToSend = { UserName: userName, RoleToRemove: role };
            $http.post('Admin/RemoveRoleFromUser', dataToSend)
                .then(onListComplete, onError);
        };

        $scope.addRole = function (userName, role) {
            var dataToSend = { UserName: userName, RoleToSet: role };
            $http.post('Admin/AddRoleToUser', dataToSend)
                .then(onListComplete, onError);
        };

        $scope.deactivateUser = function (userName) {
            var dataToSend = { UserName: userName};
            $http.post('Admin/DeactivateUser', dataToSend)
                .then(onListComplete, onError);
            //$http.post('Admin/DeleteUser', { params: { UserName: userName } })
        };

        $scope.activateUser = function (userName) {
            var dataToSend = { UserName: userName };
            $http.post('Admin/ActivateUser', dataToSend)
                .then(onListComplete, onError);
            //$http.post('Admin/DeleteUser', { params: { UserName: userName } })
        };

        $scope.pageVals = Pagin.vals();

    }]);