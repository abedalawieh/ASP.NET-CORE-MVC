var dataTable;
$(document).ready(function () {
    loudProducts();
});

function loudProducts() {
   dataTable= $('#tblData').DataTable({
        "ajax": {
            "url": '/Admin/Product/Getall',
        },
        "columns": [
            { data: 'title', "width": "20%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id', "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert/${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <button onclick="Delete(${data})" class="btn btn-danger">
                            <i class="bi bi-trash-fill"></i> Delete
                        </button>
                    </div>`;
                }, "width": "25%"
            },
        ]
    });
};

function Delete(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/admin/product/delete/${id}`,
                type: "DELETE",
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
