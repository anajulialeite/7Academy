// Alterna a visualização de campos de senha (olhinho / olhinho cortado)
function togglePasswordVisibility(button) {
    const wrapper = button.closest('.password-field-wrapper');
    if (!wrapper) return;

    const input = wrapper.querySelector('input');
    const icon = button.querySelector('i');
    if (!input || !icon) return;

    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('bi-eye');
        icon.classList.add('bi-eye-slash');
        button.setAttribute('aria-label', 'Ocultar senha');
        button.setAttribute('title', 'Ocultar senha');
    } else {
        input.type = 'password';
        icon.classList.remove('bi-eye-slash');
        icon.classList.add('bi-eye');
        button.setAttribute('aria-label', 'Mostrar senha');
        button.setAttribute('title', 'Mostrar senha');
    }
}
