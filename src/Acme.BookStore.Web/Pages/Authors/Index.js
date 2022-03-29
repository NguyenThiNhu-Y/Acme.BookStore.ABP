var dataTable;
var l;
var getFilter;
$(function () {
    l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager(abp.appPath + 'Authors/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Authors/EditModal');
    var listBook = new abp.ModalManager(abp.appPath + 'Authors/ListBook');

    //get the value of the search input
    getFilter = function () {
        return {
            filterText: $("input[name='Search']").val(),
        };
    };

    dataTable = $('#AuthorsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            
            ajax: abp.libs.datatables.createAjax(acme.bookStore.authors.author.getList, getFilter),
            columnDefs: [
                {
                    title: l('Name'),
                    data: "name"
                },
                {
                    title: l('DoB'),
                    data: 'doB',
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString();
                    }
                },
                {
                    title: l('ShortBio'),
                    data: 'shortBio'
                    

                },
                {
                    title: l('Status'),
                    data: { status: "status", id: "id" },
                    
                    render: function (data) {
                        var check = '';
                        if (data.status == 1)
                            check = "checked";
                        var str = '<label class="switch" >' +
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
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('AuthorDeletionConfirmationMessage', data.record.name);
                                    },
                                    action: function (data) {
                                        acme.bookStore.authors.author
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                },
                                {
                                    text: l('Books'),
                                    action: function (data) {
                                        debugger;
                                        listBook.open({ id: data.record.id });
                                        
                                    }
                                }
                            ]
                    }
                },
               
            ]
        })
    )

    
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewAuthor').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $("input[name='Search'").change(function () {
        dataTable.ajax.reload();
        console.log(getFilter);
    });

    
});
function ChangeStatus(id, status) {
    var mess = 'Are you sure to block the author?';
    if (status == 0) {
        mess = 'Are you sure to unblock the author?';
    }
    abp.message.confirm(mess)
        .then(function (confirmed) {
            if (confirmed) {
                acme.bookStore.authors.author.changeStatus(id)
                abp.message.success('Your changes have been successfully saved!', 'Congratulations');
                dataTable.ajax.reload();
            }
            else {
                if ($('#' + id).is(':checked')) {
                    $("#" + id).prop("checked", false);
                }
                else {
                    $("#" + id).prop("checked", true);
                }
            }
            
        });
};

function ListBookOfAuthor(id) {
    var dataTableBooks = $('#BookOfAuthorsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(acme.bookStore.books.book.getListByIdAuthor(id)),
            columnDefs: [
                {
                    title: l('Name'),
                    data: "name"
                },
                {
                    title: l('CategoryParent'),
                    data: "categoryParent",

                },
                {
                    title: l('Author'),
                    data: "author",

                },
                {
                    title: l('PublishDate'),
                    data: "publishDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString();
                    }
                },
                {
                    title: l('Price'),
                    data: "price"
                },
                {
                    title: l('CreationTime'), data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('BookStore.Books.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id }).then(function () {
                                            abp.notify.info(l('Successfully'));
                                            dataTable.ajax.reload();
                                        });;
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('BookStore.Books.Delete'),
                                    confirmMessage: function (data) {
                                        return l('BookDeletionConfirmationMessage', data.record.name);
                                    },
                                    action: function (data) {
                                        acme.bookStore.books.book
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
}

function Search() {
    var strSearch = document.getElementById("searchString").value
    console.log(strSearch);
};

//function FormatCurrency(money) {
//    return money.toLocaleString('vi-VN', {
//        style: 'currency',
//        currency: 'VND'
//    }
//};