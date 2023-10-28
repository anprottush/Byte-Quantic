function toggleSidebar() {
  const sidebar = document.getElementById('sidebar');
  const content = document.querySelector('.content');
  sidebar.classList.toggle('open-sidebar');
  content.style.marginLeft = sidebar.classList.contains('open-sidebar') ? '250px' : '0';
}