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

window.downloadFile = (filename, base64) => {

    const link = document.createElement('a');

    link.href = "data:application/pdf;base64," + base64;

    link.download = filename;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);
};

window.closeModal = (id) => {
    const modal = bootstrap.Modal.getInstance(
        document.getElementById(id)
    );
    modal?.hide();
};