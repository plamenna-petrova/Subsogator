// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const pageSizeSelect = document.querySelector(".page-size-select");
const searchTermInput = document.querySelector(".search-term-input");
const changePageSizeButton = document.querySelector(".change-page-size-button");

changePageSizeButton.addEventListener('click', () => {
    let currentFilter = searchTermInput.value;
    let currentFilterQueryParameter = currentFilter !== ""
        ? `currentFilter=${currentFilter}`
        : "";
    let pageSizeQueryParameter = `pageSize=${pageSizeSelect.value}`;
    let redirectWithOptions = `${changePageSizeButton.href}?${currentFilterQueryParameter}&${pageSizeQueryParameter}`;
    changePageSizeButton.href = redirectWithOptions;
});
