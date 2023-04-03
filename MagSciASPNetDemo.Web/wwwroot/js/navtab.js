/* HTML
    <ul class="nav nav-tabs mb-3" id="ex1" role="tablist">
    <li class="nav-item" role="presentation">
        <a class="nav-link active"
           id="ex1-tab-1"
           data-mdb-toggle="tab"
           href="#ex1-tabs-1"
           role="tab"
           aria-controls="ex1-tabs-1"
           aria-selected="true">Tab 1</a>
    </li>
    <li class="nav-item" role="presentation">
        <a class="nav-link"
           id="ex1-tab-2"
           data-mdb-toggle="tab"
           href="#ex1-tabs-2"
           role="tab"
           aria-controls="ex1-tabs-2"
           aria-selected="false">Tab 2</a>
    </li>
    </ul>
    <!-- Tabs navs -->
    <!-- Tabs content -->
    <div class="tab-content" id="ex1-content">
        <div class="tab-pane fade show active" id="ex1-tabs-1" role="tabpanel" aria-labelledby="ex1-tab-1">
            Tab 1 content
        </div>
        <div class="tab-pane fade" id="ex1-tabs-2" role="tabpanel" aria-labelledby="ex1-tab-2">
            Tab 2 content
        </div>
    </div>
*/
// Get the nav tabs element
const navTabs = document.querySelector('#ex1');

// Get the tabs content element
const tabsContent = document.querySelector('#ex1-content');

// Add event listener to the nav tabs element
navTabs.addEventListener('click', e => {
    e.preventDefault(); // Prevent the default action of clicking on a link

    // Remove the 'active' class from all nav links
    navTabs.querySelectorAll('.nav-link').forEach(link => {
        link.classList.remove('active');
    });

    // Add the 'active' class to the clicked nav link
    const clickedLink = e.target.closest('.nav-link');
    clickedLink.classList.add('active');

    // Hide all tab panes
    tabsContent.querySelectorAll('.tab-pane').forEach(tabPane => {
        tabPane.classList.remove('show', 'active');
    });

    // Show the corresponding tab pane
    const targetPaneId = clickedLink.getAttribute('href');
    const targetPane = tabsContent.querySelector(targetPaneId);
    targetPane.classList.add('show', 'active');
});