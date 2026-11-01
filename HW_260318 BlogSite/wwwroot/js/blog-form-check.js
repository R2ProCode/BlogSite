$(() => {
    $("input, textarea").on("input", function () {
        
        ensureFormValidity();
    });

    $("form").on("submit", function () {
       $("#btn-submit").prop('disabled', true);
    });

    function ensureFormValidity() {
        const isFormValid = $("#title").val().trim().length > 0 && $("#text").val().trim().length > 0;

        $(".comment-btn").prop("disabled", !isFormValid);
    }

}); 