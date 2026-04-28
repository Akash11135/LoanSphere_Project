document.addEventListener("DOMContentLoaded", () => {
    const banners = document.querySelectorAll("[data-auto-dismiss='true']");

    banners.forEach((banner) => {
        window.setTimeout(() => {
            banner.classList.add("is-hiding");
            window.setTimeout(() => banner.remove(), 260);
        }, 3600);
    });
});
