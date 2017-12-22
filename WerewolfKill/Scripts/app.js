var App = angular.module("App", ['ui.router']);

App.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
    $locationProvider.hashPrefix('');

    $urlRouterProvider.otherwise('/')

    $stateProvider.state('home', {
        url: '/',
        templateUrl: '/Home/Home'
    });
    $stateProvider.state('join', {
        url: '/join',
        templateUrl: '/Home/Join'
    });

    $stateProvider.state('setting', {
        url: '/setting',
        templateUrl: '/Manage/Index'
    });
    $stateProvider.state('setting.password', {
        url: '/setting/password',
        templateUrl: '/Manage/ChangePassword'
    });
    $stateProvider.state('setting.icon', {
        url: '/setting/icon',
        templateUrl: '/Manage/ChangeIcon'
    });
    $urlRouterProvider.when('/setting', '/setting/icon');

    $stateProvider.state('room', {
        url: '/room/{roomId}',
        templateUrl: '/Room/Index'
    });
    $stateProvider.state('room.wait', {
        url: '/wait',
        templateUrl: '/Room/Wait'
    });
    $stateProvider.state('room.game', {
        url: '/game',
        templateUrl: '/Room/Game'
    });
    $urlRouterProvider.when('/room/{roomId}/', '/room/{roomId}/wait');
});

App.run(function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    $rootScope.GlobalData = {};
    $rootScope.GlobalData.UserInfo = {
        UserName: "Zachary.H.",
        AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png"
    };
    $rootScope.GlobalData.Title = null;
});

App.controller('HomeController', function ($scope, $rootScope, $state) {
    $rootScope.GlobalData.Title = "首页";
    $scope.GlobalData = $rootScope.GlobalData;

    $scope.CreateRoom = function () {
        $state.go('room', { roomId: "111111" });
    };
});

App.controller('JoinController', function ($scope, $rootScope, $stateParams, $state) {
    $rootScope.GlobalData.Title = "加入房间";
    $scope.InputId = null;

    $scope.InputChange = function () {
        if ($scope.InputId) {
            $scope.InputId = $scope.InputId.replace(/[^0-9\.]+/g, '');
            if ($scope.InputId.length >= 6) {
                var roomId = $scope.InputId;
                $scope.InputId = null;
                $state.go('room', { roomId: roomId });
            }
        }
    };
});

App.controller('RoomController', function ($scope, $rootScope, $stateParams, $state) {
    $rootScope.GlobalData.Title = "房间号：" + $stateParams.roomId;
    $scope.GlobalData = $rootScope.GlobalData;
    $scope.data = {};
    $scope.data.roomId = $stateParams.roomId;
    $state.go('room.wait', { roomId: $scope.data.roomId });
});