(function () {
    angular.module("app").controller("searchcn", [
        "abp.services.app.searchCn", "$rootScope", "$scope", "appSession", "$timeout",

        function (searchCnService, $rootScope, $scope, appSession, $timeout) {
            abp.ui.setBusy();

            var mr = this;
            mr.reportdataList = [];

            $scope.$watch(function () {
                return $rootScope.defaultUserStation;
            },
                function () {
                    if ($rootScope.defaultUserStation != undefined && initialNeeded) {
                        initialNeeded = false;

                        initialLoad();
                    }
                }, true);

            var initialNeeded = true;




            function initialLoad() {
                loadSetup();
            }

            function loadSetup() {
                abp.ui.clearBusy();
            }

            //Triggers the page load functions when everything has been loaded.
            $(document).ready(function () {
                if ($rootScope.defaultUserStation != undefined && initialNeeded) {
                    initialNeeded = false;

                    initialLoad();
                }
            });

            
            

            //Main search based on the selected report type.
            mr.mainSearch = function (){
                abp.ui.setBusy();

                var searchCN = document.getElementById("tbcn").value;


                searchCnService.reportdetailGet(searchCN)
                    .then(function (result) {
                    mr.reportdataList = result.data;

                    abp.ui.clearBusy();
   
                });
              
                       
            }



        }
    ]);
})();