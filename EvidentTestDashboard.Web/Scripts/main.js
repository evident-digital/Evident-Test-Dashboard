function tooltip() {
    $('[title]').tooltip({
        position: {
            my: "center bottom-20",
            at: "center top",
            using: function (position, feedback) {
                $(this).css(position);
                $("<div>")
                  .addClass("arrow")
                  .addClass(feedback.vertical)
                  .addClass(feedback.horizontal)
                  .appendTo(this);
            }
        }
    });
};

function checkEmptyPart() {
    $(".parts").each(function () {
        if ($(this).children().length == 0) {
            $(this).prev('.info').hide();
            $(this).hide();
        } else {
        }
    });
};