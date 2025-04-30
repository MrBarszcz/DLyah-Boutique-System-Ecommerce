$(document).ready(function () {
    $('.btn-total-products').click(function () {
        $('#modalProductCategory').modal('show');
    });
    
    $('.btn-delete-category').click(function () {
        var categoryId = $(this).attr('category-id'); // Get the category ID when clicking the button

        // $.ajax({ // AJAX request that returns a PartialView with the information to delete the category
        //     type: "GET",
        //     url: "/ControlPanel/KillCategory/" + categoryId, 
        //     success: function(result){
        //         $("#KillConfirmationCategory").html(result);
        //         $('#modalDeleteCategory').modal('show');
        //     }
        // });

        $('#modalDeleteCategory').modal('show');
        
    })
})