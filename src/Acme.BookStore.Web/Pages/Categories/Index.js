var categroryAppService;
var dataTable;
$(function () {
    var l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager(abp.appPath + 'Categories/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Categories/EditModal');
    var deleteModal = new abp.ModalManager(abp.appPath + 'Categories/EditModal');
    categroryAppService = acme.bookStore.categories.category;

     dataTable = $('#CategoriesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(acme.bookStore.categories.category.getList),
            columnDefs: [

                {
                    title: l('Name'),
                    data: "name"
                },
                //{
                //    title: l('Parent'),
                //    data: "idParen",

                //},
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
                    title: l('Describe'),
                    data: "describe",
                },
                {
                    title: l('CountBook'),
                    data: "countBook",
                },
                {
                    title: l('Status'),
                    data: { status: "status", id:"id"} ,
                    render: function (data) {
                        //var stt = "Hide";
                        //if (data.status) {
                        //    stt = "Visibility"
                        //}
                        ////var btn = `<button class="btn btn-primary" id="${data.id}" type="Button" onclick="Change(this.id)">${stt}</button>`;
                        //var btn = `<a href="javascript:void(0)" class="btn btn-primary" id="${data.id}" onclick="ChangeStatus(this.id)">${stt}</a>`;
                        //return btn;
                        var check = '';
                        if (data.status == 1)
                            check = "checked";
                        var str = '<label class="switch">' +
                            `<input type = "checkbox" id="${data.id}" ${check} onclick="ChangeStatus(this.id)">` +
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
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('CategoryDeletionConfirmationMessage', data.record.name);
                                    },
                                    action: function (data) {
                                        acme.bookStore.categories.category
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
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
    
});

function ChangeStatus(id) {
    debugger;
    categroryAppService.changStatus(id);
    dataTable.ajax.reload();
};

