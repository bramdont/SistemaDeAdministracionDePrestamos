$(function () {

    var getPage = function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            type: "get"
        };

        $.ajax(options).done(function (data) {
            var target = $a.parents("div.pagedList").attr("data-otf-target");
            var $newHtml = $(data)
            $(target).replaceWith($newHtml);
            $newHtml.effect("highlight");
        });
        return false;
    };

    $(".container-fluid").on("click", ".pagedList a", getPage);
});