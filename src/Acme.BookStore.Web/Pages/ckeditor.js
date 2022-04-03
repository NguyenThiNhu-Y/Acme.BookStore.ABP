$(function () {

    el = document.getElementsByName("Category.Describe");
    id = el[0].dataset.id;
    var editor = CKEDITOR.replace(id);
    CKFinder.setupCKEditor(editor, '/ckfinder/');
})