$(document).ready(function() {
    // Sidebar toggle
    $('.sidebar-toggle').click(function() {
        $('body').toggleClass('sidebar-open');
    });
    
    // Submenu toggle
    $('.menu-has-children').click(function(e) {
        e.preventDefault();
        $(this).find('.submenu-toggle').toggleClass('rotate');
        $(this).next('.submenu').slideToggle();
    });
    
    // Handle window resize
    $(window).resize(function() {
        if ($(window).width() > 992) {
            $('body').removeClass('sidebar-open');
        }
    });
    
    // Stat card menu toggle
    $('.stat-menu-btn').click(function(e) {
        e.stopPropagation();
        $('.stat-menu').not($(this).next('.stat-menu')).removeClass('open');
        $(this).next('.stat-menu').toggleClass('open');
    });
    
    // Close stat menu when clicking outside
    $(document).click(function() {
        $('.stat-menu').removeClass('open');
    });
    
    // Toggle dark mode
    $('.navbar-buttons .btn-icon:first-child').click(function() {
        $('body').toggleClass('dark-mode');
        $(this).find('i').toggleClass('fa-moon fa-sun');
    });
});