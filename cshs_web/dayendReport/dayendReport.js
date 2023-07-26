(function () {
    angular.module("app").controller("dayendreport", [
        "abp.services.app.dayEndReport", "$rootScope", "$scope", "appSession", "$timeout",

        function (dayEndReportService, $rootScope, $scope, appSession, $timeout) {
            abp.ui.setBusy();

            var der = this;
            der.main = {};
            der.main.startdate = new Date();

            var initialLoadTrigger = false;
            var initialLoaded = 0;
            var reportResult = [];
            var currentreportType = "";



            $scope.$watch(function () {
                return $rootScope.defaultUserStation;
            },
                function () {
                    if ($rootScope.defaultUserStation != undefined) {
                        initialLoad();
                    }
                }, true);


            function initialLoad() {
                if (!initialLoadTrigger) {
                    initialLoadTrigger = true;
                }



                loadSetup();

            }


            $(document).ready(function () {
                if ($rootScope.defaultUserStation != undefined) {
                    initialLoad();
                }
            });

            function loadSetup() {
                abp.ui.clearBusy();
            }


            //The main search functions.
            der.mainsearch = function () {
                abp.ui.setBusy();
                der.reportResult = [];

                //Query search date
                var searchReport = document.getElementById("dtSearch").value;






                dayEndReportService.mainSearch(searchReport)

                    .then(function (result) {

                        if (result.data.item.length > 0) {
                            der.reportResultOut = result.data;
                        }
                        else {
                            abp.notify.error("There is no report within this date for outbound.", "Empty data");
                        }




                        abp.ui.clearBusy();
                    });



                dayEndReportService.reportGet(searchReport)

                    .then(function (result) {

                        if (result.data.item.length > 0) {
                            der.reportResultIn = result.data;
                        }
                        else {
                            abp.notify.error("There is no report within this date for inbound.", "Empty data");
                        }




                        abp.ui.clearBusy();
                    });

            }


            //Export the found result to a CSV.
            //Initiates a function that generates a CSV based on the available data.
            der.csvOutbound = function () {
                abp.ui.setBusy();

                var csvArray = [];
                var dataPushed = 0;

                while (dataPushed < der.reportResultOut.item.length) {
                    csvArray.push(der.reportResultOut.item[dataPushed]);

                    dataPushed++;
                }

                csvArray.unshift(der.reportResultOut.header);

                var csv = [];

                var totalDone = 0;

                while (totalDone < csvArray.length) {
                    csv.push(csvArray[totalDone].join(",").replace(/\n/g, " "));

                    totalDone++;
                }

                var filename = "DayEndReport_" + document.getElementById("dtSearch").value + ".csv";

                csvDownload(csv.join("\n"), filename);
            }

            function csvDownload(csv, filename) {
                var csvFile;
                var downloadLink;

                csvFile = new Blob([csv], { type: "text/csv" });

                downloadLink = document.createElement("a");

                downloadLink.download = filename;

                downloadLink.href = window.URL.createObjectURL(csvFile);

                downloadLink.style.display = "none";

                document.body.appendChild(downloadLink);

                downloadLink.click();

                abp.ui.clearBusy();
            };

            der.csvInbound = function () {
                abp.ui.setBusy();

                var csvArray = [];
                var dataPushed = 0;

                while (dataPushed < der.reportResultIn.item.length) {
                    csvArray.push(der.reportResultIn.item[dataPushed]);

                    dataPushed++;
                }

                csvArray.unshift(der.reportResultIn.header);

                var csv = [];

                var totalDone = 0;

                while (totalDone < csvArray.length) {
                    csv.push(csvArray[totalDone].join(",").replace(/\n/g, " "));

                    totalDone++;
                }

                var filename = "DayEndReport_" + document.getElementById("dtSearch").value + ".csv";

                csvDownload(csv.join("\n"), filename);
            }

            function csvDownload(csv, filename) {
                var csvFile;
                var downloadLink;

                csvFile = new Blob([csv], { type: "text/csv" });

                downloadLink = document.createElement("a");

                downloadLink.download = filename;

                downloadLink.href = window.URL.createObjectURL(csvFile);

                downloadLink.style.display = "none";

                document.body.appendChild(downloadLink);

                downloadLink.click();

                abp.ui.clearBusy();
            };

            der.openOut = function () {

                var url = "http://localhost:6235/#/searchCn";
                window.location.href = url;
                //window.open(url, '_blank', 'noreferrer');
              
            }

            der.openIn = function () {

                var url = "http://localhost:6235/#/summaryReport";
                window.location.href = url;
                //window.open(url, '_blank', 'noreferrer');

            }
            // JavaScript code to handle the 'Open Outbound' button click
            der.openOutbound = function () {
                var outboundModal = document.getElementById('outboundModal');
                outboundModal.style.display = 'block';
            }

            // JavaScript code to handle the 'Open Inbound' button click
            der.openInbound = function() {
                var inboundModal = document.getElementById('inboundModal');
                inboundModal.style.display = 'block';
            }

            // JavaScript code to close the 'Open Outbound' modal
            der.closeOutboundModal = function() {
                var outboundModal = document.getElementById('outboundModal');
                outboundModal.style.display = 'none';
            }

            // JavaScript code to close the 'Open Inbound' modal
            der.closeInboundModal = function() {
                var inboundModal = document.getElementById('inboundModal');
                inboundModal.style.display = 'none';
            }

        }
        




        

    ]);
})();