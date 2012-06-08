!function($){

var create_or_call = function($dom, name, option, args, cls) {
	return $dom.each(function() {
		var $this = $(this),
			data = $this.data(name),
			options = typeof option == 'object' && option;
		if (!data) // object not created, now create
			$this.data(name, (data = new cls(this, options)));
		if (typeof option == 'string') // call method
			data[option].apply(data, Array.prototype.slice.call(args, 1));
	});
}

var Ztree = function(element, options) {
	var $element = this.$element = $(element),
		defaults = $.fn.ztree.defaults,
		options = this.options = $.extend(true, {}, defaults, options);

	if (!$element.hasClass('ztree'))
		$element.addClass('ztree');
	this.ztree = $.fn.zTree.init($element, options.setting, options.nodes);
};

Ztree.prototype = {
};

$.fn.ztree = function(option) {
	var args = arguments;
	return this.each(function() {
		var $this = $(this),
			data = $this.data('ztree'),
			options = typeof option == 'object' && option;
		if (!data) // ztree not created, now create
			$this.data('ztree', (data = new Ztree(this, options)));
		if (typeof option == 'string') // call method
			data.ztree[option].apply(data.ztree, Array.prototype.slice.call(args, 1));
	});
}

$.fn.ztree.defaults = {
	setting: {
		data: {
			simpleData: {
				enable: true,
				idKey: "id",
				pIdKey: "parent_id"
			}
		}
	}
};

var DropDownTree = function(container, options) {
	var $container = this.$container = $(container),
		$element = this.$element = $container.find("ul.ztree"),
		defaults = $.fn.ddtree.defaults,
		options = this.options = $.extend(true, {}, defaults, options),
		nodes = options.nodes || {},
		ztree = this.ztree = $.fn.zTree.init($element, options.setting, options.nodes);
	ztree.expandAll(true);

}

DropDownTree.prototype = {
	show: function() {
		var host = this.options.host,
			offset = host.offset(),
			self = this;
		this.$container.css({
			left: offset.left,
			top: offset.top + host.outerHeight()
		}).slideDown("fast");
		this.on_body_down_handler = function(e) {
			self.on_body_down(e);
		}
		$("body").bind("mousedown", this.on_body_down_handler);
	},
	hide: function() {
		this.$container.fadeOut("fast");
		$("body").unbind("mousedown", this.on_body_down_handler);
	},
	on_body_down: function(e) {
		var cid = this.$container.attr("id");
		if (!(e.target.id == cid || e.target.id == this.$element.attr("id")
			  || $(e.target).parents("#"+cid).length > 0)) {
			this.hide();
		}
	},
	on_click: function(tree_node) {
		var outputs = this.options.outputs;
		for (var i = 0; i < outputs.length; i++) {
			var output = outputs[i];
			output.dom.val(tree_node[output.key]);
		}
	}
}

$.fn.ddtree = function(option) {
	return create_or_call(this, "ddtree", option, arguments, DropDownTree);
}

$.fn.ddtree.defaults = {
	setting: {
		view: {
			dbClcikExpand: false,
			showLine: true,
			selectedMulti: false
		},
		data: {
			simpleData: {
				enable: true,
				idKey: "id",
				pIdKey: "parent_id"
			}
		},
		callback: {
			onClick: function(e, tree_id, tree_node) {
				$("#"+tree_id).parent().ddtree("on_click", tree_node);
			}
		}
	},
	default_id: -1
};

}(jQuery);
