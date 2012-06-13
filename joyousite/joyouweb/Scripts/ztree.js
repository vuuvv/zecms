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
		async: {
			enable: true,
			url: '/admin/admin.ashx?action=page_tree&id=-1'
		},
		data: {
			key: {
				name: "title"
			},
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
			onClick: function(event, tree_id, tree_node) {
				self.on_click(tree_id, tree_node);
			},
			beforeRemove: function(tree_id, tree_node) {
				return confirm("Confirm delete page '" + tree_node.name + "' it?");
			},
			onRename: function(event, tree_id, tree_node) {
				self.on_rename(tree_id, tree_node);
			},
			onRemove: function(event, tree_id, tree_node) {
				self.on_remove(tree_id, tree_node);
			}
		}
	};
	var $element = this.$element = $(element),
		defaults = $.fn.dbtree.defaults,
		options = this.options = $.extend(true, {}, defaults, options);
	options.setting = $.extend(true, {}, dbtree_setting, options.setting);
	this.create();
};

DBTree.prototype = {
	create: function() {
		var options = this.options;

		this.ztree = $.fn.zTree.init(this.$element, options.setting);
		this.ztree.expandAll(true);
	},

	to_add_node: function(tree_id, tree_node) {
		var p = tree_node.getParentNode(),
			form = $("page-form");
		form.find("legend").text("New Page");
		form.find("[name=action]").val("page-add");
		if (p) {
			form.find("[name=parent_id]").val(p.id);
			form.find("[name=parent]").val(p.name);
		} else {
			form.find("[name=parent_id]").val(-1);
			form.find("[name=parent]").val("");
		}
	},

	to_update_node: function(tree_id, tree_node) {
		$.ajax({
			url: "/admin/admin.ashx?action=page_item",
			data: {
				id: tree_node.id
			},
			success: function(data) {
				var form = $("#page-form");
				form.find("legend").text("Update Page: " + data.title);
				form.find("[name=action]").val("page-update");
				form.find("[name=id]").val(data.id);
				form.find("[name=title]").val(data.title);
				form.find("[name=content]").val(data.content);
				form.find("[name=parent_id]").val(data.parent_id);
				form.find("[name=parent]").val(data.parent);
			}
		});
	},

	refresh: function() {
		this.ztree.reAsyncChildNodes(null, "refresh");
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
		this.to_update_node(tree_id, tree_node);
	}
};

$.fn.dbtree = function(option) {
	return create_or_call(this, "dbtree", option, arguments, DBTree);
};

$.fn.dbtree.defaults = {
}

var Form = function(element, options) {
	var options = this.options = $.extend(true, {}, $.fn.form.defaults, options),
		fields = this.fields = options.fields || {};
};

$.fn.form = function(option) {
	return create_or_call(this, "dbtree", option, arguments, DBTree);
}

$.fn.form.defaults = {
}

}(jQuery);

