'use strict';
let Application = function() {
    const APIS = {
        BASE: "/api",
        ACTIVITY: {
            BASE: "/activity",
            LIST: "/getactivitylist",
            DETAILS: "/getActivityDetails"
        }
    };
    let Events = function () {
        this.openModal = function(modalConfig, data) {
            $("#dialog").dialog({
                title: modalConfig.title,
                closeOnEscape: true
            });
            let htmlString = "";
            for (let item in data) {
                if (data.hasOwnProperty(item)) {
                    let dataItem = data[item];
                    htmlString += `<label class="control-label" for="${item}">${item}</label><input class="form-control" id="${item}" type="text" placeholder="" disabled="" value="${dataItem}">`
                }
            }            
            $("#dialog .dialog-content").html(htmlString);
        }
        
    }
    let events = new Events();
    let ColumnFilters = function () {
        this.dateTimeOffset = function(data){
            return moment(data).format("MM-DD-YYYY HH:mm:ss Z")
        }
        this.verified = function (data) {
            if (data) {
                return '<input type="checkbox" disabled="true" checked="checked" aria-label="Buld Checkbox for Verifying Activity">';
            }
            else {
                return '<input type="checkbox" aria-label="Buld Checkbox for Verifying Activity">';
            }
        }
        this.detailsBtn = function (data) {
            return `<button data-id="${data.Id}" type="button" class="btn btn-primary">Details</button>`;
        }
    };
    let columnFilters = new ColumnFilters();
    let VerifyDashboard = function () {
        let table = null, verifyDashboardTableSelector = `#verify-dashboard #varify-dashboard-table`;
        let columns = [
            {
                data: "Id",
                visible: false,
                title: "Activity ID",
                order: 0
            },
            {
                data: "Type",
                visible: false,
                title: "Activity Type",
                orderable: false,
                order: 1
            },
            {
                data: "Verified",
                visible: true,
                title: "Verified",
                order: 2,
                render: columnFilters.verified,
                orderable: false,
                className: "verified"
            },
            {
                data: "HandleName",
                visible: true,
                title: "Handle Name",
                orderable: false,
                order: 3
            },
            {
                data: "DateTime",
                visible: true,
                title: "Date & Time",
                order: 4,
                orderable: false,
                render: columnFilters.dateTimeOffset
            },
            {
                data: "Description",
                visible: true,
                title: "Description",
                orderable: false,
                order: 5
            },
            {
                visible: true,
                className: "details",
                data: null,
                targets: 0,
                orderable: false,
                order: 6,
                render: columnFilters.detailsBtn
            }
        ];
        let clickHandlers = () => {
            $(`${verifyDashboardTableSelector} .details`).unbind().on('click', function () {
                let data = table.row(this).data()
                $.ajax({
                    url: `${APIS.BASE}${APIS.ACTIVITY.BASE}${APIS.ACTIVITY.DETAILS}?activityId=${data.Id}&activityType=${data.Type}`,
                    type: "GET",
                }).success((data) => {
                    let modalConfig = {};
                    modalConfig.title = "Activity Details";
                    events.openModal(modalConfig, data);
                }).error((er) => {
                    console.error(er);
                });
            });
        }
        let initializeApp = () => {
            return new Promise((resolve, reject) => {
                table = $(`${verifyDashboardTableSelector}`).DataTable({
                    processing: true,
                    serverSide: true,
                    filter: false,
                    columns: columns,
                    bLengthChange: false,
                    ajax: {
                        url: "/home/LoadData",
                        type: "POST",
                        datatype: "json"
                    },
                    drawCallback: (data) => {
                        clickHandlers();
                        resolve();
                    },
                    initComplete: (data) => {
                        resolve();
                    }
                });
            });
        }
        initializeApp().then(() => { }).catch((err) => {
            console.error(err);
        });
    };
    new VerifyDashboard();

};
new Application();