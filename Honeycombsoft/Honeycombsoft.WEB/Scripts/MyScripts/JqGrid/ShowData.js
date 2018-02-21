
    $(function () {

        $("#jqGridPager").jsGrid({
            height: "auto",
            width: "100%",

            filtering: true,
            sorting: true,
         
            paging: true,
            autoload: true,
            

            pageSize: 10,
            pageButtonCount: 10,

       
            controller: {
                loadData: function (filter) {
                    return $.ajax({
                        type: "POST",
                        url: "/Admin/GetData",
                        data: filter,
                        dataType: "json"
                    });
                }
               

              
            },
            fields: [
                { name: "Name", type: "text", width: 150 },
                { name: "LastName", type: "text", width: 50 },
                { name: "Age", type: "number", width: 50 },
                { name: "Email", type: "text", width: 150},
                { name: "RegisteredDate", type: "text", width: 150},
                { name: "StudyDate", type: "text", width: 150 }
              
                 
            ]
            
        });

    });
