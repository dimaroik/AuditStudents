

    $(function () {

        $("#studyDate").jsGrid({
            height: "auto",
            width: "50%",

            filtering: true,
            inserting: true,
            sorting: true,
            paging: true,
            autoload: true,

            pageSize: 5,
            pageButtonCount: 5,


            controller: {
               
                loadData: function (filter) {
                    var d = $.Deferred();
                    return $.ajax({
                        type: "GET",
                        url: "/Admin/GetDataStudy",
                        data: filter,
                        dataType: "json"
                    });
                },

                insertItem: function (item) {
                    return $.ajax({
                        type: "POST",
                        url: "/Admin/AddStudyDate",
                        data: item,
                        dataType: "json"
                    });
                    
                }
               
            },
            fields: [
              
                { name: "Date", type: "text", width: 150 },
                { type: "control" }
               
            ]

        });

    });

    
