var categroryAppService;
var dataTable;
var l;
var getFilter;


$(function () {
    l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Categories/CreateModal',
        //scriptUrl : '/Pages/ckeditor.js'
    });
    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Categories/EditModal',
        //scriptUrl: '/Pages/ckeditor.js'
    });
    var detailModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Categories/DetailModal',
        //scriptUrl: '/Pages/ckeditor.js'
    });
    var deleteModal = new abp.ModalManager(abp.appPath + 'Categories/EditModal');
    var listBook = new abp.ModalManager(abp.appPath + 'Categories/ListBook');

    getFilter = function () {
        return {
            filterText: $("input[name='Search']").val(),
        };
    };

    categroryAppService = acme.bookStore.categories.category;

     dataTable = $('#CategoriesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(acme.bookStore.categories.category.getList, getFilter),
            columnDefs: [
                {
                    title: l('STT'),
                    data: "stt"
                },
                {
                    title: l('Name'),
                    data: "name"
                },
                
                {
                    title: l('CategoryParent'),
                    data: "categoryParent",

                },
                {
                    title: l('Image'),
                    data: { image: "image", id: "id"},
                    render: function (data) {
                        var img = `<img src="/ImageCategories/${data.image}" name="${data.id}" width="100px" height="100px" onerror="this.onerror=null;this.src='/ImageCategories/imageDefault.jpg'"/>`;
                        return img;
                    }
                },
                
                {
                    title: l('CountBook'),
                    data: "countBook",
                },
                {
                    title: l('Status'),
                    data: { status: "status", id:"id"} ,
                    render: function (data) {
                        
                        var check = '';
                        if (data.status == 1)
                            check = "checked";
                        var str = '<label class="switch">' +
                            `<input type = "checkbox" id="${data.id}" ${check} onclick="ChangeStatus(this.id,${data.status})">` +
                                '<span class="slider round"></span>' +
                            '</label >';
                        return str;

                    }
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Detail'),
                                    action: function (data) {                                        
                                        dataTable.ajax.reload();
                                        window.location = "https://localhost:44338/Categories/DetailModal?Id=" + data.record.id;
                                        //detailModal.open({ id: data.record.id });                                      

                                       
                                        //location.reload();
                                    }
                                },
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                        window.location = "https://localhost:44338/Categories/EditModal?Id=" + data.record.id;
                                        //dataTable.ajax.reload();
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('Delete', data.record.name);
                                    },
                                    action: function (data) {
                                        acme.bookStore.categories.category
                                            .delete(data.record.id)
                                            .then(function (data) {
                                                if (data) {
                                                    abp.notify.info(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                }
                                                
                                                else{
                                                    abp.message.error(l("NotifyDeleteAuthor"));
                                                }
                                            });
                                    }
                                },
                                {
                                    text: l('Books'),
                                    action: function (data) {
                                        listBook.open({ id: data.record.id });
                                        dataTable.ajax.reload();
                                    }
                                }
                            ]
                    }
                }
            ]
        })
    );

    

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewCategoryButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $('#StatusButton').click(function (e) {
        acme.bookStore.categories.category.changStatus(data.record.id);
    });

    $("input[name='Search'").keyup(function () {
        dataTable.ajax.reload();
        console.log(getFilter);
    });
    
});

function ChangeStatus(id, status) {
    debugger;
    if ($('#' + id).is(':checked')) {
        $("#" + id).prop("checked", false);
    }
    else {
        $("#" + id).prop("checked", true);
    }
    dataTable.ajax.reload();

    var mess = l('BlockTheCategory');
    if (status == 0) {
        mess = l('UnblockTheCategory');
    }

    abp.message.confirm(mess, l('Notify'))
        .then(function (confirmed) {

            if (confirmed) {
                acme.bookStore.categories.category.changStatus(id)
                abp.message.success(l('YourChangesHaveBeenSuccessfullySaved'), l('Congratulations'));
                dataTable.ajax.reload();
            }


        });
};

