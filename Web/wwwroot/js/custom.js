$(function() {
    "use strict";
	
	//Loader
	$(window).on('load', function() {
		$(".Loader").fadeOut("slow");;
	});

    var o = function() {
        var o = 390,
            n = (window.innerHeight > 0 ? window.innerHeight : this.screen.height) - 1;
        n -= o, 1 > n && (n = 1), n > o && $(".page-wrapper").css("min-height", n + "px")
    };
	
    $(window).on('ready', o), $(window).on("resize", o), $(function() {
        $('[data-toggle="tooltip"]').tooltip()
    }), $(function() {
        $('[data-toggle="popover"]').popover()
    }), jQuery(document).on("click", ".nav-dropdown", function(o) {
        o.stopPropagation()
    }), jQuery(document).on("click", ".navbar-nav > .dropdown", function(o) {
        o.stopPropagation()
    }), $(".dropdown-submenu").on('click', function() {
        $(".dropdown-submenu > .dropdown-menu").toggleClass("show")
    }), $("body").trigger("resize");
    var n = $(window);
    n.on("load", function() {
        var o = n.scrollTop(),
            e = $(".topbar");
        o > 100 ? e.addClass("fixed-header animated slideInDown") : e.removeClass("fixed-header animated slideInDown")
    }), $(window).on('scroll', function() {
        $(window).scrollTop() >= 200 ? ($(".topbar").addClass("fixed-header animated slideInDown"), $(".bt-top").addClass("visible")) : ($(".topbar").removeClass("fixed-header animated slideInDown"), $(".bt-top").removeClass("visible"))
    }), AOS.init(), $(".bt-top").on("click", function(o) {
        o.preventDefault(), $("html,body").animate({
            scrollTop: 0
        }, 700)
    })
	
	// Jobs
	$("#job-slide").owlCarousel({
		loop:true,
		autoplay:true,
		nav:false,
		dots:false,
		margin:0,
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
				nav:false
			},
			600:{
				items:2,
				nav:false
			},
			1000:{
				items:3,
				nav:false,
				loop:false
			}
		}
	})
	
	// Jobs
	$("#testimonial-3-slide").owlCarousel({
		loop:true,
		autoplay:true,
		nav:false,
		dots:true,
		margin:0,
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
				nav:false
			},
			600:{
				items:1,
				nav:false
			},
			1000:{
				items:1,
				nav:false,
				loop:false
			}
		}
	})
	
	// Jobs
	$("#agency-slide").owlCarousel({
		loop:true,
		autoplay:true,
		nav:false,
		dots:true,
		margin:0,
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
				nav:false
			},
			600:{
				items:2,
				nav:false
			},
			1000:{
				items:3,
				nav:false,
				loop:false
			}
		}
	})
	
	// RL List
	$("#rl-list").owlCarousel({
		loop:true,
		autoplay:true,
		nav:false,
		dots:true,
		margin:0,
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
				nav:false
			},
			600:{
				items:1,
				nav:false
			},
			1000:{
				items:2,
				nav:false,
				loop:false
			}
		}
	})
	
	// Testimonials 2
	$("#testimonials-two").owlCarousel({
		nav:true,
		dots:false,
		items: 1,
		center:false,
		loop: !0,
		navText: ['<i class="fa fa-arrow-left"></i>', '<i class="fa fa-arrow-right"></i>'],
		responsive: {
			0: {
				items:1,
				stagePadding: 0,
				margin: 0
			},
			768: {
				items:1,
				stagePadding: 150,
				margin:50
			},
			1025: {
				stagePadding:280,
				margin:10
			},
			1700: {
				items:1,
				stagePadding:280,
				margin:120
			}
		}
	})
	
	// All Select Category
	$('#category').select2({
		placeholder: "Choose Category...",
		allowClear: true
	});
	
	// All Select Category
	$('#category-2').select2({
		placeholder: "Choose Category...",
		allowClear: true
	});
	
	// Filter Sidebar Category
	$('#category-3').select2({
		placeholder: "Choose Category...",
		allowClear: true
	});
	
	// All Search
	$('#search-allow').select2({
		placeholder: "Search Allow",
		allowClear: true
	});
	
	// Job type
	$('#jb-type').select2({
		placeholder: "Job type",
		allowClear: true
	});
	
	// Career  Lavel
	$('#career-lavel').select2({
		placeholder: "Career Level",
		allowClear: true
	});
	
	// Offerd Salary
	$('#offerd-sallery').select2({
		placeholder: "Offerd Salary",
		allowClear: true
	});
	
	// Experience
	$('#experience').select2({
		placeholder: "Please Select",
		allowClear: true
	});
	
	// Gender
	$('#gender').select2({
		placeholder: "Please Select",
		allowClear: true
	});	
	
	// Industry
	$('#industry').select2({
		placeholder: "Please Select",
		allowClear: true
	});	
	
	// Qualification
	$('#qualification').select2({
		placeholder: "Please Select",
		allowClear: true
	});	
	
	
	// Business Type
	$('#business-type').select2({
		placeholder: "Search Allow",
		allowClear: true
	});
	
	// Search Page Tag & Skill 
	$(".tag-skill").select2({
	  tags: true
	});
	
	// Specialisms 
	$("#specialisms").select2({
		placeholder: "Specialisms"
	});

	// Editor 
	$('#summernote').summernote({
		height: 150
	});
	
	// Editor 
	$('#resume-info').summernote({
		height: 120
	});
	
	// Job Description
	$('#jb-description').summernote({
		height: 150
	});
	
	// CV 
	$('#cv-cover').summernote({
		height: 150
	});
	
	// File upload
	$(".custom-file-input").on("change", function() {
	  var fileName = $(this).val().split("\\").pop();
	  $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
	});
	
	/****----- Counter ---------*/
	$('.count').on('each', function () {
		$(this).prop('Counter',0).animate({
			Counter: $(this).text()
		}, {
			duration: 4000,
			easing: 'swing',
			step: function (now) {
				$(this).text(Math.ceil(now));
			}
		});
	});
	
	
});