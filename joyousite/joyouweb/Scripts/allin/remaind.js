!function ($) {

    var create_or_call = function ($dom, name, option, args, cls) {
        return $dom.each(function () {
            var $this = $(this),
        data = $this.data(name),
        options = typeof option == 'object' && option;
            if (!data) // object not created, now create
                $this.data(name, (data = new cls(this, options)));
            if (typeof option == 'string') // call method
                data[option].apply(data, Array.prototype.slice.call(args, 1));
        });
    }

    var Remaind = function (element, options) {
        var self = this,
        $element = this.$element = $(element),
        options = this.options = $.extend({}, $.fn.remaind.defaults, options);
        var a = this.calc();
        self.set();
        setInterval(function () {
            self.set();
        }, 60000);
    };

    Remaind.prototype = {
        calc: function () {
            var self = this,
                $element = this.$element,
                options = this.options,
                seconds = Math.floor((options.deadline.getTime() - new Date().getTime()) / 1000),
                day = Math.floor(seconds / 3600 / 24),
                hour = Math.floor((seconds % (3600 * 24)) / 3600),
                minite = Math.floor((seconds % 3600) / 60);

            return {
                day_0: Math.floor(day / 10),
                day_1: day % 10,
                hour_0: Math.floor(hour / 10),
                hour_1: hour % 10,
                minite_0: Math.floor(minite / 10),
                minite_1: minite % 10
            };
        },
        set: function () {
            var obj = this.calc();
            $("#remaind-day-0").text(obj.day_0);
            $("#remaind-day-1").text(obj.day_1);
            $("#remaind-hour-0").text(obj.hour_0);
            $("#remaind-hour-1").text(obj.hour_1);
            $("#remaind-minite-0").text(obj.minite_0);
            $("#remaind-minite-1").text(obj.minite_1);
        }
    };

    $.fn.remaind = function (option) {
        create_or_call(this, "remaind", option, arguments, Remaind);
    };

    $.fn.remaind.defaults = {
};

} (jQuery);