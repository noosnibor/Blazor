/*Auto scroll page*/
window.scrollToElement = (id) => {
    const el = document.getElementById(id);
    if (el) {
        el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
};

window.closeModal = (id) => {
    const modal = bootstrap.Modal.getInstance(
        document.getElementById(id)
    );
    modal?.hide();
};