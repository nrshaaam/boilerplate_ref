(function () { 
    angular.module("app").controller("summaryreport", [
        "abp.services.app.summaryReport", "$rootScope", "$scope", "appSession", "$timeout",

        function (summaryReportService, $rootScope, $scope, appSession, $timeout) {
            abp.ui.setBusy();

            var sr = this;

            sr.main = {};

            sr.main.startdate = new Date();
            sr.main.enddate = new Date();

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
                document.getElementById("dtStartdate").disabled = true;
                document.getElementById("dtEnddate").disabled = true;

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
            

   /*         For calendar (display or not when report been choosen)*/
                document.querySelector('#slcReportType').addEventListener('change', (event) => {
                    var reporttype = document.getElementById("slcReportType").value;
                    var dtStartdate = document.getElementById("dtStartdate");
                    var dtEnddate = document.getElementById("dtEnddate");


                    if (reporttype == "0") {
                        dtStartdate.disabled = true;
                        dtEnddate.disabled = true;
                    }
                    else if (reporttype == "AGSS Orders Status") {
                        dtStartdate.disabled = false;
                        dtEnddate.disabled = true;
                    }

                    else {
                        dtStartdate.disabled = false;
                        dtEnddate.disabled = false;
                            
                    }

                });

                //The main search functions.
            sr.mainsearch = function () {
                abp.ui.setBusy();
                sr.reportResult = [];


                //Query search date
                var reportType = currentreportType = document.getElementById("slcReportType").value;
                var dateStart = document.getElementById("dtStartdate").value;
                var dateEnd = document.getElementById("dtEnddate").value;
                

                summaryReportService.mainSearch(reportType, dateStart, dateEnd)


                    .then(function (result) {

                        if (result.data.item.length > 0) {
                            sr.reportResult = result.data;
                        }
                        else {
                            abp.notify.error("There is no report within this date.", "Empty data");
                        }
                        



                        abp.ui.clearBusy();
                    });



            }

                //Export the found result to a CSV.
            //Initiates a function that generates a CSV based on the available data.
            sr.csv = function () {
                abp.ui.setBusy();

                var csvArray = [];
                var dataPushed = 0;

                while (dataPushed < sr.reportResult.item.length) {
                    csvArray.push(sr.reportResult.item[dataPushed]);

                    dataPushed++;
                }

                csvArray.unshift(sr.reportResult.header);

                var csv = [];

                var totalDone = 0;

                while (totalDone < csvArray.length) {
                    csv.push(csvArray[totalDone].join(",").replace(/\n/g, " "));

                    totalDone++;
                }

                var filename = "Report " + document.getElementById("dtStartdate").value + document.getElementById("slcReportType").value + ".csv";

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



            }
        
    ]);
})();