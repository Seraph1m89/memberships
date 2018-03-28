$(function () {
    var self = $(".panel-carret");
    $(".panel-carret").on("click",
        function (e) {
            self.toggleClass("pressed");
            self.children(".glyphicon-play").toggleClass("gly-rotate-90");
            e.preventDefault();
        });
});