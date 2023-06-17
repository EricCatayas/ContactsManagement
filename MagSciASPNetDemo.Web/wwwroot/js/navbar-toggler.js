const togglerButton = document.querySelector('.navbar-toggler');

// Get the target collapse element
const navbarCollapse = document.querySelector('.collapse.navbar-collapse');

// Add event listener to the toggler button
togglerButton.addEventListener('click', function () {
    // Toggle the "show" class on the navbarCollapse element
    navbarCollapse.classList.toggle('show');
});