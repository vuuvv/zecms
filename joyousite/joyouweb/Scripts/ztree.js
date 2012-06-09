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
		},
		edit: {
			enable: true
		},
		check: {
			enable: true,
			nocheckInherit: true
		},
		view: {
		},
		callback: {
		}
	}
};

var DropDownTree = function(container, options) {
	var $container = this.$container = $(container),
		$element = this.$element = $container.find("ul.ztree"),
		defaults = $.fn.ddtree.defaults,
		options = this.options = $.extend(true, {}, defaults, options),
		nodes = options.nodes || {};

		this.create(nodes);
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

	create: function(nodes) {
		var options = this.options;

		options.nodes = nodes;
		this.ztree = $.fn.zTree.init(this.$element, options.setting, nodes).expandAll(true); 
	},

	clear: function() {
		var options = this.options,
			outputs = options.outputs,
			id = options.default_id;
		outputs["id"].val(id);
		outputs["name"].val("");
		return false;
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
		for (var key in outputs) {
			var output = outputs[key];
			output.val(tree_node[key]);
		}
	}
}

$.fn.ddtree = function(option) {
	return create_or_call(this, "ddtree", option, arguments, DropDownTree);
};

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

var DBTree = function(element, options) {
	var self = this;
	var dbtree_setting = {
		data: {
			simpleData: {
				enable: true,
				idKey: "id",
				pIdKey: "parent_id"
			}
		},
		edit: {
			enable: true
		},
		check: {
			enable: true,
			nocheckInherit: true
		},
		view: {
			addHoverDom: function(tree_id, tree_node) {
				self.add_hover_dom(tree_id, tree_node);
			},
			removeHoverDom: function(tree_id, tree_node) {
				self.remove_hover_dom(tree_id, tree_node);
			},
			nameIsHTML: true
		},
		callback: {
			onClick: function(tree_id, tree_node) {
				self.on_click(tree_id, tree_node);
			},
			beforeRemove: function(tree_id, tree_node) {
				return confirm("Confirm delete page '" + tree_node.name + "' it?");
			},
			onRename: function(tree_id, tree_node) {
				self.on_rename(tree_id, tree_node);
			},
			onRemove: function(tree_id, tree_node) {
				self.on_remove(tree_id, tree_node);
			}
		}
	};
	var $element = this.$element = $(element),
		defaults = $.fn.ddtree.defaults,
		options = this.options = $.extend(true, {}, defaults, options),
		nodes = options.nodes || {};
	options.setting = $.extend(true, {}, dbtree_setting, options.setting);
	this.create(nodes);
};

DBTree.prototype = {
	create: function(nodes) {
		var options = this.options;

		options.nodes = nodes;
		this.ztree = $.fn.zTree.init(this.$element, options.setting, nodes).expandAll(true); 
	},

	to_add_node: function() {
	},

	to_update_node: function() {
	},

	add_hover_dom: function(tree_id, tree_node) {
		var self = this,
			span = $("#" + tree_node.tId + "_span");

		if (tree_node.editNameFlag || $("#add-btn-"+tree_node.id).length > 0)
			return;
		span.append("<span class='button add' id='add-btn-" + tree_node.id
			+ "' title='add node' onfocus='this.blur();'></span>");
		var btn = $("#add-btn-" + tree_node.id);
		if (btn) {
			btn.bind("click", function() {
				self.to_add_node(tree_id, tree_node);
				return false;
			});
		}
	},

	remove_hover_dom: function(tree_id, tree_node) {
		$("#add-btn-" + tree_node.id).unbind().remove();
	},

	on_remove: function(tree_id, tree_node) {
		$.ajax({
			url: "/admin/admin.ashx?action=page_delete&id=" + tree_node.id
		});
	},

	on_rename: function(tree_id, tree_node) {
		$.ajax({
			url: "/admin/admin.ashx?action=page_rename",
			data: {
				id: tree_node.id,
				title: tree_node.name
			}
		});
	},

	on_click: function(tree_id, tree_node) {
		to_update_node(tree_id, tree_node);
	}
};

$.fn.dbtree = function(option) {
	return create_or_call(this, "dbtree", option, arguments, DBTree);
};

$.fn.dbtree.defaults = {
}


}(jQuery);

