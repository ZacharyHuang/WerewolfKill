var App = angular.module("App", ['ui.router']);

App.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
    $locationProvider.hashPrefix('');

    $urlRouterProvider.otherwise('/')

    $stateProvider.state('home', {
        url: '/',
        templateUrl: '/Home/Home'
    });
    $stateProvider.state('create', {
        url: '/create',
        templateUrl: '/Home/Create'
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

App.controller('CreateController', function ($scope, $rootScope, $state, $http) {
    $rootScope.GlobalData.Title = "创建房间";
    $scope.data = {};
    $scope.data.config = {
        PlayerNumber: 12,
        VillageNumber: 4,
        WerewolfNumber: 4,
        Prophet: true,
        Witch: true,
        Hunter: true,
        Guard: true,
        Idiot: false,
        Demon: false,
        KillAll: 0,
        WitchHealSelf: 1,
        WitchTwoShots: 0,
        HealAndGuardIsDead: 1
    }
    $scope.data.configText = {
        KillAll: ["屠边", "屠城"],
        WitchHealSelf: ["不可自救", "仅首夜", "可以自救"],
        WitchTwoShots: ["不可同时用药", "可以同时用药"],
        HealAndGuardIsDead: ["会死", "不会死"]

    };
    $scope.submit = function () {
        $http({
            method: 'POST',
            url: '/api/Room/Create',
            data: $scope.data.config
        }).then(function (response) {
            console.log(response);
            //if (response.status == 200) {

            //}
        }, function (response) {
            console.log("error");
            console.log(response);
        });
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
    $scope.data.players = [
        { NickName: null, AvatarUrl: null },
        { NickName: "BOT1", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT2", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT3", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT4", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT5", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT6", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT7", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT8", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT9", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT10", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT11", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT12", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" },
        { NickName: "BOT13", AvatarUrl: "https://zacharyhuangstoraqge.blob.core.windows.net/werewolfkill/icon/6a7387b7479dc45048004e22925aec58.png" }
    ];
    $scope.data.config = {
        PlayerNumber: 12,
        VillageNumber: 4,
        WerewolfNumber: 4,
        HasProphet: true,
        HasWitch: true,
        HasHunter: true,
        HasGuard: true,
        HasIdiot: false,
        HasCupid: false,
        HasDemon: false,
        HasWhiteWerewolf: false,
        HasThief: false,
        CampKill: true,
        WitchHealSelf: 1,
        WitchTwoShots: false,
        HealAndGuardIsDead: true,
        ThiefGaranteed: false,
        TwoWerewolfForThief: false,
        ThiefMustChooseWerewolf: true
    }
    $state.go('room.wait', { roomId: $scope.data.roomId });

    $scope.readyToStart = function () {
        return $scope.data.players.filter(p => p.NickName == null).length <= 1;
    };


    $scope.start = function () {
        console.log("start");
        $state.go('room.game', { roomId: $scope.data.roomId });
    };

    $scope.kill = function () {
        console.log("kill");
    };

});