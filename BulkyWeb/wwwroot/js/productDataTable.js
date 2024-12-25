var dataTable;

$(document).ready(() => LoadDataTable());

function LoadDataTable() {
    dataTable = $('#ProductTable').DataTable({
        "ajax": { url: "/admin/api/product/getall" },
        "columns": [
            { data: "title", "width": "30%" },
            { data: "author", "width": "15%" },
            { data: "priceList", "width": "10%", render: $.fn.dataTable.render.number(',', '.', 2, 'R$ ') },
            { data: "isbN13", "width": "15%", },
            { data: "category.name", width: "15%" },
            {
                data: "id",
                "width": "10%",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a class="btn btn-outline-warning mx-1"
                            href="/admin/product/upsert/?id=${data}"
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <a class="btn btn-outline-danger mx-1"
                            onClick=DeleteConfirmation("/admin/api/product/delete?id=${data}")
                           <i class="bi bi-trash"></i> Delete
                        </a>
                    </div>
                    `
                }
            }
        ],
        "responsive": true
    });
}

function DeleteConfirmation(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: 'rgb(252, 57, 57)',
        cancelButtonColor: 'rgb(0, 156, 220)',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: (data) => {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

