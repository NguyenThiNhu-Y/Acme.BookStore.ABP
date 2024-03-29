﻿
var getFilterCategory;
var getFilterAuthor;
var getFilter;
$(function () {
    var l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Books/CreateModal',
        scriptUrl: '/Pages/ckeditor.js'
    });
    var editModal = new abp.ModalManager(abp.appPath + 'Books/EditModal');
    //getFilterCategory = function () {
    //    return {
    //        idCategory: $("select[name='CategoryId']").val(),
    //    };
    //};
    //getFilterAuthor = function () {
    //    return {
    //        idAuthor: $("select[name='AuthorId']").val(),
    //    };
    //};
    
    getFilter = function () {
        return {
            filterText: $("input[name='Search']").val(),
            idAuthor: $("select[name='AuthorId']").val(),
            idCategory: $("select[name='CategoryId']").val()
        };
    };
    var dataTable = $('#BooksTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(acme.bookStore.books.book.getList, getFilter),
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
                    title: l("Image"),
                    data: { image: "image", id: "id" },
                    render: function (data) {
                        var img = `<img src="/ImageBooks/${data.image}" id="${data.id}" width="100px" height="100px" onerror="this.onerror=null;this.src='/ImageBooks/imageDefault.jpg'"/>`;
                        return img;
                    }
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
                    data: "price",
                    render: function (data) {
                        return data.toLocaleString('vi-VN', {
                            style: 'currency',
                            currency: 'VND'
                        })
                    }
                },
                //{
                //    title: l('CreationTime'), data: "creationTime",
                //    render: function (data) {
                //        return luxon
                //            .DateTime
                //            .fromISO(data, {
                //                locale: abp.localization.currentCulture.name
                //            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                //    }
                //},
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
                                        return l('Delete', data.record.name);
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
    

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewBookButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $("select[name='CategoryId']").change(function () {
        dataTable.ajax.reload();
        console.log(getFilterCategory);
    });
    $("select[name='AuthorId']").change(function () {
        dataTable.ajax.reload();
        console.log(getFilterCategory);
    });
    $("input[name='Search'").keyup(function () {
        dataTable.ajax.reload();
        console.log(getFilter);
    });
});

//function format(n, currency) {
//    return currency + n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
//}