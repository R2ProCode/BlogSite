$(() => {
    $("input, textarea").on("input", function () {
        const isFormValid = $("#commenterName").val().trim().length > 0 && $("#text").val().trim().length > 0;

        $(".comment-btn").prop("disabled", !isFormValid);
    });
}); 