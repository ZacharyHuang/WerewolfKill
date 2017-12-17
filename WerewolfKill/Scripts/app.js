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

App.controller('RoomController', function ($scope, $rootScope, $stateParams, $state) {
    $scope.data = {};
    $scope.data.roomId = $stateParams.roomId;
});