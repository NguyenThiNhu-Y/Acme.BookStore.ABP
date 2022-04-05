var dataTable;
var l;
var getFilter;
var i = 1;
$(function () {
    l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Authors/CreateModal',
        //scriptUrl: '/Pages/Authors/Create.js'
    });
    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Authors/EditModal',
        //scriptUrl: '/Pages/Authors/Edit.js'
    });
    var listBook = new abp.ModalManager(abp.appPath + 'Authors/ListBook');
    var detailModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Authors/DetailModal',
        //scriptUrl: '/Pages/Authors/Edit.js'
    })
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
            order: ["1","asc"],
            searching: false,
            scrollX: true,
            
            ajax: abp.libs.datatables.createAjax(acme.bookStore.authors.author.getList, getFilter),
            columnDefs: [
                {
                    title: l('STT'),
                    data: "stt"
                },
                {
                    title: l('Name'),
                    data: "name",
                    orderable: true,
                },
                {
                    title: l('DoB'),
                    data: 'doB',
                    orderable: true,
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toFormat('dd/MM/yyyy');
                    },
                    className: "text-center",
                },
                //{
                //    title: l('ShortBio'),
                //    data: 'shortBio',
                //    className: "text-center"
                    

                //},
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
                    },
                    className: "text-center",
                },
                {
                    title: l('Actions'),
                    className: "text-center",
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Detail'),
                                    action: function (data) {
                                        //detailModal.open({ id: data.record.id });
                                        window.location = "https://localhost:44338/Authors/DetailModal?Id=" + data.record.id;
                                    }
                                },
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        //editModal.open({ id: data.record.id });
                                        window.location = "https://localhost:44338/Authors/EditModal?Id=" + data.record.id;
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('Delete', data.record.name);
                                    },
                                    action: function (data) {
                                        
                                        acme.bookStore.authors.author
                                            .delete(data.record.id)
                                            .then(function (data) {
                                                if (data) {
                                                    abp.notify.info(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                }
                                                else {
                                                    //abp.notify.info(l('UnsuccessfullyDeleted'));
                                                    abp.message.error(l("NotifyDeleteAuthor"));
                                                }
                                                
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
        //createModal.open();
        window.location = "https://localhost:44338/Authors/CreateModal";
    });

    $("input[name='Search'").keyup(function () {
        
        dataTable.ajax.reload();
        console.log(getFilter);
    });

    $("#from-datepicker").datepicker({
        format: 'yyyy-mm-dd' //can also use format: 'dd-mm-yyyy'     
    });

    
});
function ChangeStatus(id, status) {
    if ($('#' + id).is(':checked')) {
        $("#" + id).prop("checked", false);
    }
    else {
        $("#" + id).prop("checked", true);
    }
    dataTable.ajax.reload();

    var mess = l('BlockTheAuthor');
    if (status == 0) {
        mess = l('UnblockTheAuthor');
    }
    
    abp.message.confirm(mess,l('Notify'))
        .then(function (confirmed) {
            
            if (confirmed) {
                acme.bookStore.authors.author.changeStatus(id)
                abp.message.success(l('YourChangesHaveBeenSuccessfullySaved'), l('Congratulations'));
                dataTable.ajax.reload();
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


function change(el) {
    var max_len = 255;
    if (el.value.length > max_len) {
        el.value = el.value.substr(0, max_len);
    }
    document.getElementById('char_cnt').innerHTML = el.value.length;
    document.getElementById('chars_left').innerHTML = max_len - el.value.length;
    return true;
}

function checkDate() {
    var ngay = $("input[name='Author.DoB']").val();
    var today = new Date(); 
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    var homnay = new Date(date);
    var dateInput = new Date(ngay);
    if (dateInput >= homnay) {
        document.getElementById('errorDate').innerHTML = l('Input date must be less than today');
        document.getElementById("save").disabled = true;
    }
    else {
        
            document.getElementById('errorDate').innerHTML = '';
            document.getElementById("save").disabled = false;
    }
}
