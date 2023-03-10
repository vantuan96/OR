/*global $*/
/*jshint unused:false,forin:false*/

var iosOverlay = function(params) {

	"use strict";

	var overlayDOM;
	var shadow; 
	var noop = function() {};
	var defaults = {
		onbeforeshow: noop,
		onshow: noop,
		onbeforehide: noop,
		onhide: noop,
		text: "",
		icon: null,
		spinner: null,
		duration: null,
		id: null,
		parentEl: null
	};

	// helper - merge two objects together, without using $.extend
	var merge = function (obj1, obj2) {
		var obj3 = {};
		for (var attrOne in obj1) { obj3[attrOne] = obj1[attrOne]; }
		for (var attrTwo in obj2) { obj3[attrTwo] = obj2[attrTwo]; }
		return obj3;
	};

	// helper - does it support CSS3 transitions/animation
	var doesTransitions = (function() {
		var b = document.body || document.documentElement;
		var s = b.style;
		var p = 'transition';
		if (typeof s[p] === 'string') { return true; }

		// Tests for vendor specific prop
		var v = ['Moz', 'Webkit', 'Khtml', 'O', 'ms'];
		p = p.charAt(0).toUpperCase() + p.substr(1);
		for(var i=0; i<v.length; i++) {
			if (typeof s[v[i] + p] === 'string') { return true; }
		}
		return false;
	}());

	// setup overlay settings
	var settings = merge(defaults,params);

	// 
	var handleAnim = function(anim) {
		if (anim.animationName === "ios-overlay-show") {
			settings.onshow();
		}
		if (anim.animationName === "ios-overlay-hide") {
			destroy();
			settings.onhide();
		}
	};

	// IIFE
	    var create = (function () {
	        if (settings.shadow) {
	            shadow = document.createElement("div");
	            shadow.className = "ui-ios-overlay-shadow";
	        }
	    // initial DOM creation and event binding
		overlayDOM = document.createElement("div");
		overlayDOM.className = "ui-ios-overlay";
	        //overlayDOM.innerHTML += '<span class="title">' + settings.text + '</span';
		overlayDOM.innerHTML += '';
		if (params.icon) {
		    overlayDOM.innerHTML += "<div class='icon-holder'><div class='host1'>" +
		    "<div class='loading-a loading-0'></div>" +
            "<div class='loading-a loading-1'></div>" +
            "<div class='loading-a loading-2'></div>" + "</div></div>";
		} else if (params.spinner) {
			overlayDOM.appendChild(params.spinner.el);
		}
		if (doesTransitions) {
			overlayDOM.addEventListener("webkitAnimationEnd", handleAnim, false);
			overlayDOM.addEventListener("msAnimationEnd", handleAnim, false);
			overlayDOM.addEventListener("oAnimationEnd", handleAnim, false);
			overlayDOM.addEventListener("animationend", handleAnim, false);
		}
		    if (settings.shadow) {
		        if (params.parentEl) {
		                    document.getElementById(params.parentEl).appendChild(shadow);
		                    shadow.appendChild(overlayDOM);

		        } else {
		                    document.body.appendChild(shadow);
		                    shadow.appendChild(overlayDOM);
		        }
		    }
		    else {
		        if (params.parentEl) {
		            document.getElementById(params.parentEl).appendChild(overlayDOM);
		
		        } else {
		            document.body.appendChild(overlayDOM);
		        }
		    }
		
		
		settings.onbeforeshow();
		// begin fade in
		if (doesTransitions) {
			overlayDOM.className += " ios-overlay-show";
		} else if (typeof $ === "function") {
			$(overlayDOM).fadeIn({
				duration: 200,
				complete: function() {
					settings.onshow();
				}
			});
		}

		if (settings.duration) {
			window.setTimeout(function() {
				hide();
			},settings.duration);
		}

	}());

	var hide = function() {
		// pre-callback
		settings.onbeforehide();
		// fade out
		if (doesTransitions) {
			// CSS animation bound to classes
		        overlayDOM.className = overlayDOM.className.replace("show", "hide");
		        if (settings.shadow) {
		            shadow.className = shadow.className.replace("show", "hide");
		        }
		    
		} else if (typeof $ === "function") {
			// polyfill requires jQuery
			$(overlayDOM).fadeOut({
				duration: 200,
				complete: function() {
					destroy();
					settings.onhide();
				}
			});

			    if (settings.shadow) {
			        $(shadow).fadeOut({
			            duration: 200
			        });
		}
		    }
	};

	var destroy = function () {
	        if (settings.shadow) {
		        
			    document.body.removeChild(shadow);
		       
	        }
	        else {
	            if (params.parentEl) {
	                document.getElementById(params.parentEl).removeChild(overlayDOM);
	            } else {
	                document.body.removeChild(overlayDOM);
	            }
	        }
		
	};

	var update = function(params) {
		if (params.text) {
			overlayDOM.getElementsByTagName("span")[0].innerHTML = params.text;
		}
		if (params.icon) {
			if (settings.spinner) {
				settings.spinner.el.parentNode.removeChild(settings.spinner.el);
			}
			var iconHolder = overlayDOM.getElementsByTagName("div")[0];
			if (iconHolder) {
				iconHolder.parentNode.removeChild(iconHolder);
			}
			overlayDOM.innerHTML += "<div class='icon-holder'><div class='host1'>" +
		    "<div class='loading-a loading-0'></div>" +
            "<div class='loading-a loading-1'></div>" +
            "<div class='loading-a loading-2'></div>" + "</div></div>";
		}
	};

	return {
		hide: hide,
		destroy: destroy,
		update: update
	};

};
