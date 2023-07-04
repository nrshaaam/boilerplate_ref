(function () {
    angular.module('app').controller('motherbagdetailList', [
        'abp.services.app.motherbagDetail', 'NgTableParams', '$rootScope', '$scope',

        function (motherbagdetailService, NgTableParams, $rootScope, $scope) {
            abp.ui.setBusy();

            var md = this;

            md.main = {};

            md.main.startdate = new Date();
            md.main.enddate = new Date();

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

            //Extracts needed values for dropdowns from the database.
            function getDest() {
                motherbagdetailService.destGet()
                    .then(function (result) {
                        md.destinationList = result.data;

                        initialLoaded++;

                        if (initialLoaded == 3) {
                            loadSetup();
                        }
                    });
            }

            //Extracts needed values for dropdowns from the database.
            function getOrigin() {
                motherbagdetailService.originGet()
                    .then(function (result) {
                        md.originList = result.data;

                        initialLoaded++;

                        if (initialLoaded == 3) {
                            loadSetup();
                        }
                    });
            }

            function getColoader() {
                var currentBranch = $rootScope.defaultUserStation;

                motherbagdetailService.coloaderGet(currentBranch)
                    .then(function (result) {
                        md.coloaderList = result.data;

                        initialLoaded++;

                        if (initialLoaded == 3) {
                            loadSetup();
                        }
                    });
            }

            function getTranstype() {
                motherbagdetailService.transtypeGet()
                    .then(function (result) {
                        md.transtypeList = result.data;

                        initialLoaded++;

                        if (initialLoaded == 3) {
                            loadSetup();
                        }
                    });
            }

            function initialLoad() {
                if (!initialLoadTrigger) {
                    initialLoadTrigger = true;
                    getDest();
                    getOrigin();
                    getColoader();
                    getTranstype();
                }
            }

            function loadSetup() {
                md.isEasthub = $rootScope.defaultUserStation == "HKI" || $rootScope.defaultUserStation == "HKC";

                abp.ui.clearBusy();
            }

            $(document).ready(function () {
                if ($rootScope.defaultUserStation != undefined) {
                    initialLoad();
                }
            });

            //The main search functions.
            md.mainsearch = function () {
                abp.ui.setBusy();

                var startDate = document.getElementById("dtStartdate").value;
                var endDate = document.getElementById("dtEnddate").value;

                if (startDate == "" || endDate == "") {
                    abp.notify.warn("Please select start and end dates", "");

                    abp.ui.clearBusy();
                }

                else {
                    var reportType = currentreportType = document.getElementById("slcReportType").value;
                    var bagNo = document.getElementById("txtBagno").value.trim();
                    var destination = document.getElementById("slcDest").value;
                    var batchNo = document.getElementById("txtBatchno").value.trim();
                    var staffId = document.getElementById("txtStaffid").value.trim();
                    var coloader = document.getElementById("slcColoader").value;
                    var runningNo = document.getElementById("txtRunningno").value.trim();
                    var lorryplateNo = document.getElementById("txtLorryplateno").value.trim();
                    var transType = document.getElementById("slcTranstype").value;
                    var flightType = document.getElementById("slcFlighttype").value;
                    var flightNo = document.getElementById("txtFlightno").value.trim();
                    var currentBranch = document.getElementById("slcOrigin").value.trim();

                    if (bagNo == "") {
                        bagNo = "n0n3";
                    }

                    if (batchNo == "") {
                        batchNo = "n0n3";
                    }

                    if (staffId == "") {
                        staffId = "n0n3";
                    }

                    if (runningNo == "") {
                        runningNo = "n0n3";
                    }

                    if (lorryplateNo == "") {
                        lorryplateNo = "n0n3";
                    }

                    if (flightNo == "") {
                        flightNo = "n0n3";
                    }

                    motherbagdetailService.mainSearch(reportType, startDate, endDate, bagNo, origin, destination, batchNo, staffId, coloader, runningNo, lorryplateNo, transType, flightType, flightNo, currentBranch)
                        .then(function (result) {
                            reportResult = result.data;

                            md.tableParams = new NgTableParams({ count: result.data.length }, { dataset: result.data, counts: [] });

                            abp.ui.clearBusy();
                        });
                }
            }

            //Export the found result to a CSV.
            md.csv = function () {
                abp.ui.setBusy();

                if (reportResult.length == 0) {
                    abp.notify.warn("No data to Export to CSV", "");

                    abp.ui.clearBusy();
                }

                else {
                    var csvArray = [];

                    arrayHeader = [
                        "No",
                        "Bag No",
                        "Bag Date",
                        "Co-loader",
                        "Origin",
                        "Destination",
                        "Bag Condition",
                        "Weight",
                        "Staff ID",
                        "Vehicle No",
                        "Running No",
                        "Batch No",
                        "Remarks"
                    ];

                    csvArray.push(arrayHeader);

                    var totalPushed = 0;

                    while (totalPushed < reportResult.length) {
                        var tempArray = [
                            reportResult[totalPushed].no,
                            reportResult[totalPushed].manifestNo,
                            reportResult[totalPushed].manifestDate,
                            reportResult[totalPushed].coloader,
                            reportResult[totalPushed].origin,
                            reportResult[totalPushed].destination,
                            reportResult[totalPushed].bagCondition,
                            reportResult[totalPushed].weight,
                            reportResult[totalPushed].staffId,
                            reportResult[totalPushed].vehicleNo,
                            reportResult[totalPushed].runningNo,
                            reportResult[totalPushed].batchNo,
                            reportResult[totalPushed].remark
                        ];

                        csvArray.push(tempArray);

                        totalPushed++;
                    }

                    var csv = [];

                    var totalDone = 0;

                    while (totalDone < csvArray.length) {
                        csv.push(csvArray[totalDone].join(",").replace(/\n/g, " "));

                        totalDone++;
                    }

                    var filename = "MotherbagDetails(" + currentreportType + ").csv";

                    csvDownload(csv.join("\n"), filename);
                }
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

            //Resets the form to it's initial state.
            md.reset = function () {
                document.getElementById("txtBagno").value = "";
                document.getElementById("txtBatchno").value = "";
                document.getElementById("txtStaffid").value = "";
                document.getElementById("txtRunningno").value = "";
                document.getElementById("txtLorryplateno").value = "";
                document.getElementById("txtFlightno").value = "";
                document.getElementById("slcFlighttype").value = "n0n3";
                document.getElementById("slcTranstype").value = "n0n3";
                document.getElementById("slcColoader").value = "n0n3";
                document.getElementById("slcDest").value = "n0n3";
            }

            //Changes the available filters displayed.
            document.getElementById("slcReportType").onchange = function () {
                document.getElementById("txtRunningno").value = "";
                document.getElementById("txtLorryplateno").value = "";
                document.getElementById("txtFlightno").value = "";
                document.getElementById("slcFlighttype").value = "n0n3";
                document.getElementById("slcTranstype").value = "n0n3";
                document.getElementById("slcColoader").value = "n0n3";

                if (document.getElementById("slcReportType").value == "G") {
                    document.getElementById("divMotherbagoutboundfilter").style.display = "none";
                    document.getElementById("divMotherbaginboundfilter").style.display = "block";
                }

                else {
                    document.getElementById("divMotherbaginboundfilter").style.display = "none";
                    document.getElementById("divMotherbagoutboundfilter").style.display = "block";
                }
            }
        }
    ]);
})();